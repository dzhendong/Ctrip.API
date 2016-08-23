using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using Ctrip.Component;
using System.Threading;

namespace Ctrip.Controllers
{
    /// <summary>
    /// http://localhost:53695/api/
    /// </summary>
    public class ActiveController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5      
        public string Get(int id)
        {
            return "api/values/5";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
       
        [ForceHttps()]
        public HttpResponseMessage Post()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not save to the database.");
        }

        /// <summary>
        ///  代码中，我们多用个缓存标记key，双检锁校验。
        ///  它设置为正常时间，过期后通知另外的线程去更新缓存数据。
        ///  而实际缓存由于设置了2倍缓存时间，仍然可以用脏数据给前端展现。
        ///  这样就能提高不少系统吞吐量了。
        /// </summary>      
        //public object GetMemberSigninDays5()
        //{
        //    const int cacheTime = 5;
        //    const string cacheKey = "mushroomsir";

        //    //缓存标记。
        //    const string cacheSign = cacheKey + "_Sign";
        //    var sign = CacheHelper.Get(cacheSign);

        //    //获取缓存值
        //    var cacheValue = CacheHelper.Get(cacheKey);

        //    //未过期，直接返回。
        //    if (sign != null)
        //        return cacheValue; 

        //    lock (cacheSign)
        //    {
        //        sign = CacheHelper.Get(cacheSign);
        //        if (sign != null)
        //            return cacheValue;

        //        CacheHelper.Add(cacheSign, "1", cacheTime);
        //        ThreadPool.QueueUserWorkItem((arg) =>
        //        {
        //            //这里一般是 sql查询数据。 例：395 签到天数
        //            cacheValue = "395";

        //            //日期设缓存时间的2倍，用于脏读。
        //            CacheHelper.Add(cacheKey, cacheValue, cacheTime*2); 
        //        });
        //    }
        //    return cacheValue;
        //}
    }
}