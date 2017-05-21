using brt.Models.Reference;

namespace brt.Microservices.Reference.Interface
{
    public interface IReferencePrivate
    {
        Entities GetAll();
        Entity Create(Entity entity);
        Entity Update(Entity entity);
        void Delete(string id);
    }
}
