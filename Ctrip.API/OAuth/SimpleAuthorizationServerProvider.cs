using Ctrip.Component;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ctrip.API.Providers
{
    /// <summary>
    /// 向服务器请求token
    /// resource owner password credentials模式需要请求头必须包含3个参数：
    /// grant_type-必须为password
    /// username-用户名
    /// password-用户密码
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// 用来对third party application 认证
        /// </summary>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //具体的做法是为third party application颁发appKey和appSecrect
            //在本例中我们省略了颁发appKey和appSecrect的环节
            //我们认为所有的third party application都是合法的
            //context.Validated()
            //表示所有允许此third party application请求
            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// resource owner password credentials模式的重点
        /// 由于客户端发送了用户的用户名和密码
        /// 所以我们在这里验证用户名和密码是否正确
        /// 
        /// 只有这两个方法同时认证通过才会颁发token
        /// </summary>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (context.UserName == "rranjan" && context.Password == "password@123")
            {
                var claimsIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                claimsIdentity.AddClaim(new Claim("user", context.UserName));
                context.Validated(claimsIdentity);
                return;
            }

            context.Rejected();  

            //二
            //using (AuthRepository _repo = new AuthRepository())
            //{
            //    IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

            //    if (user == null)
            //    {
            //        context.SetError("invalid_grant", "The user name or password is incorrect.");
            //        return;
            //    }
            //}

            ////采用了ClaimsIdentity认证方式
            ////可以把他当作一个NameValueCollection看待
            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            //identity.AddClaim(new Claim("sub", context.UserName));

            //var props = new AuthenticationProperties(new Dictionary<string, string>
            //    {
            //        { 
            //            "as:client_id", context.ClientId ?? string.Empty
            //        },
            //        { 
            //            "userName", context.UserName
            //        }
            //    });

            ////最后context.Validated(ticket)
            ////表明认证通过
            //var ticket = new AuthenticationTicket(identity, props);
            //context.Validated(ticket);
        }

        /// <summary>
        /// 把Context中的属性加入到token中
        /// </summary>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}