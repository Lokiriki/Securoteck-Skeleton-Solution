using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecuroteckWebApplication.Models;
using System.Security.Cryptography;
using System.Text;

namespace SecuroteckWebApplication.Controllers
{
    public class ProtectedController : ApiController
    {
        [ActionName("Hello")]
        [CustomAuthorise]
        public IHttpActionResult Get(HttpRequestMessage request)
        {
            //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
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
                    return Ok("Hello " + userName);

                }
                return Ok(userName);
            }
            return Ok(userName);
        }


        [CustomAuthorise]
        [ActionName("SHA1")]
        [HttpGet]
        public IHttpActionResult SHA1([FromUri] string message)
        {        
            if (message != null)
            {
                string SHA1string = null;
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(message));
                    var sb = new StringBuilder(hash.Length * 2);

                    foreach (byte b in hash)
                    {                
                        sb.Append(b.ToString("X2"));
                    }
                    SHA1string = sb.ToString();
                }

                /*    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] requestSHA1 = sha.ComputeHash(bytes);
                string requestSHA1String = requestSHA1.ToString(); */
                return Ok(SHA1string); 
            }
            return Ok("Bad Request");
        }


        [CustomAuthorise]
        [ActionName("SHA256")]
        [HttpGet]
        public IHttpActionResult SHA256([FromUri] string message)
        {
            HttpResponseMessage outMessage = new HttpResponseMessage(HttpStatusCode.OK);
            if (message != null)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
                SHA256 sha = new SHA256CryptoServiceProvider();
                byte[] requestSHA256 = sha.ComputeHash(bytes);
                string requestSHA256String = requestSHA256.ToString();
                return Ok(requestSHA256String);
            }
            return Ok("Bad Request");
        }
    }
}
