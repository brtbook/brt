using brt.Microservices.Reference.Service;
using brt.Models.Reference;
using System.Configuration;
using System.Web.Http;

namespace ReferenceAPI.Controllers
{
    public class ReferencePublicController : ApiController
    {
        ReferenceServicePublic _referenceService;

        public ReferencePublicController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time

            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];

            _referenceService = new ReferenceServicePublic(docdburi, docdbkey, database, collection);
        }

        [Route("reference/entities/domain/{domain}")]
        [RequireHttps]
        [HttpGet]
        public Entities GetAllByDomain(string domain)
        {
            return _referenceService.GetAllByDomain(domain);
        }

        [Route("reference/entities/code/{code}")]
        [RequireHttps]
        [HttpGet]
        public Entity GetByCode(string code)
        {
            return _referenceService.GetByCode(code);
        }

        [Route("reference/entities/codevalue/{codevalue}")]
        [RequireHttps]
        [HttpGet]
        public Entities GetByCodeValue(string codevalue)
        {
            return _referenceService.GetByCodeValue(codevalue);
        }

        [Route("reference/entities/link/{link}")]
        [RequireHttps]
        [HttpGet]
        public Entities GetAllByLink(string link)
        {
            return _referenceService.GetAllByLink(link);
        }
    }
}
