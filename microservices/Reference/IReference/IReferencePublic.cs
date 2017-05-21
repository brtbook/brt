using brt.Models.Reference;

namespace BlueMetl.Microservices.Reference.Interface
{
    public interface IReferencePublic
    {
        Entities GetAllByDomain(string domain);
        Entities GetAllByLink(string link);
        Entity GetByCode(string code);
        Entities GetByCodeValue(string codevalue);
    }
}
