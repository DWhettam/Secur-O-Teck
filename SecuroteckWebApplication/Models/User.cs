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
        public User()
        {

        }
        #endregion
    }

    #region Task11?
    // TODO: You may find it useful to add code here for Log
    #endregion

    public class UserDatabaseAccess
    {
        #region Task3 
        // TODO: Make methods which allow us to read from/write to the database 
        #endregion
    }


}