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
        [CustomAuthorise]
        [ActionName("Hello")]
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
                    var hash1 = sha1.ComputeHash(Encoding.UTF8.GetBytes(message));
                    var sb = new StringBuilder(hash1.Length * 2);

                    foreach (byte b in hash1)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    SHA1string = sb.ToString();
                }
                return Ok(SHA1string);
            }
            return Ok("Bad Request");
        }


        [CustomAuthorise]
        [ActionName("SHA256")]
        [HttpGet]
        public IHttpActionResult SHA256([FromUri] string message)
        {
            if (message != null)
            {
                string SHA256string = null;
                StringBuilder sb = new StringBuilder();

                using (SHA256 hash256 = SHA256Managed.Create())
                {
                    Encoding enc = Encoding.UTF8;
                    Byte[] result = hash256.ComputeHash(enc.GetBytes(message));

                    foreach (Byte b in result)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    SHA256string = sb.ToString();
                }
                return Ok(SHA256string);
            }
            return Ok("Bad Request");
        }

        [CustomAuthorise]
        [ActionName("GetPublicKey")]
        public IHttpActionResult GetPublicKey()
        {
            try
            {
                return Ok(WebApiConfig.pubkey);
            }
           catch
            {
                return BadRequest();
            }
        }
    }
}
