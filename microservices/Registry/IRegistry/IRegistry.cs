using brt.Models.Registry;

namespace brt.Microservices.Registry.Interface
{
    public interface IRegistry
    {
        Profiles GetAll();
        Profiles GetAllByState(string state);
        Profiles GetAllByCompany(string company);
        Profiles GetAllByType(int type);
        Profile GetById(string id);
        Profile GetByAuthId(string authid);
        Profile GetByName(string firstname, string lastname);
        Profile Create(Profile profile);
        Profile Update(Profile profile);
        void Delete(string id);
    }
}
