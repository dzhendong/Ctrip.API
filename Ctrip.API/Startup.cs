using System;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Ctrip.API.Providers;
using Ctrip.Component;

[assembly: OwinStartup(typeof(Ctrip.API.Startup))]
namespace Ctrip.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            config.MapHttpAttributeRoutes();

            //开启了OAuth服务
            ConfigureOAuth(app);

            //这一行代码必须放在ConfiureOAuth(app)之后
            app.Use<HttpConfiguration>(config);

            //通过注册 MigrateDatabaseToLatestVersion 数据库初始值设定项来实现该功能。
            //数据库初始值设定项只是包含用于确保数据库安装正确的某种逻辑。
            //首次在应用程序进程 (AppDomain) 中使用上下文时
            //将运行此逻辑。Demo中执行
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, EFConfiguration>());
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //允许客户端使用http协议请求
                AllowInsecureHttp = true,

                //token请求的地址
                //即http://localhost:端口号/token
                TokenEndpointPath = new PathString("/token"),

                //token过期时间
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(10),

                //提供具体的认证策略
                Provider = new SimpleAuthorizationServerProvider(),

                //刷新的认证策略
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}