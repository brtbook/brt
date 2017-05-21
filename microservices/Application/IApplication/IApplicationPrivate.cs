using brt.Models.Application;

namespace brt.Microservices.Application.Interface
{
    public interface IApplicationPrivate
    {
        Applications GetAll();
        Configuration GetById(string id);
        Configuration GetByAppName(string appname);
        Configuration Create(Configuration configuration);
        Configuration Update(Configuration configuration);
        void Delete(string id);
    }
}
