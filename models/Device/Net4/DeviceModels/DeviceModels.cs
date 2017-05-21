using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace brt.Models.Device
{
    public enum DeviceTypeEnum
    {
        NotSet = 0,
        Simulator = 1,
        MobilePhone = 2,
        SmartDevice = 3,
        EdgeGateway = 4
    }

    public class ModelBase
    {
        public ModelBase()
        {
            id = Guid.NewGuid().ToString();
            cachettl = 10;
        }

        public ModelBase(int _cachettl)
        {
            id = Guid.NewGuid().ToString();
            cachettl = _cachettl;
        }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        public int cachettl { get; set; }
    }

    public class DeviceProperty
    {
        public DeviceProperty()
        {
            Key = string.Empty;
            Val = string.Empty;
        }

        public DeviceProperty(string key, string val)
        {
            Key = key;
            Val = val;
        }

        public string Key { get; set; }
        public string Val { get; set; }
    }

    public class DevicePropertyList : List<DeviceProperty>
    {
        public string this[string key]
        {
            get
            {
                string val = null;
                for (var i = 0; i < Count; i++)
                {
                    if (this[i].Key != key) continue;
                    val = this[i].Val;
                    break;
                }
                return val;
            }
            set
            {
                int i;
                var matched = false;
                for (i = 0; i < Count; i++)
                {
                    matched = (this[i].Key == key);
                    if (matched) break;
                }
                if (matched)
                    this[i].Val = value;
                else
                    Add(new DeviceProperty(key, value));
            }
        }
    }

    public class SymmetricKey
    {
        public SymmetricKey()
        {
            PrimaryKey = string.Empty;
            SecondaryKey = string.Empty;
        }

        public string PrimaryKey { get; set; }
        public string SecondaryKey { get; set; }
    }

    public class Manifest : ModelBase
    {
        public Manifest()
        {
            Created = DateTime.UtcNow;
            Modified = DateTime.UtcNow;
            Type = DeviceTypeEnum.NotSet;
            Longitude = 0.0;
            Latitude = 0.0;
            Hub = string.Empty;
            Key = new SymmetricKey();
            Extensions = new DevicePropertyList();
            DeviceDescription = string.Empty;
            FirmwareVersion = string.Empty;
            HardwareVersion = string.Empty;
            Manufacturer = string.Empty;
            ModelNumber = string.Empty;
            SerialNumber = string.Empty;
            Timezone = string.Empty;
            Utcoffset = string.Empty;
        }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DeviceTypeEnum Type { get; set; }
        public SymmetricKey Key { get; set; }
        public DevicePropertyList Extensions { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Hub { get; set; }
        public string DeviceDescription { get; set; }
        public string FirmwareVersion { get; set; }
        public string HardwareVersion { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Timezone { get; set; }
        public string Utcoffset { get; set; }

        public bool IsValid()
        {
            return (SerialNumber != string.Empty);
        }
    }

    public class Devices : ModelBase
    {
        public Devices()
        {
            List = new List<Manifest>();
        }

        public List<Manifest> List { get; set; }
    }

    public class TwinPropertyRequest
    {
        public TwinPropertyRequest()
        {
            DeviceId = string.Empty;
            Name = string.Empty;
            Properties = new DevicePropertyList();
        }

        public string DeviceId { get; set; }
        public string Name { get; set; }
        public DevicePropertyList Properties { get; set; }
    }

}
