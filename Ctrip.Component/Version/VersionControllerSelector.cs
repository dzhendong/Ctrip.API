using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Ctrip.Component
{
    /// <summary>
    /// 版本
    /// </summary>
    public class VersionControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;

        public VersionControllerSelector(HttpConfiguration config)
            : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping();

            var routeData = request.GetRouteData();

            if (string.IsNullOrWhiteSpace(routeData.Route.RouteTemplate))
            {
               return   base.SelectController(request);
            }

            var controllerName = routeData.Values["controller"].ToString();
           
            HttpControllerDescriptor controllerDescriptor;

            if (controllers.TryGetValue(controllerName, out controllerDescriptor))
            {
                var version = GetVersionFromQueryString(request);
                //var version = GetVersionFromHttpHeader(request);
                //var version = GetVersionFromAcceptHeaderVersion(request);

                var versionedControllerName = string.Concat(controllerName, "V", version);

                HttpControllerDescriptor versionedControllerDescriptor;
                if (controllers.TryGetValue(versionedControllerName, out versionedControllerDescriptor))
                {
                    return versionedControllerDescriptor;
                }

                return controllerDescriptor;
            }

            return null;
        }    
       
        /// <summary>
        /// 获取基于MIME类型的版本号
        /// Accept Header设置版本
        /// 直接使用Accept Header
        /// 请求的时候将它设置为“Accept:application/json; version=2”
        /// </summary>        
        private string GetVersionFromAcceptHeaderVersion(HttpRequestMessage request)
        {
            var acceptHeader = request.Headers.Accept;

            foreach (var mime in acceptHeader)
            {
                if (mime.MediaType == "application/json")
                {
                    var version = mime.Parameters
                        .Where(v => v.Name.Equals("version", StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (version != null)
                    {
                        return version.Value;
                    }
                    return "1";
                }
            }
            return "1";
        }

        /// <summary>
        /// 自定义请求头
        /// 不是URI的一部分
        /// 添加一个头“X-Version”并把版本号设置在里面
        /// 当客户端没有这条头信息是我们可以认为它需要V1版本
        /// </summary>        
        private string GetVersionFromHttpHeader(HttpRequestMessage request)
        {
            const string HEADER_NAME = "X-Version";

            if (request.Headers.Contains(HEADER_NAME))
            {
                var versionHeader = request.Headers.GetValues(HEADER_NAME).FirstOrDefault();
                if (versionHeader != null)
                {
                    return versionHeader;
                }
            }

            return "1";
        }

        /// <summary>
        /// http://localhost:{your_port}/api/students/?v=2
        /// </summary>        
        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            var version = query["v"];

            if (version != null)
            {
                return version;
            }

            return "1";
        }
    }
}