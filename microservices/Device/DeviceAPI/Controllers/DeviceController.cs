using System.Web.Http;
using brt.Microservices.Device.Service;
using System.Configuration;
using brt.Models.Device;

namespace DeviceAPI.Controllers
{
    public class DeviceController : ApiController
    {
        private readonly DeviceService _deviceService;

        public DeviceController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time
            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];
            var iothubconnstr = ConfigurationManager.AppSettings["iothubconnstr"];
            var iothubhostname = ConfigurationManager.AppSettings["iothubhostname"];

            _deviceService = new DeviceService(docdburi, docdbkey, database, collection, iothubconnstr, iothubhostname);
        }

        [Route("device/manifests")]
        [RequireHttps]
        [HttpGet]
        public Devices GetAll()
        {
            return _deviceService.GetAll();
        }

        [Route("device/manifests/id/{id}")]
        [RequireHttps]
        [HttpGet]
        public Manifest GetById(string id)
        {
            return _deviceService.GetById(id);
        }

        [Route("device/manifests/customer/{customerid}")]
        [RequireHttps]
        [HttpGet]
        public Devices GetByCompany(string customerid)
        {
            return _deviceService.GetByCustomer(customerid);
        }

        [Route("device/manifests/teammate/{teammateid}")]
        [RequireHttps]
        [HttpGet]
        public Manifest GetByTeammate(string teammateid)
        {
            return _deviceService.GetByTeammate(teammateid);
        }

        [Route("device/manifests/extension/index/{index}/key/{key}/value/{value}")]
        [RequireHttps]
        [HttpGet]
        public Manifest GetByExtension(int index, string key, string value)
        {
            return _deviceService.GetByExtension(index, key, value);
        }

        [Route("device/manifests")]
        [RequireHttps]
        [HttpPost]
        public Manifest Create([FromBody] Manifest manifest)
        {
            return _deviceService.Create(manifest);
        }

        [Route("device/manifests")]
        [RequireHttps]
        [HttpPut]
        public Manifest Update([FromBody] Manifest manifest)
        {
            return _deviceService.Update(manifest);
        }

        [Route("device/manifests/id/{id}")]
        [RequireHttps]
        [HttpDelete]
        public void Delete(string id)
        {
            _deviceService.Delete(id);
        }

        [Route("device/twin/deviceid/{deviceid}")]
        [RequireHttps]
        [HttpGet]
        public string GetDeviceTwin(string deviceid)
        {
            return _deviceService.GetDeviceTwin(deviceid);
        }

        [Route("device/twin/properties/direct")]
        [RequireHttps]
        [HttpPut]
        public void UpdateTwinPropertiesDirect([FromBody] TwinPropertyRequest twinPropertyRequest)
        {
            _deviceService.UpdateTwinPropertiesDirect(twinPropertyRequest);
        }

        [Route("device/twin/tags")]
        [RequireHttps]
        [HttpPut]
        public void UpdateTwinTags([FromBody] TwinPropertyRequest twinPropertyRequest)
        {
            _deviceService.UpdateTwinTags(twinPropertyRequest);
        }

        [Route("device/twin/properties")]
        [RequireHttps]
        [HttpPut]
        public void UpdateTwinProperties([FromBody] TwinPropertyRequest twinPropertyRequest)
        {
            _deviceService.UpdateTwinProperties(twinPropertyRequest);
        }
    }
}
