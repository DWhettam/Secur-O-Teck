using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using SecuroteckWebApplication.Models;

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
                dbAccess.AddLog(key, $"Protected/SHA1 {message}");

                byte[] asciiByteMessage = Encoding.ASCII.GetBytes(message);
                byte[] sha1ByteMessage;
                SHA1 sha1Provider = new SHA1CryptoServiceProvider();
                sha1ByteMessage = sha1Provider.ComputeHash(asciiByteMessage);
                return Ok(BitConverter.ToString(sha1ByteMessage).Replace("-", ""));
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
                dbAccess.AddLog(key, $"Protected/SHA256 {message}");

                byte[] asciiByteMessage = Encoding.ASCII.GetBytes(message);
                byte[] sha256ByteMessage;
                SHA256 sha256Provider = new SHA256CryptoServiceProvider();
                sha256ByteMessage = sha256Provider.ComputeHash(asciiByteMessage);
                return Ok(BitConverter.ToString(sha256ByteMessage).Replace("-", ""));
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
                dbAccess.AddLog(key, $"Protected/GetSign {message}");

                byte[] asciiByteMessage = Encoding.ASCII.GetBytes(message);               
                byte[] encryptedBytes = WebApiConfig.rsaProvider.SignData(asciiByteMessage, CryptoConfig.MapNameToOID("SHA1"));
                                
                return Ok(BitConverter.ToString(encryptedBytes));
            }
            return BadRequest("Invalid API Key");
        }

        [CustomAuthorise]
        [ActionName("addfifty")]
        [HttpGet]
        public IHttpActionResult GetAddFifty(HttpRequestMessage request, [FromUri] string encryptedInteger, [FromUri] string encryptedsymkey, [FromUri] string encryptedIV)   
        {
            string key = request.Headers.GetValues("ApiKey").First().ToString();
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            if (dbAccess.ApiKeyExists(key))
            {
                byte[] encryptedByteMessage = StringToByteArray(encryptedInteger.Replace("-", string.Empty));
                byte[] encryptedByteKey = StringToByteArray(encryptedsymkey.Replace("-", string.Empty));
                byte[] encryptedByteIV = StringToByteArray(encryptedIV.Replace("-", string.Empty));

                string test = WebApiConfig.rsaProvider.ToXmlString(true);

                byte[] decryptedByteMessage = WebApiConfig.rsaProvider.Decrypt(encryptedByteMessage, true);
                byte[] decryptedByteKey = WebApiConfig.rsaProvider.Decrypt(encryptedByteKey, true);
                byte[] decryptedByteIV = WebApiConfig.rsaProvider.Decrypt(encryptedByteIV, true);

                int plaintextMessage = BitConverter.ToInt32(decryptedByteMessage, 0);

                string response = (plaintextMessage + 50).ToString();
                byte[] encryptedMessageBytes;

                AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();
                aesProvider.Key = decryptedByteKey;
                aesProvider.IV = decryptedByteIV;
                ICryptoTransform encryptor = aesProvider.CreateEncryptor();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(response);
                        }
                        encryptedMessageBytes = ms.ToArray();
                    }
                }
                UserDatabaseAccess.udb.AddLog(key, $"Protected/AddFifty {plaintextMessage}");
                return Ok(BitConverter.ToString(encryptedMessageBytes));                
            }            
            return BadRequest("Bad Request");
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

    }
}
