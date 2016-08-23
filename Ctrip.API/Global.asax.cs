using Ctrip.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Ctrip.API
{    
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {            
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            QueryStringMapping mapping = new QueryStringMapping("json", "true", "application/json");
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(mapping);

            #region Configuration
            ////请求 url 中如果带有参数 xml=true，则返回 xml 数据
            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("xml", "true", "application/xml"));

            ////添加一个转换器 IsoDateTimeConverter，其用于日期数据的序列化和反序列化
            //var dateTimeConverter = new IsoDateTimeConverter();
            //dateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            //JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            //serializerSettings.Converters.Add(dateTimeConverter);

            ////清除全部 Formatter
            ////默认有 4 个
            ////JsonMediaTypeFormatter
            ////XmlMediaTypeFormatter
            ////FormUrlEncodedMediaTypeFormatter
            ////JQueryMvcFormUrlEncodedFormatter
            ////GlobalConfiguration.Configuration.Formatters.Clear();

            ////如果请求 header 中有 accept: text/html 则返回这个新建的 JsonMediaTypeFormatter 数据
            //var jsonFormatter = new JsonMediaTypeFormatter();
            //jsonFormatter.SerializerSettings = serializerSettings;
            //MediaTypeHeaderValue mediaHeader = new MediaTypeHeaderValue("text/html");
            //RequestHeaderMapping mapping1 = new RequestHeaderMapping("accept", "text/html", StringComparison.InvariantCultureIgnoreCase, true, mediaHeader);
            //jsonFormatter.MediaTypeMappings.Add(mapping1);
            //GlobalConfiguration.Configuration.Formatters.Insert(0, jsonFormatter);

            ////请求 url 中如果带有参数 jsonp=true
            ////则返回支持 jsonp 协议的数据（具体实现参见 MyJsonFormatter.cs）
            //MyJsonFormatter formatter = new MyJsonFormatter();
            //formatter.MediaTypeMappings.Add(new QueryStringMapping("jsonp", "true", "application/javascript"));
            //GlobalConfiguration.Configuration.Formatters.Add(formatter);
            #endregion
        }
    }
}