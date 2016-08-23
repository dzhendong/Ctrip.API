
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Ctrip.Controllers
{
    /// <summary>
    /// 自定义一个 JsonMediaTypeFormatter，以支持 jsonp 协议
    /// </summary>
    public class MyJsonFormatter : JsonMediaTypeFormatter
    {
        /// <summary>
        /// jsonp 回调的函数名称
        /// </summary>
        private string JsonpCallbackFunction;

        public MyJsonFormatter()
        {
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        /// <summary>
        /// 每个请求都先来这里
        /// </summary>
        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, System.Net.Http.HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            var formatter = new MyJsonFormatter()
            {
                JsonpCallbackFunction = GetJsonCallbackFunction(request)
            };

            // 增加一个转换器，以便枚举值与枚举名间的转换
            formatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            // 增加一个转换器，以方便时间格式的序列化和饭序列化
            var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            dateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            formatter.SerializerSettings.Converters.Add(dateTimeConverter);

            // 排版返回的 json 数据，使其具有缩进格式，以方便裸眼查看
            formatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            return formatter;
        }

        /// <summary>
        /// 序列化的实现
        /// </summary>
        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
        {
            if (string.IsNullOrEmpty(JsonpCallbackFunction))
            {
                return base.WriteToStreamAsync(type, value, stream, content, transportContext);
            }

            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(stream);
                writer.Write(JsonpCallbackFunction + "(");
                writer.Flush();
            }
            catch (Exception ex)
            {
                try
                {
                    if (writer != null)
                        writer.Dispose();
                }
                catch { }

                var tcs = new TaskCompletionSource<object>();
                tcs.SetException(ex);
                return tcs.Task;
            }

            return base.WriteToStreamAsync(type, value, stream, content, transportContext)
                       .ContinueWith(innerTask =>
                            {
                                if (innerTask.Status == TaskStatus.RanToCompletion)
                                {
                                    writer.Write(")");
                                    writer.Flush();
                                }

                            }, TaskContinuationOptions.ExecuteSynchronously)
                        .ContinueWith(innerTask =>
                            {
                                writer.Dispose();
                                return innerTask;

                            }, TaskContinuationOptions.ExecuteSynchronously)
                        .Unwrap();
        }

        /// <summary>
        /// 从请求 url 中获取其参数 callback 的值
        /// </summary>
        private string GetJsonCallbackFunction(HttpRequestMessage request)
        {
            if (request.Method != HttpMethod.Get)
                return null;

            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var queryVal = query["callback"];

            if (string.IsNullOrEmpty(queryVal))
                return null;

            return queryVal;
        }
    }
}