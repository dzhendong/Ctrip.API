
using Ctrip.Test.OAuth;

namespace WebApiSelfHost
{
    /// <summary>
    /// 自宿主 web api 的 demo
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            OAuthClientTest test = new OAuthClientTest();
            test.Get_Accesss_Token_By_Client_Credentials_Grant1();
        }
    }
}
