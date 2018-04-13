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
        public IHttpActionResult Get([FromUri]string[] integers)
        {
            string split = string.Join(",", integers);
            if (split.Any(x => char.IsLetter(x)))
            {
                return BadRequest();
            }
            return Ok(integers.OrderBy(i => i).ToArray());
        }        
    }
}
