using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecuroteckWebApplication.Models;
using System.Security.Cryptography;

namespace SecuroteckWebApplication.Controllers
{
    public class ProtectedController : ApiController
    {
        [ActionName("Hello")]
        [CustomAuthorise]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            string headerKey = request.Headers.GetValues("ApiKey").FirstOrDefault();
            string userName = null;
            if (headerKey != null)
            {
                UserDatabaseAccess DBAccess = new UserDatabaseAccess();
                bool keyExist = DBAccess.checkUserViaKeyBool(headerKey);
                if (keyExist == true)
                {
                    User user = DBAccess.checkUserViaKeyObj(headerKey);
                    userName = user.UserName;
                    return Request.CreateResponse(HttpStatusCode.OK, "Hello " + userName);

                }
                return Request.CreateResponse(HttpStatusCode.OK, userName);
            }
            return Request.CreateResponse(HttpStatusCode.OK, userName);
        }


        [CustomAuthorise]
        [ActionName("SHA1")]
        [HttpGet]
        public HttpResponseMessage SHA1([FromUri] string request)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            if (request != null)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(request);
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] requestSHA1 = sha.ComputeHash(bytes);
                string requestSHA1String = requestSHA1.ToString();
                return Request.CreateResponse(HttpStatusCode.OK, requestSHA1String);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Bad Request");
        }


        [CustomAuthorise]
        [ActionName("Sha256")]
        [HttpGet]
        public HttpResponseMessage SHA256([FromUri] string request)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            if (request != null)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(request);
                SHA256 sha = new SHA256CryptoServiceProvider();
                byte[] requestSHA256 = sha.ComputeHash(bytes);
                string requestSHA256String = requestSHA256.ToString();
                return Request.CreateResponse(HttpStatusCode.OK, requestSHA256String);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Bad Request");
        }
    }
}
