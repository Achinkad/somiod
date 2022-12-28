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
        public IHttpActionResult GetApplications()
        {
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
        public IHttpActionResult GetApplicationById(int id)
        {
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
                return Ok("A new application was stored with success!");
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // PUT: api/somiod/applications/{id} -> Update an Application
        [HttpPut, Route("api/somiod/applications/{id}")]
        public IHttpActionResult PutApplication(int id, [FromBody] Application value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");

            try
            {
                ApplicationController app = new ApplicationController();
                bool response = app.Update(value, id);
                if (!response) return BadRequest("Operation Failed");
                return Ok("Application was updated successfully!");
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // DELETE: api/somiod/applications/{id} -> Deletes an Application
        [HttpDelete, Route("api/somiod/applications/{id}")]
        public IHttpActionResult DeleteApplication(int id)
        {
            try
            {
                ApplicationController app = new ApplicationController();
                bool response = app.DeleteApplication(id);
                if (!response) return BadRequest("Operation Failed");
                return Ok("Application was deleted successfully!");
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /* --- MODULE API ROUTES --- */

        // GET: api/somiod/modules -> Get all modules
        [HttpGet, Route("api/somiod/modules")]
        public IHttpActionResult GetModules()
        {
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
        public IHttpActionResult GetModuleById(int id)
        {
            try
            {
                ModuleController module = new ModuleController();
                Module response = module.GetModule(id);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // POST: api/somiod/{application_name} -> Stores a new Module
        [HttpPost, Route("api/somiod/{application_name}")]
        public IHttpActionResult PostModule(string application_name, [FromBody] Module value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");
            if (value.Res_type != "module") return BadRequest("Request type is different from 'module'.");

            ApplicationController application = new ApplicationController();
            if (application.GetApplicationByName(application_name) == -1) return BadRequest("The application '" + application_name + "' does not exist.");

            try
            {
                ModuleController module = new ModuleController();
                bool response = module.Store(value, application.GetApplicationByName(application_name));
                if (!response) return BadRequest("Operation Failed");
                return Ok("A new Module was stored with success!");
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // PUT: api/somiod/modules/{id} -> Update a Module
        [HttpPut, Route("api/somiod/modules/{id}")]
        public IHttpActionResult PutModule(int id, [FromBody] Module value)
        {
            if (value == null) return BadRequest("Please provide the required information for this request.");

            try
            {
                ModuleController module = new ModuleController();
                bool response = module.UpdateModule(value, id);
                if (!response) return BadRequest("Operation Failed");
                return Ok("Module was updated successfully!");
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // DELETE: api/somiod/modules/{id} -> Deletes a Module
        [HttpDelete, Route("api/somiod/modules/{id}")]
        public IHttpActionResult DeleteModule(int id)
        {
            try
            {
                ModuleController module = new ModuleController();
                bool response = module.DeleteModule(id);
                if (!response) return BadRequest("Operation Failed");
                return Ok("Module was deleted successfully!");
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /* --- SUBSCRIPTION / DATA API ROUTES --- */

        // POST: api/somiod/{application_name}/{module_name} -> Stores a new Subscription Or Data
        [HttpPost, Route("api/somiod/{application_name}/{module_name}")]
        public IHttpActionResult PostSubscriptionOrData(string application_name, string module_name, [FromBody] JObject request)
        {
            if (request == null) return BadRequest("Please provide the required information for this request.");

            ApplicationController application = new ApplicationController();
            if (application.GetApplicationByName(application_name) == -1) return BadRequest("The application '" + application_name + "' does not exist.");

            ModuleController module = new ModuleController();
            if (module.GetModuleByName(module_name) == -1) return BadRequest("The module '" + module_name + "' does not exist.");

            var request_type = request["res_type"].ToString();

            switch (request_type)
            {
                case "data":
                    if (request_type != "data") return BadRequest("Request type is different from 'data'.");

                    try
                    {
                        DataController data = new DataController();
                        bool response = data.Store(request.ToObject<Data>(), module.GetModuleByName(module_name));
                        if (!response) return BadRequest("Operation Failed");
                        return Ok("A new Data was stored with success!");
                    }
                    catch (Exception exception)
                    {
                        return InternalServerError(exception);
                    }

                case "subscription":
                    if (request_type != "subscription") return BadRequest("Request type is different from 'subscription'.");

                    try
                    {
                        SubscriptionController subscription = new SubscriptionController();
                        bool response = subscription.Store(request.ToObject<Subscription>(), module.GetModuleByName(module_name));
                        if (!response) return BadRequest("Operation Failed");
                        return Ok("A new Subscription was stored with success!");
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
        public IHttpActionResult DeleteData(int id)
        {
            try
            {
                DataController data = new DataController();
                bool response = data.DeleteData(id);
                if (!response) return BadRequest("Operation Failed");
                return Ok("Data was deleted successfully!");
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        // DELETE: api/somiod/subscriptions/{id} -> Deletes a Subscription Resource
        [HttpDelete, Route("api/somiod/subscriptions/{id}")]
        public IHttpActionResult DeleteSubscription(int id)
        {
            try
            {
                SubscriptionController subscription = new SubscriptionController();
                bool response = subscription.DeleteSubcription(id);
                if (!response) return BadRequest("Operation Failed");
                return Ok("Subscription was deleted successfully!");
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
