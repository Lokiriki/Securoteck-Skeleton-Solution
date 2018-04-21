using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecuroteckWebApplication.Models;

namespace SecuroteckWebApplication.Controllers
{
    public class UserController : ApiController
    {
        [ActionName("New")]
        public HttpResponseMessage Get([FromUri]string userName)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);

            UserDatabaseAccess readAccess = new UserDatabaseAccess();
            bool exists = readAccess.checkUserViaNameBool(userName);
            if (exists == true)
            {
                //if user exists return "True - User Does Exist! Did you mean to do a POST to create a new user?" in body
                //with status code OK(200)

                message.Content = new StringContent("True - User Does Exist! Did you mean to do a POST to create a new user?");
                return message;
            }
            else
            {
                //elseif user does not exist return "False - User Does Not Exist! Did you mean to do a POST to create a new user?"
                //or if no string submitted 
                //with status code OK(200)

                message.Content = new StringContent("False - User Does Not Exist! Did you mean to do a POST to create a new user?");
                return message;
            }
        }

        [ActionName("New")]
        public HttpResponseMessage Post([FromBody]string userName)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            if (userName != null)
            {
                UserDatabaseAccess DBAccess = new UserDatabaseAccess();
                string newApiKey = DBAccess.writeNewUserToDB(userName);
                message.Content = new StringContent(newApiKey);
                return message;
            }
            else
            {
                message.Content = new StringContent("Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json");
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }

        }

        [ActionName("Removeuser")]
        [CustomAuthorise]
        public HttpResponseMessage Delete(HttpRequestMessage request, [FromUri]string userName)
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            bool deleteStatus = false;
            string headerKey = request.Headers.GetValues("ApiKey").FirstOrDefault();
            if (headerKey != null)
            {
                UserDatabaseAccess DBAccess = new UserDatabaseAccess();
                bool keyExist = DBAccess.checkUserViaKeyBool(headerKey);
                if (keyExist == true)
                {
                    string[] userCheck = new string[2];

                    userCheck[0] = userName;
                    userCheck[1] = headerKey;
                    bool userExist = DBAccess.checkUserViaKeyAndNameBool(userCheck);

                    if (userExist == true)
                    {
                        deleteStatus = DBAccess.deleteUser(userCheck[1]);
                        return Request.CreateResponse(HttpStatusCode.OK, deleteStatus);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, deleteStatus);

                }

                return Request.CreateResponse(HttpStatusCode.OK, deleteStatus);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, deleteStatus);
            }

        }
    }
}
