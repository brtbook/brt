using System.Configuration;
using System.Web.Http;
using brt.Microservices.Reference.Service;
using brt.Models.Reference;

namespace ReferenceAPI.Controllers
{
    public class ReferencePrivateController : ApiController
    {
         ReferenceServicePrivate _referenceService;

        public ReferencePrivateController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time

            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];

            _referenceService = new ReferenceServicePrivate(docdburi, docdbkey, database, collection);
        }

        [Route("reference/admin/entities")]
        [RequireHttps]
        [HttpGet]
        public Entities GetAll()
        {
            return _referenceService.GetAll();
        }

        [Route("reference/admin/entities")]
        [RequireHttps]
        [HttpPost]
        public Entity Create([FromBody] Entity entity)
        {
            return _referenceService.Create(entity);
        }

        [Route("reference/admin/entities")]
        [RequireHttps]
        [HttpPut]
        public Entity Update([FromBody] Entity entity)
        {
            return _referenceService.Update(entity);
        }

        [Route("reference/admin/entities/id/{id}")]
        [RequireHttps]
        [HttpDelete]
        public void Delete(string id)
        {
            _referenceService.Delete(id);
        }
    }
}
