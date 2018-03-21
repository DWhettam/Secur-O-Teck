using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;

namespace SecuroteckWebApplication.Controllers
{
    public class ProtectedController : ApiController
    {
        [CustomAuthorise]
        [ActionName("hello")]        
        public IHttpActionResult Get(HttpRequestMessage request)
        {
            string key = request.Headers.GetValues("ApiKey").First().ToString();
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            Models.User user = dbAccess.ApiKeyExistsReturnUser(key);           
            return Ok("Hello " + user.UserName.ToString());
        }

        [CustomAuthorise]
        [ActionName("sha1")]
        [HttpGet]
        public IHttpActionResult SHA1([FromUri] string message)
        {
            if (message == "")
            {
                return BadRequest("Bad Request");
            }

            byte[] data = Encoding.ASCII.GetBytes(message);
            SHA1 sha = new SHA1CryptoServiceProvider();
            return Ok(Convert.ToBase64String(sha.ComputeHash(data)));
        }

        [CustomAuthorise]
        [ActionName("sha256")]
        [HttpGet]
        public IHttpActionResult SHA256([FromUri] string message)
        {
            if (message == "")
            {
                return BadRequest("Bad Request");
            }

            byte[] data = Encoding.ASCII.GetBytes(message);
            SHA256 sha = new SHA256CryptoServiceProvider();
            return Ok(Convert.ToBase64String(sha.ComputeHash(data)));
        }
    }
}
