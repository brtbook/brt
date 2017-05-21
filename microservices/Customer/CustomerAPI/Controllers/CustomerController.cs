using brt.Microservices.Customer.Service;
using brt.Models.Customer;
using System.Configuration;
using System.Web.Http;

namespace CustomerAPI.Controllers
{
    public class CustomerController : ApiController
    {
        CustomerService _customerService;

        public CustomerController()
        {
            // the configuration information comes from Web.Config when 
            // debugging and from the Azure Portal at runt time

            var docdburi = ConfigurationManager.AppSettings["docdburi"];
            var docdbkey = ConfigurationManager.AppSettings["docdbkey"];
            var database = ConfigurationManager.AppSettings["database"];
            var collection = ConfigurationManager.AppSettings["collection"];

            _customerService = new CustomerService(docdburi, docdbkey, database, collection);
        }

        [Route("customer/organizations")]
        [RequireHttps]
        [HttpGet]
        public Customers GetAll()
        {
            return _customerService.GetAll();
        }

        [Route("customer/organizations/id/{id}")]
        [RequireHttps]
        [HttpGet]
        public Organization GetById(string id)
        {
            return _customerService.GetById(id);
        }

        [Route("customer/organizations/name/{name}")]
        [RequireHttps]
        [HttpGet]
        public Organization GetByName(string name)
        {
            return _customerService.GetByName(name);
        }

        [Route("customer/organizations")]
        [RequireHttps]
        [HttpPost]
        public Organization Create([FromBody] Organization account)
        {
            return _customerService.Create(account);
        }

        [Route("customer/organizations")]
        [RequireHttps]
        [HttpPut]
        public Organization Update([FromBody] Organization account)
        {
            return _customerService.Update(account);
        }

        [Route("customer/organizations/id/{id}")]
        [RequireHttps]
        [HttpDelete]
        public void Delete(string id)
        {
            _customerService.Delete(id);
        }
    }
}