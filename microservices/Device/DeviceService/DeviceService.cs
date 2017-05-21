using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using brt.Microservices.Device.Interface;
using Microsoft.Azure.Devices;
using brt.Models.Device;
using brt.Microservices.Common.Store;
using Microsoft.Azure.Devices.Shared;

namespace brt.Microservices.Device.Service
{
    internal static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
            TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
                .StartNew<Task<TResult>>(func)
                .Unwrap<TResult>()
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
                .StartNew<Task>(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }

    public class DeviceService : IDevice
    {
        private readonly string _iothubhostname;
        private readonly IDbase _dbase;
        private readonly RegistryManager _registryManager;
        private readonly ServiceClient _serviceClient;

        public DeviceService(string docdburi, string docdbkey, string database, string collection, string iothubconnstr,
            string iothubhostname)
        {
            _iothubhostname = iothubhostname;
            this._registryManager = RegistryManager.CreateFromConnectionString(iothubconnstr);
            this._serviceClient = ServiceClient.CreateFromConnectionString(iothubconnstr);
            this._dbase = new Dbase(docdburi, docdbkey);
            this._dbase.Connect(database, collection);
        }

        public Manifest Create(Manifest manifest)
        {
            try
            {
                if (manifest.IsValid())
                {
                    // Provision the device in IoT Hub
                    var device =
                        AsyncHelper.RunSync<Microsoft.Azure.Devices.Device>(
                            () =>
                                _registryManager.AddDeviceAsync(
                                    new Microsoft.Azure.Devices.Device(manifest.SerialNumber)));

                    var tags = new TwinPropertyRequest
                    {
                        DeviceId = device.Id,
                        Name = "manufacturer",
                        Properties =
                        {
                            ["manufacturer"] = manifest.Manufacturer,
                            ["model"] = manifest.ModelNumber
                        }
                    };
                    UpdateTwinTags(tags);

                    var desiredProps = new TwinPropertyRequest
                    {
                        DeviceId = device.Id,
                        Name = "cadenceConfig",
                        Properties =
                        {
                            ["heartbeat"] = manifest.Extensions["heartbeat"],
                            ["telemetry"] = manifest.Extensions["telemetry"]
                        }
                    };
                    UpdateTwinProperties(desiredProps);

                    // Update the manifest and store in DocDb
                    manifest.Modified = DateTime.UtcNow;
                    manifest.Key.PrimaryKey = device.Authentication.SymmetricKey.PrimaryKey;
                    manifest.Key.SecondaryKey = device.Authentication.SymmetricKey.SecondaryKey;
                    manifest.Hub = _iothubhostname;
                    _dbase.Insert(manifest);
                }
                else
                {
                    throw new Exception(Errors.ERR_DEVICE_MODEL_NOT_VALID);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }

            return manifest;
        }

        public Manifest Update(Manifest manifest)
        {
            try
            {
                if (manifest.IsValid())
                {
                    manifest.Modified = DateTime.UtcNow;
                    this._dbase.Update(manifest);
                }
                else
                {
                    throw new Exception(Errors.ERR_DEVICE_MODEL_NOT_VALID);
                }
            }
            catch (Exception err)
            {
                throw new Exception(string.Format(Errors.ERR_DEVICE_MODEL_NOT_UPDATED, manifest.DeviceDescription), err);
            }

            return manifest;
        }

        public Devices GetAll()
        {
            var devices = new Devices();

            try
            {
                var deviceList = this._dbase.SelectAll<Manifest>();
                foreach (var d in deviceList)
                {
                    devices.List.Add(d);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }

            return devices;
        }

        public Manifest GetById(string id)
        {
            Manifest manifest = null;

            try
            {
                var query = $"select * from Manifest m where m.SerialNumber='{id}'";
                var manifestList = _dbase.SelectByQuery<Manifest>(query);
                if (manifestList[0] != null)
                {
                    manifest = manifestList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }

            return manifest;
        }

        public Manifest GetByExtension(int index, string key, string value)
        {
            Manifest manifest = null;

            try
            {
                // passing index array location is an issue. This worksbut need a more general solution
                // probably need to move this to a Javascript function in the Db 
                var query =
                    $@"SELECT * FROM Manifest m where m.Extensions[{index}].Key = '{key}' and m.Extensions[{index}].Val = '{value}'";

                var manifestList = _dbase.SelectByQuery<Manifest>(query);
                if (manifestList[0] != null)
                {
                    manifest = manifestList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }

            return manifest;
        }

        public void Delete(string id)
        {
            try
            {
                var manifest = GetById(id);
                AsyncHelper.RunSync(() => _registryManager.RemoveDeviceAsync(manifest.SerialNumber));
                _dbase.Delete(id);
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }
        }

        public Devices GetByCustomer(string customerid)
        {
            var devices = new Devices();

            try
            {
                var query =
                    $@"SELECT * FROM Manifest m where m.Extensions[2].Key = 'customerId' and m.Extensions[2].Val = '{customerid}'";
                var deviceList = _dbase.SelectByQuery<Manifest>(query);
                foreach (var d in deviceList)
                {
                    devices.List.Add(d);
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }

            return devices;
        }

        public Manifest GetByTeammate(string teammateid)
        {
            Manifest manifest = null;

            try
            {
                var query =
                    $@"SELECT * FROM Manifest m where m.Extensions[3].Key = 'teammateId' and m.Extensions[3].Val = '{teammateid}'";
                var manifestList = _dbase.SelectByQuery<Manifest>(query);
                if (manifestList[0] != null)
                {
                    manifest = manifestList[0];
                }
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }

            return manifest;
        }

        public string GetDeviceTwin(string deviceId)
        {
            Twin twin;

            try
            {
                twin = AsyncHelper.RunSync<Twin>(() => _registryManager.GetTwinAsync(deviceId));
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }
            return twin.ToJson();
        }

        public void UpdateTwinPropertiesDirect(TwinPropertyRequest twinPropertyRequest)
        {
            // Update Twin Properties using a Direct Method

            var method = new CloudToDeviceMethod(twinPropertyRequest.Name) {ResponseTimeout = TimeSpan.FromSeconds(30)};

            // see if this command has any desired properties
            if (twinPropertyRequest.Properties.Count > 0)
            {
                var json = "{";
                for (var i = 0; i < twinPropertyRequest.Properties.Count; i++)
                {
                    json += twinPropertyRequest.Properties[i].Key + $":'{twinPropertyRequest.Properties[i].Val}'";
                    if (i < (twinPropertyRequest.Properties.Count - 1))
                        json += ",";
                }
                json += "}";

                method.SetPayloadJson(json);
            }

            try
            {
                AsyncHelper.RunSync<CloudToDeviceMethodResult>(() => _serviceClient.InvokeDeviceMethodAsync(twinPropertyRequest.DeviceId, method));
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }
        }

        public void UpdateTwinProperties(TwinPropertyRequest twinPropertyRequest)
        {
            // Update Twin Properties using Desired Properties

            try
            {
                var twin = AsyncHelper.RunSync<Twin>(() => _registryManager.GetTwinAsync(twinPropertyRequest.DeviceId));

                var json = "{ properties : { desired : {" + $"{twinPropertyRequest.Name}" + " : {";

                for (var i = 0; i < twinPropertyRequest.Properties.Count; i++)
                {
                    json += twinPropertyRequest.Properties[i].Key + $":'{twinPropertyRequest.Properties[i].Val}'";
                    if (i < (twinPropertyRequest.Properties.Count - 1))
                        json += ",";
                }
                json += "}}}}";

                AsyncHelper.RunSync<Twin>(() => _registryManager.UpdateTwinAsync(twinPropertyRequest.DeviceId, json, twin.ETag));
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }
        }

        public void UpdateTwinTags(TwinPropertyRequest twinPropertyRequest)
        {
            // Update Twin Tags

            try
            {
                var twin = AsyncHelper.RunSync<Twin>(() => _registryManager.GetTwinAsync(twinPropertyRequest.DeviceId));

                var json = "{ tags : {" + $"{twinPropertyRequest.Name}" + ": {";

                for (var i = 0; i < twinPropertyRequest.Properties.Count; i++)
                {
                    json += twinPropertyRequest.Properties[i].Key + $":'{twinPropertyRequest.Properties[i].Val}'";
                    if (i < (twinPropertyRequest.Properties.Count - 1))
                        json += ",";
                }
                json += "}}}";

                AsyncHelper.RunSync<Twin>(() => _registryManager.UpdateTwinAsync(twinPropertyRequest.DeviceId, json, twin.ETag));
            }
            catch (Exception err)
            {
                throw new Exception(Errors.ERR_DEVICE_BAGREQUEST, err);
            }
        }
    }
}
