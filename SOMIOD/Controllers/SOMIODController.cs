using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using SOMIOD.Models;
using System.Web.Http.Results;
using System.Runtime.CompilerServices;

namespace SOMIOD.Controllers
{
    public class SOMIODController : ApiController
    {
        // GET: api/somiod/{module}/{subscription}
        [HttpGet, Route("api/somiod/")]
        public IHttpActionResult GetTest()
        {
            ModuleController m = new ModuleController();
            Module mod = m.GetModule(1);
            if (mod != null) {
                return Ok(mod);
            }
            return BadRequest("O módulo com o id não existe");
        }

        // POST: api/somiod/ -> Stores a new Application
        [HttpPost, Route("api/somiod/")]
        public IHttpActionResult PostApplication([FromBody] Application value)
        {           
            if (value != null)
            {
                // -> Creates Application + Stores
                Application application = new Application();

                bool response = application.Store(value.name, value.creation_dt);
                 
                if (!response) return InternalServerError();
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
