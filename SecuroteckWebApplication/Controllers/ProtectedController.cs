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
        [HttpGet]
        public IHttpActionResult GetHello(HttpRequestMessage request)
        {
            string key = request.Headers.GetValues("ApiKey").First().ToString();            
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            if (dbAccess.ApiKeyExists(key))
            {                
                dbAccess.AddLog(key, "Protected/Hello");
                Models.User user = dbAccess.ApiKeyExistsReturnUser(key);
                return Ok("Hello " + user.UserName.ToString());
            }
            return BadRequest("Invalid API Key");
        }

        [CustomAuthorise]
        [ActionName("sha1")]
        [HttpGet]
        public IHttpActionResult SHA1(HttpRequestMessage request, [FromUri] string message)
        {
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            string key = request.Headers.GetValues("ApiKey").First().ToString();
            if (message == "")
            {
                return BadRequest("No message provided");
            }            
            else if (dbAccess.ApiKeyExists(key))
            {
                dbAccess.AddLog(key, "Protected/SHA1");

                byte[] asciiByteMessage = Encoding.ASCII.GetBytes(message);
                byte[] sha1ByteMessage;
                SHA1 sha1Provider = new SHA1CryptoServiceProvider();
                sha1ByteMessage = sha1Provider.ComputeHash(asciiByteMessage);
                return Ok(BitConverter.ToString(sha1ByteMessage));
            }
            return BadRequest("Invalid API Key");            
        }

        [CustomAuthorise]
        [ActionName("sha256")]
        [HttpGet]
        public IHttpActionResult SHA256(HttpRequestMessage request, [FromUri] string message)
        {
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            string key = request.Headers.GetValues("ApiKey").First().ToString();
            if (message == "")
            {
                return BadRequest("Bad Request");
            }
            else if (dbAccess.ApiKeyExists(key))
            {                
                dbAccess.AddLog(key, "Protected/SHA256");

                byte[] asciiByteMessage = Encoding.ASCII.GetBytes(message);
                byte[] sha256ByteMessage;
                SHA256 sha256Provider = new SHA256CryptoServiceProvider();
                sha256ByteMessage = sha256Provider.ComputeHash(asciiByteMessage);
                return Ok(BitConverter.ToString(sha256ByteMessage));
            }
            return BadRequest("Invalid API Key");
        }

        [CustomAuthorise]
        [ActionName("getpublickey")]
        [HttpGet]
        public IHttpActionResult GetPublicKey(HttpRequestMessage request)
        {
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            string key = request.Headers.GetValues("ApiKey").First().ToString();
            if (dbAccess.ApiKeyExists(key))
            {
                dbAccess.AddLog(key, "Protected/GetPublicKey");
                return Ok(WebApiConfig.rsaProvider.ToXmlString(false));
            }
            return BadRequest("Invalid API Key");
        }

        [CustomAuthorise]
        [ActionName("sign")]
        [HttpGet]
        public IHttpActionResult GetSign(HttpRequestMessage request, [FromUri] string message)
        {
            string key = request.Headers.GetValues("ApiKey").First().ToString();
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            if (dbAccess.ApiKeyExists(key))
            {
                dbAccess.AddLog(key, "Protected/GetPublicKey");
                byte[] asciiByteMessage = Encoding.ASCII.GetBytes(message);
                byte[] encryptedBytes = WebApiConfig.rsaProvider.Encrypt(asciiByteMessage, true);
                SHA1 sha1Provider = new SHA1CryptoServiceProvider();
                encryptedBytes = sha1Provider.ComputeHash(encryptedBytes);
                return Ok(BitConverter.ToString(encryptedBytes));
            }
            return BadRequest("Invalid API Key");
        }
    }
}
