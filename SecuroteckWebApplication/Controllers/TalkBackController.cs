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
            return Ok("Hello World");
        }

        [ActionName("Sort")]
        public IHttpActionResult Get([FromUri]int[] integers)
        {
            return Ok(integers.OrderBy(i => i).ToArray());
        }        
    }
}
