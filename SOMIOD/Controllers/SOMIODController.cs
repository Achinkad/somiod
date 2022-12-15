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
            if (value != null && value.Res_type == "application")
            {
                ApplicationController app = new ApplicationController();
                bool response = app.Store(value);                 
                if (!response) return BadRequest("Operation Failed");//Local do erro
                return Ok(response);
            }

            return BadRequest();
        }

        // POST: api/somiod/{module} -> Stores a new Module
        [HttpPost, Route("api/somiod/{module}")]
        public IHttpActionResult PostModule([FromBody] Module value)
        {
            if (value != null && value.Res_type == "module")
            {
                ModuleController module = new ModuleController();
                bool response = module.Store(value);
                if (!response) return InternalServerError();
                return Ok();
            }
            return BadRequest();
        }

        // POST: api/somiod/{module}/{data} -> Stores a new Data
        [HttpPost, Route("api/somiod/{module}/{data}")]
        public IHttpActionResult PostData([FromBody] Data value)
        {
            if (value != null && value.Res_type == "data")
            {
                DataController data = new DataController();
                bool response = data.Store(value);
                if (!response) return InternalServerError();
                return Ok();
            }
            return BadRequest();
        }

        // POST: api/somiod/{module}/{subscription} -> Stores a new Subscription
        [HttpPost, Route("api/somiod/{module}/{subscription}")]
        public IHttpActionResult PostSubscription([FromBody] Subscription value)
        {
            if (value != null && value.Res_type == "subscription")
            {
                SubscriptionController subscription = new SubscriptionController();
                bool response = subscription.Store(value);
                if (!response) return InternalServerError();
                return Ok();
            }
            return BadRequest();
        }

       
    }
}
