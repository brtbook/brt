using System.Web.Http;
using brt.Microservices.Application.Service;
using brt.Models.Application;
using System.Configuration;

namespace ApplicationAPI.Controllers
{
    public class ApplicationPrivateController : ApiController
    {
        ApplicationServicePrivate _appService;

        public ApplicationPrivateController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time

            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];

            _appService = new ApplicationServicePrivate(docdburi, docdbkey, database, collection);
        }

        [Route("application/admin/configurations")]
        [RequireHttps]
        [HttpGet]
        public Applications GetAll()
        {
            return _appService.GetAll();
        }

        [Route("application/admin/configurations/id/{id}")]
        [RequireHttps]
        [HttpGet]
        public brt.Models.Application.Configuration GetById(string id)
        {
            return _appService.GetById(id);
        }

        [Route("application/admin/configurations/appname/{appname}")]
        [RequireHttps]
        [HttpGet]
        public brt.Models.Application.Configuration GetByAppName(string appname)
        {
            return _appService.GetByAppName(appname);
        }

        [Route("application/admin/configurations")]
        [RequireHttps]
        [HttpPost]
        public brt.Models.Application.Configuration Create([FromBody] brt.Models.Application.Configuration configuration)
        {
            return _appService.Create(configuration);
        }

        [Route("application/admin/configurations")]
        [RequireHttps]
        [HttpPut]
        public brt.Models.Application.Configuration Update([FromBody] brt.Models.Application.Configuration configuration)
        {
            return _appService.Update(configuration);
        }

        [Route("application/admin/configurations")]
        [RequireHttps]
        [HttpDelete]
        public void Delete(string id)
        {
            _appService.Delete(id);
        }
    }
}
