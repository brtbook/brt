using System.Web.Http;
using brt.Microservices.Application.Service;
using System.Configuration;

namespace ApplicationAPI.Controllers
{
    public class ApplicationPublicController : ApiController
    {
        ApplicationServicePublic _appService;

        public ApplicationPublicController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time

            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];

            _appService = new ApplicationServicePublic(docdburi, docdbkey, database, collection);
        }

        [Route("application/configurations/id/{id}")]
        [RequireHttps]
        [HttpGet]
        public brt.Models.Application.Configuration GetById(string id)
        {
            return _appService.GetById(id);
        }

        [Route("application/configurations/appname/{appname}")]
        [RequireHttps]
        [HttpGet]
        public brt.Models.Application.Configuration GetByAppName(string appname)
        {
            return _appService.GetByAppName(appname);
        }
    }
}
