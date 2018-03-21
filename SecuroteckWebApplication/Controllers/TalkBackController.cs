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
        public IHttpActionResult Get()
        {
            #region TASK1
            return Ok("Hello World");
            #endregion
        }

        [ActionName("Sort")]
        public IHttpActionResult Get([FromUri]int[] integers)
        {
            #region TASK1
            return Ok(integers.OrderBy(i => i).ToArray());
            #endregion
        }

    }
}
