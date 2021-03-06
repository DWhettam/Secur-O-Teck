﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SecuroteckWebApplication.Models;

namespace SecuroteckWebApplication.Controllers
{
    public class APIAuthorisationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //TODO check apikey exists
            #region Task5           
            var header = request.Headers;
            if (header.Contains("ApiKey"))
            {
                if (UserDatabaseAccess.udb.ApiKeyExists(header.GetValues("ApiKey").First()))
                {
                    UserDatabaseAccess databaseAccess = new Models.UserDatabaseAccess();
                    User user = databaseAccess.ApiKeyExistsReturnUser(header.GetValues("ApiKey").First());
                    Claim claim = new Claim(ClaimTypes.Name, user.UserName);
                    Claim[] claims = new Claim[1];
                    claims[0] = claim;

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "ApiKey");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    Thread.CurrentPrincipal = principal;
                }                
            }
            #endregion
            return base.SendAsync(request, cancellationToken);
        }
    }
}