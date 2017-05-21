using brt.Models.Device;

namespace brt.Microservices.Device.Interface
{
    public interface IDevice
    {
        Devices GetAll();
        Manifest GetById(string id);
        Devices GetByCustomer(string customerid);
        Manifest GetByTeammate(string teammateid);
        Manifest Create(Manifest manifest);
        Manifest Update(Manifest manifest);
        string GetDeviceTwin(string deviceId);
        void UpdateTwinPropertiesDirect(TwinPropertyRequest twinPropertyRequest);
        void UpdateTwinProperties(TwinPropertyRequest twinPropertyRequest);
        void UpdateTwinTags(TwinPropertyRequest twinPropertyRequest);
        void Delete(string id);
    }
}
