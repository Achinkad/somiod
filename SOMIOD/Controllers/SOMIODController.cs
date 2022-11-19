using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using SOMIOD.Models;
using System.Web.Http.Results;

namespace SOMIOD.Controllers
{
    public class SOMIODController : ApiController
    {
        // TODO: SQL CONNECTION

        string connectionString = null;

        // POST: api/somiod/
        [HttpPost, Route("api/somiod/")]
        public IHttpActionResult PostApplication([FromBody] Application value)
        {
            /* --- Store a new Application --- */

            if (value != null)
            {
                // -> Creates Application + Stores
                Application application = new Application();

                // bool response = application.store(value.name, value.creation_dt);
                 
                // if (!response) return InternalServerError();

                return Ok();
            }

            return BadRequest();
        }

        // POST: api/somiod/{module}
        [HttpPost, Route("api/somiod/{module}")]
        public IHttpActionResult PostModule([FromBody] string value)
        {
            return Ok();
        }

        // POST: api/somiod/{module}/{data}
        [HttpPost, Route("api/somiod/{module}/{data}")]
        public IHttpActionResult PostData([FromBody] string value)
        {
            return Ok();
        }

        // POST: api/somiod/{module}/{subscription}
        [HttpPost, Route("api/somiod/{module}/{subscription}")]
        public IHttpActionResult PostSubscription([FromBody] string value)
        {
            return Ok();
        }
    }
}
