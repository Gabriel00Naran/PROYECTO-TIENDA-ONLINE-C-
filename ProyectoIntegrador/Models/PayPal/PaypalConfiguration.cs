using System.Collections.Generic;
using PayPal.Api;

namespace ProyectoIntegrador.Controllers
{
    public class PaypalConfiguration
    {
        // traemos la configuracion del paypal.
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"]; //traigo los valores en la configuracion del webConfig
            ClientSecret = config["clientSecret"];//traigo los valores en la configuracion del webConfig

        }
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        // Creamos el acceso de Token generados del Pay pal

        private static string GetAccesToken()
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }


        // aqui retornamos el apiContext

        public static APIContext GetAPIContext()
        {
            var apiContext = new APIContext(GetAccesToken())
            {
                Config = GetConfig()
            };
            return apiContext;
        }

    } 
}

