using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;

namespace SecuroteckWebApplication.Models
{
    public class User
    {
        #region Task2
            [Key]
            public string ApiKey { get; set; }
            public string UserName { get; set; }
        #endregion
    }

    #region Task11?
    // TODO: You may find it useful to add code here for Log
    #endregion

    public class UserDatabaseAccess
    {
        #region Task3 
        //Insert user
        public User InsertUser(string userName)
        {
            var apiKey = Guid.NewGuid();
            User user = new User
            {
                ApiKey = apiKey.ToString(),
                UserName = userName,
            };
            using (var context = new UserContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
            return user;
        }
        //check if api key exists in db
        public bool ApiKeyExists(string key)
        {
            using (var context = new UserContext())
            {
                if (context.Users.Find(key) != null)
                {
                    return true;
                }
                return false;
            }
        }
        //check if username exists in database
        public bool UserExists(string userName)
        {
            using (var context = new UserContext())
            {
                if (context.Users.Any(a => a.UserName == userName))
                {
                    return true;
                }
                return false;
            }
        }
        //check if api key and username exists in database
        public bool ApiKeyUserExists(string key, string userName)
        {
            using (var context = new UserContext())
            {
                if (context.Users.Any(o => o.ApiKey == key && o.UserName == userName))
                {
                    return true;
                }
                return false;
            }
        }
        //check if api key exists in database, returns user
        public User ApiKeyExistsReturnUser(string key)
        {
            using (var context = new UserContext())
            {
                return context.Users.Find(key);
            }
        }

        public void DeleteUser(string key)
        {   
            using (var context = new UserContext())
            {
                User user = context.Users.First(a => a.ApiKey == key);
                context.Users.Attach(user);
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
        #endregion
    }
}