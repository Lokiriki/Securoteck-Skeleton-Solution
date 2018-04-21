using System;
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
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            #region Task5
            // TODO:  Find if a header ‘ApiKey’ exists, and if it does, check the database to determine if the given API Key is valid
            //        Then authorise the principle on the current thread using a claim, claimidentity and claimsprinciple
            string headerKey;
            try
            {
                headerKey = request.Headers.GetValues("ApiKey").FirstOrDefault();
                User validUser;
                if (headerKey != "")
                {
                    UserDatabaseAccess DBAccess = new UserDatabaseAccess();
                    validUser = DBAccess.checkUserViaKeyObj(headerKey);
                    if (validUser != null)
                    {
                        Claim claim = new Claim(ClaimTypes.Name, validUser.UserName);
                        Claim[] singularClaim = new Claim[] { claim };
                        ClaimsIdentity claimIdentity = new ClaimsIdentity(singularClaim, "ApiKey");
                        ClaimsPrincipal principal = new ClaimsPrincipal(claimIdentity);
                        Thread.CurrentPrincipal = principal;
                    }
                }
            }
            catch
            {

            }
            
            #endregion
            return base.SendAsync(request, cancellationToken);
        }
    }
}