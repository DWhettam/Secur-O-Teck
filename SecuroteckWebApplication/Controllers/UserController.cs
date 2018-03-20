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
        public string Get([FromUri]string username)
        {
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            if (dbAccess.UserExists(username))
            {                
                return "True - User Does Exist! Did you mean to do a POST to create a new user?";
            }
            return "False - User Does Not Exist! Did you mean to do a POST to create a new user?";
        }
        [ActionName("new")]       
        public string Post(HttpRequestMessage request)
        {
            string username = request.Content.ReadAsStringAsync().Result;
            if (username == null)
            {
                return "Oops. Make sure your body contains a string with your username and your Content-Type is Content - Type:application / json";
            }
            Models.UserDatabaseAccess dbAccess = new Models.UserDatabaseAccess();
            Models.User newUser = dbAccess.InsertUser(username);
            return newUser.ApiKey.ToString();
        }
    }
}
