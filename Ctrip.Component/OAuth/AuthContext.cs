using Ctrip.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Ctrip.Component
{
    /// <summary>
    /// OAuthPractice.ProtectedApi.Auth
    /// 使用ASP.NET Identity 实现一个简单的用户认证功能
    /// 以便我们生成用户名和密码
    /// </summary>
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext():base("AuthContext")
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}