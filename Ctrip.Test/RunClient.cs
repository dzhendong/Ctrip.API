using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Ctrip.Test
{
    /// <summary>
    /// web api
    /// </summary>
    public class HelloController : ApiController
    {
        public string Get()
        {
            return "hello: webabcd";
        }
    }

    public class RunClient
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:123/");

        static void Test1(string[] args)
        {            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53695/");

            using (HttpResponseMessage response = client.GetAsync("v1/values").Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Version 1 response: &apos;{0}&apos;\n", content);
            }

            using (HttpResponseMessage response = client.GetAsync("v2/values").Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Version 2 response: &apos;{0}&apos;\n", content);
            }

            string baseUri = "http://localhost:9000/api/demo/sitelist";
            var requestJson = JsonConvert.SerializeObject(new { startId = 1, itemcount = 3 });

            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            var httpClient = new HttpClient();
            Task<HttpResponseMessage> task = httpClient.PostAsync(baseUri, httpContent);
            var responseJson = task.Result.Content.ReadAsStringAsync().Result;
        }       

        static void Test2(string[] args)
        {
            HttpSelfHostServer server = null;
            try
            {
                // 配置一个自宿主 http 服务
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);

                // 配置 http 服务的路由
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                // 创建 http 服务
                server = new HttpSelfHostServer(config);

                // 开始监听
                server.OpenAsync().Wait();

                // 停止监听
                server.CloseAsync().Wait();

                Console.WriteLine("Listening on " + _baseAddress);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
