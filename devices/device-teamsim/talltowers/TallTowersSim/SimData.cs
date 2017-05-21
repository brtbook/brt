using brt.Microservices.Common.Wire;
using brt.Models.Device;
using brt.Models.Registry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using DataRow = brt.Models.Simulation.DataRow;
using DataSet = brt.Models.Simulation.DataSet;

namespace brt.Devices.Simulator
{
    public class Teammate
    {
        public Teammate(string dataSetName, Profile profile, Manifest manifest)
        {
            this.DataSetName = dataSetName;
            this.Profile = profile;
            this.Manifest = manifest;
        }

        public string DataSetName { get; set; }
        public Profile Profile { get; set; }    
        public Manifest Manifest { get; set; }
        public DataSet DataSet { get; set; }
    }

    public class SimulationDataMap : Dictionary<int, Teammate>
    {
    }

    public class SimData
    {
        private string SubscriptionKey { get; }
        private string DeviceApi { get; }
        private string RegistryApi { get; }
        private string SimulationApi { get; }

        public SimulationDataMap SimDataMap { get; set; }
        public string CompanyName { get; set; }

        public SimData()
        {
            CompanyName = ConfigurationManager.AppSettings["CompanyName"];
            DeviceApi = ConfigurationManager.AppSettings["DeviceAPI"];
            RegistryApi = ConfigurationManager.AppSettings["RegistryAPI"];
            SimulationApi = ConfigurationManager.AppSettings["SimulationAPI"];
            SubscriptionKey = ConfigurationManager.AppSettings["SubscriptionKey"];
        }

        public void Initialize()
        { 
            // get the company profile
            var companyProfile = GetProfileByName(this.CompanyName, this.CompanyName);

            // get the list of teammate profiles for this company 
            // and then remove the company profile from that list
            var profiles = GetCompanyProfiles(this.CompanyName);
            profiles.list.Remove(profiles.list.Find((profile => profile.id == companyProfile.id)));

            // get the device manifest for each member of this compnay
            var manifests = new Models.Device.Devices();

            // get the manifest for each teammate
            foreach (var p in profiles.list)
            {
                manifests.List.Add(GetManifestByExtension("teammateId", p.id));
            }

            // create a map that contains the teammates, their profiles and device manifest. 
            // This data will drvie the simulation
            SimDataMap = new SimulationDataMap
            {
                { 0, new Teammate(ConfigurationManager.AppSettings["teammate01"], profiles.list[0], manifests.List[0])},
                { 1, new Teammate(ConfigurationManager.AppSettings["teammate02"], profiles.list[1], manifests.List[1])},
                { 2, new Teammate(ConfigurationManager.AppSettings["teammate03"], profiles.list[2], manifests.List[2])},
                { 3, new Teammate(ConfigurationManager.AppSettings["teammate04"], profiles.list[3], manifests.List[3])},
                { 4, new Teammate(ConfigurationManager.AppSettings["teammate05"], profiles.list[4], manifests.List[4])},
                { 5, new Teammate(ConfigurationManager.AppSettings["teammate06"], profiles.list[5], manifests.List[5])},
                { 6, new Teammate(ConfigurationManager.AppSettings["teammate07"], profiles.list[6], manifests.List[6])},
                { 7, new Teammate(ConfigurationManager.AppSettings["teammate08"], profiles.list[7], manifests.List[7])},
                { 8, new Teammate(ConfigurationManager.AppSettings["teammate09"], profiles.list[8], manifests.List[8])},
                { 9, new Teammate(ConfigurationManager.AppSettings["teammate10"], profiles.list[9], manifests.List[9])},
                { 10, new Teammate(ConfigurationManager.AppSettings["teammate11"], profiles.list[10], manifests.List[10])},
                { 11, new Teammate(ConfigurationManager.AppSettings["teammate12"], profiles.list[11], manifests.List[11])},
                { 12, new Teammate(ConfigurationManager.AppSettings["teammate13"], profiles.list[12], manifests.List[12])},
                { 13, new Teammate(ConfigurationManager.AppSettings["teammate14"], profiles.list[13], manifests.List[13])},
                { 14, new Teammate(ConfigurationManager.AppSettings["teammate15"], profiles.list[14], manifests.List[14])},
            };
        }

        public Models.Device.Devices GetManifests()
        {
            var uriBuilder = new UriBuilder(DeviceApi)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var devices = JsonConvert.DeserializeObject<Models.Device.Devices>(json);

            return devices;
        }

        public Manifest GetManifestByExtension(string key, string value)
        {
            var uri = DeviceApi + $@"/extension/index/3/key/{key}/value/{value}";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var manifest = JsonConvert.DeserializeObject<Manifest>(json);

            return manifest;
        }

        public Manifest GetManifest(string deviceId)
        {
            var uri = DeviceApi + $"/id/{deviceId}";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var manifest = JsonConvert.DeserializeObject<Manifest>(json);

            return manifest;
        }

        public void UpdateManifest(Manifest manifest)
        {
            var uriBuilder = new UriBuilder(DeviceApi)
            {
                Query = SubscriptionKey
            };

            var json = JsonConvert.SerializeObject(manifest);

            Rest.Put(uriBuilder.Uri, json);
        }

        public Profiles GetCustomerProfiles()
        {
            var uri = RegistryApi + "/type/1";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var profiles = ModelManager.JsonToModel<Profiles>(json);

            return profiles;
        }

        public  Profile GetProfileById(string profileid)
        {
            var uri = RegistryApi + $"/id/{profileid}";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var profile = ModelManager.JsonToModel<Profile>(json);

            return profile;
        }

        public Profile GetProfileByName(string firstname, string lastname)
        {
            var uri = RegistryApi + $@"/firstname/{firstname}/lastname/{lastname}";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var profile = ModelManager.JsonToModel<Profile>(json);

            return profile;
        }

        public Profiles GetCompanyProfiles(string companyName)
        {
            var uri = RegistryApi + $"/company/{companyName}";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var profiles = ModelManager.JsonToModel<Profiles>(json);

            return profiles;
        }

        public DataSet GetDataSet(string name)
        {
            var uri = SimulationApi + $"/name/{ name}";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var dataset = ModelManager.JsonToModel<DataSet>(json);

            return dataset;
        }

        public DataRow GetNextDataRow(string rowid)
        {
            var uri = SimulationApi + $"/rows/id/{rowid}";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = SubscriptionKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var datarow = ModelManager.JsonToModel<DataRow>(json);

            return datarow;
        }
    }
}
