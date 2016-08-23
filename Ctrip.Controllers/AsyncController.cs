/*
 * 演示如何利用 .net 4.5 的新特性实现异步操作
 * 
 * 什么场景下需要异步操作？
 * 在因为磁盘io或网络io而导致的任务执行时间长的时候应该使用异步操作
 * 如果任务执行时间长是因为cpu的消耗则应使用同步操作（此时异步操作不会改善任何问题）
 *
 * 原理是什么？
 * 在 Web 服务器上
 * .NET Framework 维护一个用于服务 ASP.NET 请求的线程池
 * 以下把 .NET Framework 维护的用于服务 ASP.NET 请求的线程池称作为“特定线程池”
 * 同步操作时
 * 如果特定线程池利用满了
 * 则不会再提供服务
 *
 * 异步操作时：
 * 1、一个请求过来，特定线程池出一个线程处理此请求
 * 2、启动一个非特定线程池中的另一个线程处理异步操作
 *    此时处理此请求的线程就会空出来
 *    不会被阻塞
 *    它可以继续处理其它请求
 * 3、异步操作执行完毕后
 *    从特定线程池中随便找一个空闲线程返回请求结果
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ctrip.Controllers
{
    public class AsyncController : ApiController
    {
        public async Task<string> Get()
        {
            return await GetData();
        }

        [NonAction]
        public async Task<string> GetData()
        {
            await Task.Delay(10 * 1000);

            return "长时间的任务执行完毕";
        }
    }
}