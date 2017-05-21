using brt.Models.Application;

namespace brt.Microservices.Application.Interface
{
    public interface IApplicationPublic
    {
        Configuration GetById(string id);
        Configuration GetByAppName(string appname);
    }
}
