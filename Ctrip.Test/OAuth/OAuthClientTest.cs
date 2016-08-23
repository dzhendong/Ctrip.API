using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Ctrip.Test.OAuth
{
    public class OAuthClientTest
    {
        private HttpClient _httpClient;

        public OAuthClientTest()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:53695/");
        }

        /// <summary>
        /// {"access_token":"8PqaWilv_SJT7vRXambP7Mebyaf3KO1GXYHsqA-oPMOQF6xk1YpluczOZGo-WwATU5YmGb0wSR0cUQMC8RSZfwO8nwom7yG11FIANhy2PNiqTg2CYdJF0sf0ggFs6it_i3mc_m1iEFCK2dLBPDJXPI24wngCPR0wP_zugZvyKv314BM0PQmnnwg3kLXR1DISKRbs5-i59VCtFSZgkM7A0w",
        /// "token_type":"bearer",
        /// "expires_in":1209599}
        /// </summary>
        public void Get_Accesss_Token_By_Client_Credentials_Grant1()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("client_id", "1234");
            parameters.Add("client_secret", "5678");
            parameters.Add("grant_type", "client_credentials");

            Console.WriteLine(_httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters))
                .Result.Content.ReadAsStringAsync().Result);
        }

        public void Get_Accesss_Token_By_Client_Credentials_Grant2()
        {
            var clientId = "1234";
            var clientSecret = "5678";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));

            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "client_credentials");

            Console.WriteLine(_httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters))
                .Result.Content.ReadAsStringAsync().Result);
        }
    }
}
