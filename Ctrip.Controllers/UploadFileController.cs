using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ctrip.Controllers
{
    /// <summary>
    /// 通过 web api 上传文件
    /// </summary>
    public class UploadFileController : ApiController
    {
        public async Task<string> Post()
        {
            // 检查是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            // 设置上传目录
            var provider = new MultipartFormDataStreamProvider(@"c:\\temp");
         
            // 接收数据，并保存文件
            var bodyparts = await Request.Content.ReadAsMultipartAsync(provider);

            string result = "";
            // 获取表单数据
            result += "formData txtName: " + bodyparts.FormData["txtName"];
            result += "<br />";

            // 获取文件数据

            // 上传文件相关的头信息
            result += "fileData headers: " + bodyparts.FileData[0].Headers; 
            result += "<br />";

            // 文件在服务端的保存地址，需要的话自行 rename 或 move
            result += "fileData localFileName: " + bodyparts.FileData[0].LocalFileName; 

            return result;
        }
    }
}
