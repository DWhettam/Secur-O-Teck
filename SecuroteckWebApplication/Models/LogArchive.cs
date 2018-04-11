using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml;
namespace SecuroteckWebApplication.Models
{
    public class LogArchive
    {
        public LogArchive(string apiKey, string userName, string logString)
        {
            LogString = logString;
            LogDateTime = DateTime.Now;
            ApiKey = apiKey;
            UserName = userName;
        }
        public LogArchive()
        { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }
        public string ApiKey { get; set; }
        public string UserName { get; set; }
    }
}