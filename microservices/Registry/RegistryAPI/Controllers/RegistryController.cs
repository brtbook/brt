using brt.Microservices.Registry.Service;
using brt.Models.Registry;
using System.Configuration;
using System.Web.Http;

namespace RegistryAPI.Controllers
{
    public class RegistryController : ApiController
    {
        RegistryService _profileService;

        public RegistryController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time

            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];

            _profileService = new RegistryService(docdburi, docdbkey, database, collection);
        }

        [Route("registry/profiles")]
        [RequireHttps]
        [HttpGet]
        public Profiles GetAll()
        {
            return _profileService.GetAll();
        }

        [Route("registry/profiles/state/{state}")]
        [RequireHttps]
        [HttpGet]
        public Profiles GetAllByState(string state)
        {
            return _profileService.GetAllByState(state);
        }

        [Route("registry/profiles/type/{type}")]
        [RequireHttps]
        [HttpGet]
        public Profiles GetAllByType(int type)
        {
            return _profileService.GetAllByType(type);
        }

        [Route("registry/profiles/company/{company}")]
        [RequireHttps]
        [HttpGet]
        public Profiles GetAllByCompany(string company)
        {
            return _profileService.GetAllByCompany(company);
        }

        [Route("registry/profiles/id/{id}")]
        [RequireHttps]
        [HttpGet]
        public Profile GetById(string id)
        {
            return _profileService.GetById(id);
        }

        [Route("registry/profiles/authid/{authid}")]
        [RequireHttps]
        [HttpGet]
        public Profile GetByAuthId(string authid)
        {
            return _profileService.GetByAuthId(authid);
        }


        [Route("registry/profiles/firstname/{firstname}/lastname/{lastname}")]
        [RequireHttps]
        [HttpGet]
        public Profile GetByName(string firstname, string lastname)
        {
            return _profileService.GetByName(firstname, lastname);
        }

        [Route("registry/profiles")]
        [RequireHttps]
        [HttpPost]
        public Profile Create([FromBody] Profile profile)
        {
            return _profileService.Create(profile);
        }

        [Route("registry/profiles")]
        [RequireHttps]
        [HttpPut]
        public Profile Update([FromBody] Profile profile)
        {
            return _profileService.Update(profile);
        }

        [Route("registry/profiles/id/{id}")]
        [RequireHttps]
        [HttpDelete]
        public void Delete(string id)
        {
            _profileService.Delete(id);
        }
    }
}
