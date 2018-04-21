using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecuroteckWebApplication.Models;

namespace SecuroteckWebApplication.Controllers
{
    public class TalkBackController : ApiController
    {

        [ActionName("Hello")]
        public IHttpActionResult Get()
        {
            #region TASK1
            //api/talkback/hello response
            return Ok("Hello World");
            #endregion
        }

        [ActionName("Sort")]
        public IHttpActionResult Get([FromUri]string[] integers)
        {
            #region TASK1

            //sort the integers into ascending order

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            int[] intArraySorting = new int[integers.Length];
        
            for (int i = 0; i < integers.Length; i++)
            {
                try
                {
                    int num = int.Parse(integers[i]);
                    intArraySorting[i] = num;
                }
                catch
                {
                    //message.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
            }

            if (integers.Length != 0)
            {
                Array.Sort(intArraySorting);
                return Ok(intArraySorting);
            }

            else
            {
                int[] intArray = new int[0];
                return Ok(intArray);
            }
            #endregion
        }

    }
}
