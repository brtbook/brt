using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using brt.Microservices.Common.Wire;
using brt.Models.Registry;
using brt.Models.Telemetry;

namespace mt.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // You can use the PolicyAuthorize decorator to execute a certain policy 
        // if the user is not already signed into the app.
        [Authorize]
        public ActionResult Claims()
        {
            var displayName = ClaimsPrincipal.Current.FindFirst(
                ClaimsPrincipal.Current.Identities.First().NameClaimType);
	
            if (displayName != null)
            {
                ViewBag.DisplayName = displayName.Value;

                var registryApi = ConfigurationManager.AppSettings["RegistryAPI"];
                var telemetryApi = ConfigurationManager.AppSettings["TelemetryAPI"];
                var devKey = ConfigurationManager.AppSettings["DevKey"];

                var names = displayName.Value.Split(' ');
                var firstname = names[0];
                var lastname = names[1];

                var query = $"/firstname/{firstname}/lastname/{lastname}?{devKey}";
                var api = registryApi + query;

                // get autenticated user's profile
                var json = Rest.Get(new Uri(api));
                var profile = ModelManager.JsonToModel<Profile>(json);

                // NOTE: only type 2 (admin) and type 3 (employee) are able to access telemetry. 
                // All other user types will return an empy telemetry collection

                var type = 0;

                if (profile != null)
                {
                    switch (profile.type)
                    {
                        case ProfileTypeEnum.NotSet:
                            type = 0;
                            break;
                        case ProfileTypeEnum.Organization:
                            type = 1;
                            break;
                        case ProfileTypeEnum.Administrator:
                            type = 2;
                            break;
                        case ProfileTypeEnum.Employee:
                            type = 3;
                            break;
                        case ProfileTypeEnum.Contractor:
                            type = 4;
                            break;
                        case ProfileTypeEnum.Temporary:
                            type = 5;
                            break;
                        case ProfileTypeEnum.Partner:
                            type = 6;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    query = $"/companyname/{profile.companyname}?usertype={type}&username={profile.username}&count=15&{devKey}";
                    api = telemetryApi + query;

                    json = Rest.Get(new Uri(api));
                    var telemetryList = ModelManager.JsonToModel<TelemetryList>(json);

                    ViewBag.UserProfile = profile;
                    ViewBag.Telemetry = telemetryList.list;
                }
            }

            return View();
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;

            return View("Error");
        }
    }
}