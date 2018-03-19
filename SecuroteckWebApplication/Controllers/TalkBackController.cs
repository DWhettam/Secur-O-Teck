using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SecuroteckWebApplication.Controllers
{
    public class TalkBackController : ApiController
    {
        [ActionName("Hello")]
        public string Get()
        {
            #region TASK1
            return "Hello World";
            #endregion
        }

        [ActionName("Sort")]
        public int[] Get([FromUri]int[] integers)
        {
            #region TASK1
            return integers.OrderBy(i => i).ToArray();
            #endregion
        }

    }
}
