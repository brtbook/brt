using brt.Models.Customer;

namespace brt.Microservices.Customer.Interface
{
    public interface ICustomer
    {
        Customers GetAll();
        Organization GetById(string id);
        Organization Create(Organization orginization);
        Organization Update(Organization orginization);
        void Delete(string id);
    }
}
