using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ctrip.Component;
using Ctrip.Model;

namespace Ctrip.Controllers
{
    public class StudentsController : ApiController
    {
        /// <summary>
        /// http://localhost:53695/api/Students/Get?page=0
        /// </summary>
        public IEnumerable<StudentBaseModel> Get(int page = 0, int pageSize = 10)
        {
            return new StudentBaseModel[] { 
                new StudentBaseModel(){
                    Id = 1,
                    Url = "A",
                    FirstName = "A",
                    LastName = "D",
                    EnrollmentsCount = 10
                }
            };
        }

        [CASAuthorize]
        public HttpResponseMessage Get(string userName)
        {
            return Request.CreateResponse(HttpStatusCode.NotModified); 
        }

        public HttpResponseMessage Post([FromBody] Student student)
        {
            return Request.CreateResponse(HttpStatusCode.NotModified); 
        }

        [HttpPatch]
        [HttpPut]       
        public HttpResponseMessage Put(string userName, [FromBody] Student student)
        {
            return Request.CreateResponse(HttpStatusCode.NotModified); 
        }

        public HttpResponseMessage Delete(string userName)
        {
            return Request.CreateResponse(HttpStatusCode.NotModified); 
        }
    }
}
