using brt.Models.Account;

namespace brt.Microservices.AccountAPI.Interface
{
    public interface IAccount
    {
        Accounts GetAll();
        Subscription GetById(string id);
        Subscription GetByOrganizationId(string organizationid);
        Subscription Create(Subscription account);
        Subscription Update(Subscription account);
        void Delete(string id);
    }
}
