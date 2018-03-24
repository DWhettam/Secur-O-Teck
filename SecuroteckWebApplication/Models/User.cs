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
            public virtual ICollection<Log> UserLog { get; set; }
        #endregion
    }

    public class UserDatabaseAccess
    {
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
                return context.Users.Any(a => a.ApiKey == key);
            }
        }
        //check if username exists in database
        public bool UserExists(string userName)
        {
            using (var context = new UserContext())
            {                
                return context.Users.Any(a => a.UserName == userName);
            }
        }
        //check if api key and username exists in database
        public bool ApiKeyUserExists(string key, string userName)
        {
            using (var context = new UserContext())
            {                
                return context.Users.Any(o => o.ApiKey == key && o.UserName == userName);
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
        //delete user
        public void DeleteUser(string key)
        {   
            using (var context = new UserContext())
            {
                User user = context.Users.First(a => a.ApiKey == key);
                List<Log> logs = context.Logs.Where(c => c.User.ApiKey == key).ToList();
                foreach (var log in logs)
                {
                    log.User = null;
                }                
                context.Users.Attach(user);
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
        //Add log
        public void AddLog(string key, string message)
        {
            using (var context = new UserContext())
            {
                User user = ApiKeyExistsReturnUser(key);
                Log log = new Log(user, user.UserName, message);
                context.Users.Attach(log.User);
                context.Logs.Add(log);                
                context.SaveChanges();
            }
        }
    }
}