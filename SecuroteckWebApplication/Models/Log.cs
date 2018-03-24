using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml;
namespace SecuroteckWebApplication.Models
{
    public class Log
    {
        public Log(User user, string userName, string logString)
        {
            LogString = logString;
            LogDateTime = DateTime.Now;
            User = user;
            UserName = userName;
        }
        public Log()
        { }

        [Key]  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }
        public virtual User User { get; set; }
        public string UserName { get; set; }
    }
}