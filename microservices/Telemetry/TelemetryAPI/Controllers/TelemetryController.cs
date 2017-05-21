using System.Configuration;
using System.Web.Http;
using brt.Microservices.Telemetry.Service;
using brt.Models.Telemetry;

namespace Telemetry.Controllers
{
    public class TelemetryController : ApiController
    {
        private TelemetryService _telemetryService;

        public TelemetryController()
        {
            var sqlConnStr = ConfigurationManager.AppSettings["SQLConnStr"];
            _telemetryService = new TelemetryService(sqlConnStr);

        }

        [Route("telemetry/events/companyname/{companyname}")]
        [RequireHttps]
        [HttpGet]
        public TelemetryList GetTelemetry(string companyname, int usertype = 3, string username = "notused", int count = 1)
        {
            TelemetryList telemetryList = null;
            telemetryList = _telemetryService.GetTelemetry(companyname, usertype, username, count);
            return telemetryList;
        }
    }
}
