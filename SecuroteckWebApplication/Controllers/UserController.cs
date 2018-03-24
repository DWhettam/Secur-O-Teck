using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SecuroteckWebApplication.Controllers
{
    public class UserController : ApiController
    {
        [ActionName("new")]        
        public IHttpActionResult Get([FromUri]string username)
        {
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            if (dbAccess.UserExists(username))
            {                
                return Ok("True - User Does Exist! Did you mean to do a POST to create a new user?");
            }
            return Ok("False - User Does Not Exist! Did you mean to do a POST to create a new user?");
        }
        [ActionName("new")]       
        public IHttpActionResult Post([FromBody] string username)
        {                        
            if (username == null)
            {
                return BadRequest("Oops. Make sure your body contains a string with your username and your Content-Type is Content - Type:application / json");
            }
            
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            if (dbAccess.UserExists(username))
            {
                return BadRequest("A user with the given username already exists");
            }
            Models.User newUser = dbAccess.InsertUser(username);
            return Ok(newUser.ApiKey);
        }               
        [ActionName("removeuser")]
        [CustomAuthorise]
        public IHttpActionResult Delete(HttpRequestMessage request, [FromUri]string username)
        {
            string key = request.Headers.GetValues("ApiKey").First().ToString();
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            if (dbAccess.ApiKeyUserExists(key, username))
            {
                dbAccess.AddLog(key, "User/RemoveUser");
                dbAccess.DeleteUser(key);
                return Ok(true);
            }
            return Ok(false);
        }
    }
}
