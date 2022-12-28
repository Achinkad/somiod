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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace SOMIOD.Controllers
{
    public class SOMIODController : ApiController
    {   
        /* --- APPLICATION API ROUTES --- */
        
        // GET: api/somiod/applications -> Get all applications
        [HttpGet, Route("api/somiod/applications")]
        public IHttpActionResult GetApplications([FromBody] Application value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "application") return BadRequest("Request type is different from 'application'.");

            try
            {
                ApplicationController app = new ApplicationController();
                List<Application> response = app.GetApplications();
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }            
        }

        // GET: api/somiod/applications/{id} -> Gets an application
        [HttpGet, Route("api/somiod/applications/{id}")]
        public IHttpActionResult GetApplicationById(int id, [FromBody] Application value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "application") return BadRequest("Request type is different from 'application'.");

            try
            {
                ApplicationController app = new ApplicationController();
                Application response = app.GetApplication(id);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // POST: api/somiod/ -> Stores a new Application
        [HttpPost, Route("api/somiod/")]
        public IHttpActionResult PostApplication([FromBody] Application value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "application") return BadRequest("Request type is different from 'application'.");

            try
            {
                ApplicationController app = new ApplicationController();
                bool response = app.Store(value);
                if (!response) return BadRequest("Operation Failed");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // PUT: api/somiod/applications/{id} -> Update an Application
        [HttpPut, Route("api/somiod/applications/{id}")]
        public IHttpActionResult PutApplication([FromBody] Application value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "application") return BadRequest("Request type is different from 'application'.");

            try
            {
                ApplicationController app = new ApplicationController();
                bool response = app.Update(value);
                if (!response) return BadRequest("Operation Failed");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // DELETE: api/somiod/applications/{id} -> Deletes an Application
        [HttpDelete, Route("api/somiod/applications/{id}")]
        public IHttpActionResult DeleteApplication([FromBody] Application value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "application") return BadRequest("Request type is different from 'application'.");

            try
            {
                ApplicationController app = new ApplicationController();
                bool response = app.DeleteApplication(value.Id);
                if (!response) return BadRequest("Operation Failed");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /* --- MODULE API ROUTES --- */

        // GET: api/somiod/modules -> Get all modules
        [HttpGet, Route("api/somiod/modules")]
        public IHttpActionResult GetModules([FromBody] Module value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "module") return BadRequest("Request type is different from 'module'.");

            try
            {
                ModuleController module = new ModuleController();
                List<Module> response = module.GetModules();
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // GET: api/somiod/modules/{id} -> Gets a module
        [HttpGet, Route("api/somiod/modules/{id}")]
        public IHttpActionResult GetModuleById([FromBody] Module value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "module") return BadRequest("Request type is different from 'module'.");

            try
            {
                ModuleController module = new ModuleController();
                Module response = module.GetModule(value.Id);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // POST: api/somiod/{application} -> Stores a new Module
        [HttpPost, Route("api/somiod/{application}")]
        public IHttpActionResult PostModule(string application_name, [FromBody] Module value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "module") return BadRequest("Request type is different from 'module'.");

            ApplicationController application = new ApplicationController();
            if (application.GetApplicationByName(application_name)) return BadRequest("The application does not exist.");

            try
            {
                ModuleController module = new ModuleController();
                bool response = module.Store(value);
                if (!response) return BadRequest("Operation Failed");
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // PUT: api/somiod/modules/{id} -> Update a Module
        [HttpPut, Route("api/somiod/modules/{id}")]
        public IHttpActionResult PutModule([FromBody] Module value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "module") return BadRequest("Request type is different from 'module'.");

            try
            {
                ModuleController module = new ModuleController();
                bool response = module.UpdateModule(value);
                if (!response) return BadRequest("Operation Failed");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // DELETE: api/somiod/modules/{id} -> Deletes a Module
        [HttpDelete, Route("api/somiod/modules/{id}")]
        public IHttpActionResult DeleteModule([FromBody] Module value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "module") return BadRequest("Request type is different from 'module'.");

            try
            {
                ModuleController module = new ModuleController();
                bool response = module.DeleteModule(value.Id);
                if (!response) return BadRequest("Operation Failed");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /* --- SUBSCRIPTION / DATA API ROUTES --- */

        // POST: api/somiod/{application}/{module} -> Stores a new Subscription Or Data
        [HttpPost, Route("api/somiod/{application}/{module}")]
        public IHttpActionResult PostSubscriptionOrData([FromBody] JObject request)
        {
            if (request == null) return BadRequest("Please provide the required information for this request.");

            switch (request["res_type"].ToString())
            {
                case "data":
                    if (!request["res_type"].Equals("data")) return BadRequest("Request type is different from 'data'.");

                    try
                    {
                        DataController data = new DataController();
                        bool response = data.Store(request.ToObject<Data>());
                        if (!response) return InternalServerError();
                        return Ok();
                    }
                    catch (Exception exception)
                    {
                        return InternalServerError(exception);
                    }

                case "subscription":
                    if (!request["res_type"].Equals("subscription")) return BadRequest("Request type is different from 'subscription'.");

                    try
                    {
                        SubscriptionController subscription = new SubscriptionController();
                        bool response = subscription.Store(request.ToObject<Subscription>());
                        if (!response) return InternalServerError();
                        return Ok();
                    }
                    catch (Exception exception)
                    {
                        return InternalServerError(exception);
                    }


                default:
                    return BadRequest("Please provide a valid resquest type for this request.");
            }
        }

        // DELETE: api/somiod/datas/{id} -> Deletes a Data Resource
        [HttpDelete, Route("api/somiod/datas/{id}")]
        public IHttpActionResult DeleteData([FromBody] Data value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "data") return BadRequest("Request type is different from 'data'.");

            try
            {
                DataController data = new DataController();
                bool response = data.DeleteData(value.Id);
                if (!response) return BadRequest("Operation Failed");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // DELETE: api/somiod/subscriptions/{id} -> Deletes a Subscription Resource
        [HttpDelete, Route("api/somiod/subscriptions/{id}")]
        public IHttpActionResult DeleteSubscription([FromBody] Subscription value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "subscription") return BadRequest("Request type is different from 'subscription'.");

            try
            {
                SubscriptionController subscription = new SubscriptionController();
                bool response = subscription.DeleteSubcription(value.Id);
                if (!response) return BadRequest("Operation Failed");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
