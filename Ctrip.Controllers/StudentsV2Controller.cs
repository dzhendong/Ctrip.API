using System.Collections.Generic;
using System.Web.Http;
using Ctrip.Model;

namespace Ctrip.Controllers
{
    public class StudentsV2Controller : ApiController
    {
        public IEnumerable<StudentV2BaseModel> Get(int page = 0, int pageSize = 10)
        {
            return new StudentV2BaseModel[] { 
                new StudentV2BaseModel(){
                    Id = 1,
                    Url = "A",
                    FullName = "AAAD",
                    EnrollmentsCount = 10
                }
            };
        }

    }
}
