using Ctrip.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace Ctrip.Controllers
{
    /// <summary>
    /// OAtuth
    /// </summary>
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        /// <summary>
        /// 在Get()方法上加了Authorize标签
        /// 所以此api在没有授权的情况下将返回401 Unauthorize。
        /// 使用postman发个请求试试：
        /// http://localhost:65456/api/order
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public List<OrderModel> Get()
        {
            return OrderModel.CreateOrders();
        }
    }
}