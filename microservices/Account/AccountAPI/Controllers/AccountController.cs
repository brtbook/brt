using System.Configuration;
using System.Web.Http;
using brt.Models.Account;
using brt.Microservices.AccountAPI.Service;

namespace AccountAPI
{
    public class AccountController : ApiController
    {
        AccountService _accountService;

        public AccountController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time

            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];

            _accountService = new AccountService(docdburi, docdbkey, database, collection);
        }

        [Route("account/subscriptions")]
        [RequireHttps]
        [HttpGet]
        public Accounts GetAll()
        {
            return _accountService.GetAll();
        }

        [Route("account/subscriptions/id/{id}")]
        [RequireHttps]
        [HttpGet]
        public Subscription GetById(string id)
        {
            return _accountService.GetById(id);
        }

        [Route("account/subscriptions/organizationid/{organizationid}")]
        [RequireHttps]
        [HttpGet]
        public Subscription GetByOrganizationId(string organizationid)
        {
            return _accountService.GetByOrganizationId(organizationid);
        }

        [Route("account/subscriptions")]
        [RequireHttps]
        [HttpPost]
        public Subscription Create([FromBody] Subscription account)
        {
            return _accountService.Create(account);
        }

        [Route("account/subscriptions")]
        [RequireHttps]
        [HttpPut]
        public Subscription Update([FromBody] Subscription account)
        {
            return _accountService.Update(account);
        }

        [Route("account/subscriptions/id/{id}")]
        [RequireHttps]
        [HttpDelete]
        public void Delete(string id)
        {
            _accountService.Delete(id);
        }
    }
}
