using Ctrip.Component;
using Ctrip.Model;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Ctrip.API.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        /// <summary>
        /// 用来生成RefreshToken值
        /// 生成后需要持久化在数据库中
        /// 客户端需要拿RefreshToken来请求刷新token
        /// </summary>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (AuthRepository _repo = new AuthRepository())
            {
                var token = new RefreshToken()
                {
                    Id = refreshTokenId.GetHash(),
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
            }
        }

        /// <summary>
        /// ReceiveAsync方法将拿客户的RefreshToken和数据库中RefreshToken做对比
        /// 验证成功后删除此refreshToken
        /// </summary>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            string hashedTokenId = context.Token.GetHash();

            using (AuthRepository _repo = new AuthRepository())
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await _repo.RemoveRefreshToken(hashedTokenId);
                }
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}