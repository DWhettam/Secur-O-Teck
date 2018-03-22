using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Http;
using SecuroteckWebApplication.Controllers;

namespace SecuroteckWebApplication
{
    public static class WebApiConfig
    {
        // Publically accessible global static variables could go here

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            GlobalConfiguration.Configuration.MessageHandlers.Add(new APIAuthorisationHandler());

            #region Task 9
            RSACryptoServiceProvider RSAKey = new RSACryptoServiceProvider();
            #endregion

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "TalkbackApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
