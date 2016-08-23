using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Qeeka.Digital.API
{
    /// <summary>
    /// jsonp 支持
    /// </summary>
    public class JsonpMediaTypeFormatter1 : JsonMediaTypeFormatter
    {
        /// <summary>
        /// 代表JavaScript回调函数名称
        /// </summary>
        public string Callback { get; private set; }

        public JsonpMediaTypeFormatter1(string callback = null)
        {
            this.Callback = callback;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            if (string.IsNullOrEmpty(this.Callback))
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }

            try
            {
                this.WriteToStream(type, value, writeStream, content);

                //net 4.5
                AsyncVoid a = new AsyncVoid();
                return Task.FromResult<AsyncVoid>(a);
            }
            catch (Exception exception)
            {
                TaskCompletionSource<AsyncVoid> source = new TaskCompletionSource<AsyncVoid>();
                source.SetException(exception);
                return source.Task;
            }
        }

        private void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            JsonSerializer serializer = JsonSerializer.Create(this.SerializerSettings);

            using (StreamWriter streamWriter = new StreamWriter(writeStream, this.SupportedEncodings.First()))

            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter) { CloseOutput = false })
            {
                jsonTextWriter.WriteRaw(this.Callback + "(");
                serializer.Serialize(jsonTextWriter, value);
                jsonTextWriter.WriteRaw(")");
            }
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            if (request.Method != HttpMethod.Get)
            {
                return this;
            }

            string callback;

            if (request.GetQueryNameValuePairs().ToDictionary(pair => pair.Key,
                pair => pair.Value).TryGetValue("callback", out callback))
            {
                return new JsonpMediaTypeFormatter1(callback);
            }

            return this;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        private struct AsyncVoid
        { }
    }

    public class JsonpMediaTypeFormatter2 : JsonMediaTypeFormatter
    {
        private string _callbackQueryParamter;

        public JsonpMediaTypeFormatter2()
        {
            SupportedMediaTypes.Add(DefaultMediaType);
            SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("text/javascript"));

            MediaTypeMappings.Add(new UriPathExtensionMapping("jsonp", DefaultMediaType));
        }

        public string CallbackQueryParameter
        {
            get { return _callbackQueryParamter ?? "callback"; }
            set { _callbackQueryParamter = value; }
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            string callback;
            if (IsJsonpRequest(out callback))
            {
                return Task.Factory.StartNew(() =>
                {
                    var writer = new StreamWriter(writeStream);
                    writer.Write(callback + "(");
                    writer.Flush();
                    base.WriteToStreamAsync(type, value, writeStream, content, transportContext).Wait();
                    writer.Write(")");
                    writer.Flush();
                });
            }
            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }

        private bool IsJsonpRequest(out string callback)
        {
            callback = null;
            switch (HttpContext.Current.Request.HttpMethod)
            {
                case "POST":
                    callback = HttpContext.Current.Request.Form[CallbackQueryParameter];
                    break;
                default:
                    callback = HttpContext.Current.Request.QueryString[CallbackQueryParameter];
                    break;
            }
            return !string.IsNullOrEmpty(callback);
        }
    }
}
