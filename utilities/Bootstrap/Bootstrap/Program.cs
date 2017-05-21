using System;
using System.Configuration;
using brt.Models.Account;
using brt.Models.Customer;
using brt.Models.Registry;
using System.Data.OleDb;
using brt.Microservices.Common.Wire;
using System.Text.RegularExpressions;
using brt.Models.Device;
using DataRow = System.Data.DataRow;
using DataSet = System.Data.DataSet;

namespace brt.Utils.Bootstrap
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // This console utility generates simulation data for the  Account, Customer, 
    // Application, Registry and Simulation services. 
    //-----------------------------------------------------------------------------------

    enum MenuEnum
    {
        Exit = 0,
        GenProfiles = 1,
        GenOrganizations = 2,
        GenSubscriptions = 3,
        GenConfigurations = 4,
        GenSimulatorDevices = 5,
        DeleteDevices = 6
    }

    class Program
    {
        private static string _devSubKey;
        private static string _opsSubKey;
        private static string _accountApi;
        private static string _applicationApi;
        private static string _customerApi;
        private static string _registryApi;
        private static string _deviceApi;

        public static string Operation { get; private set; }

        static void Main(string[] args)
        {
            _devSubKey = ConfigurationManager.AppSettings["DevSubKey"];
            _opsSubKey = ConfigurationManager.AppSettings["OpsSubKey"];
            _accountApi = ConfigurationManager.AppSettings["AccountAPI"];
            _applicationApi = ConfigurationManager.AppSettings["ApplicationAPI"];
            _customerApi = ConfigurationManager.AppSettings["CustomerAPI"];
            _registryApi = ConfigurationManager.AppSettings["RegistryAPI"];
            _deviceApi = ConfigurationManager.AppSettings["DeviceAPI"];

            Console.WriteLine();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("* B U S I N E S S  I N  R E A L - T I M E  B O O T S T R A P *");
            Console.WriteLine("**************************************************************");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Execute these steps in the specified order.");
                Console.WriteLine();
                Console.WriteLine("Select an operation from the menu and press [Enter]");
                Console.WriteLine();
                Console.WriteLine("    1. Create Registry Profiles");
                Console.WriteLine("    2. Create Customer Oganizations");
                Console.WriteLine("    3. Create Account Subscriptions");
                Console.WriteLine("    4. Create Application Configurations");
                Console.WriteLine("    5. Create Device Manifests");
                Console.WriteLine("    6. Delete Registered Devices");
                Console.WriteLine("    0. Exit");
                Console.WriteLine();
                Console.Write("Choice [0-6]: ");
                var menuChoice = (MenuEnum) System.Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                switch (menuChoice)
                {
                    case MenuEnum.GenProfiles:
                        Console.WriteLine("Generate Registry Profiles...");
                        GenProfiles();
                        break;
                    case MenuEnum.GenOrganizations:
                        Console.WriteLine("Generate Customer Organizations...");
                        GenOrganizations();
                        break;
                    case MenuEnum.GenSubscriptions:
                        Console.WriteLine("Generate Account Subscriptions...");
                        GenSubscriptions();
                        break;
                    case MenuEnum.GenConfigurations:
                        Console.WriteLine("Generate Application Configuration...");
                        GenConfigurations();
                        break;
                    case MenuEnum.GenSimulatorDevices:
                        Console.WriteLine("Generate Device Manifests...");
                        GenSimulatorDevices();
                        break;
                    case MenuEnum.DeleteDevices:
                        Console.WriteLine("Delete Registered Device...");
                        DeleteDevices();
                        break;
                    case MenuEnum.Exit:
                        Environment.Exit(0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        static void DeleteDevices()
        {
            var devices = GetAllManifests();
            foreach (var manifest in devices.List)
            {
                DeleteDevice(manifest.SerialNumber);
            }
        }

        static void GenProfiles()
        {
            var datafile = AppDomain.CurrentDomain.BaseDirectory + @"data\ProfileData.xlsx";

            var connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + datafile +
                          @"; Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'";
            var adapter = new OleDbDataAdapter("select * from [profiles$]", connStr);
            var ds = new DataSet();
            adapter.Fill(ds, "profiles");

            foreach (DataRow r in ds.Tables["profiles"].Rows)
            {
                var profile = new Profile
                {
                    firstname = r.ItemArray[0].ToString(),
                    lastname = r.ItemArray[1].ToString(),
                    address =
                    {
                        address1 = r.ItemArray[2].ToString(),
                        address3 = r.ItemArray[3].ToString(),
                        city = r.ItemArray[4].ToString(),
                        state = r.ItemArray[5].ToString(),
                        zip = r.ItemArray[6].ToString(),
                        country = r.ItemArray[7].ToString()
                    },
                    social = {phone = r.ItemArray[8].ToString()},
                    location =
                    {
                        longitude = Convert.ToDouble(r.ItemArray[9]),
                        latitude = Convert.ToDouble(r.ItemArray[10])
                    },
                    companyname = r.ItemArray[11].ToString(),
                    type = (ProfileTypeEnum) Convert.ToInt32(r.ItemArray[12]),
                    healthInformation =
                    {
                        age = Convert.ToInt32(r.ItemArray[13]),
                        height = Convert.ToInt32(r.ItemArray[14]),
                        weight = Convert.ToInt32(r.ItemArray[15]),
                        gender = (ProfileGenderEnum) Convert.ToInt32(r.ItemArray[16]),
                        race = (ProfileRaceEnum) Convert.ToInt32(r.ItemArray[17])
                    }
                };

                profile.imageUrl = "/images/" +  profile.firstname + profile.lastname + ".png";

                var cname = profile.companyname;
                cname = Regex.Replace(cname, @"\s+", "");

                switch (profile.type)
                {
                    case ProfileTypeEnum.Organization:
                        profile.lastname = profile.firstname;
                        profile.username = profile.firstname;
                        profile.social.email = $"info@{cname}.com";
                        break;
                    default:
                        profile.username = profile.firstname + @"." + profile.lastname;
                        profile.social.email = profile.username + $"@{cname}.com";
                        break;
                }

                CreateProfile(profile);
            }
        }

        private static void GenOrganizations()
        {
            // create organizations

            var wigiTech = new Organization();
            var tallTowers = new Organization();
            var complicatedBadger = new Organization();

            wigiTech.name = "WigiTech";
            wigiTech.description = "Widget Manufacturing";
            wigiTech.profileId = GetProfile("WigiTech", "WigiTech").id;

            tallTowers.name = "Tall Towers";
            tallTowers.description = "Utility Services";
            tallTowers.profileId = GetProfile("Tall Towers", "Tall Towers").id;

            complicatedBadger.name = "The Complicated Badger";
            complicatedBadger.description = "Delivery Service";
            complicatedBadger.profileId = GetProfile("The Complicated Badger", "The Complicated Badger").id;

            // create teams

            var profiles = GetAllCompanyProfiles(wigiTech.name);

            var wigiTechTeam1 = new Team();
            var wigiTechTeam2 = new Team();
            var wigiTechTeam3 = new Team();

            wigiTechTeam1.name = "Factory Floor First Shift";
            wigiTechTeam1.description = "Factory Floor First Shift";
            wigiTechTeam2.name = "Factory Floor Second Shift";
            wigiTechTeam2.description = "Factory Floor Second Shift";
            wigiTechTeam3.name = "Factory Floor Third Shift";
            wigiTechTeam3.description = "Factory Floor Third Shift";

            for (var i = 1; i < 6; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                wigiTechTeam1.teammates.Add(teammate);
            }

            for (var i = 6; i < 11; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                wigiTechTeam2.teammates.Add(teammate);
            }

            for (var i = 11; i < profiles.list.Count; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                wigiTechTeam3.teammates.Add(teammate);
            }

            wigiTech.list.Add(wigiTechTeam1);
            wigiTech.list.Add(wigiTechTeam2);
            wigiTech.list.Add(wigiTechTeam3);

            profiles = GetAllCompanyProfiles(tallTowers.name);

            var tallTowersTeam1 = new Team();
            var tallTowersTeam2 = new Team();
            var tallTowersTeam3 = new Team();

            tallTowersTeam1.name = "North-NJ";
            tallTowersTeam1.description = "Tower service team for north Jersey";
            tallTowersTeam2.name = "Southern-CT";
            tallTowersTeam2.description = "Tower service team for southern CT";
            tallTowersTeam3.name = "Manhattan-NY";
            tallTowersTeam3.description = "Tower service team for Manhattan Island";

            for (var i = 1; i < 6; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                tallTowersTeam1.teammates.Add(teammate);
            }

            for (var i = 6; i < 11; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                tallTowersTeam2.teammates.Add(teammate);
            }

            for (var i = 11; i < profiles.list.Count; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                tallTowersTeam3.teammates.Add(teammate);
            }

            tallTowers.list.Add(tallTowersTeam1);
            tallTowers.list.Add(tallTowersTeam2);
            tallTowers.list.Add(tallTowersTeam3);

            profiles = GetAllCompanyProfiles(complicatedBadger.name);

            var complicatedBadgerTeam1 = new Team();
            var complicatedBadgerTeam2 = new Team();
            var complicatedBadgerTeam3 = new Team();

            complicatedBadgerTeam1.name = "Driver Pool One ";
            complicatedBadgerTeam1.description = "Driver Pool One";
            complicatedBadgerTeam2.name = "Driver Pool Two";
            complicatedBadgerTeam2.description = "Driver Pool Two";
            complicatedBadgerTeam3.name = "Driver Pool Three";
            complicatedBadgerTeam3.description = "Driver Pool Three";

            for (var i = 1; i < 6; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                complicatedBadgerTeam1.teammates.Add(teammate);
            }

            for (var i = 6; i < 11; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                complicatedBadgerTeam2.teammates.Add(teammate);
            }

            for (var i = 11; i < profiles.list.Count; i++)
            {
                var teammate = new Teammate {profileId = profiles.list[i].id};
                complicatedBadgerTeam3.teammates.Add(teammate);
            }

            complicatedBadger.list.Add(complicatedBadgerTeam1);
            complicatedBadger.list.Add(complicatedBadgerTeam2);
            complicatedBadger.list.Add(complicatedBadgerTeam3);

            CreateOrganization(wigiTech);
            CreateOrganization(tallTowers);
            CreateOrganization(complicatedBadger);
        }

        private static void GenSubscriptions()
        {
            var wigiTechSub = new Subscription();
            var tallTowersSub = new Subscription();
            var complicatedBadgerSub = new Subscription();

            wigiTechSub.admin = new Administrator {profileId = GetProfile("Tognk", "Denyc").id};
            wigiTechSub.organizationId = GetProfile("WigiTech", "WigiTech").id;
            wigiTechSub.level = SubscriptionTypeEnum.Gold;

            tallTowersSub.admin = new Administrator {profileId = GetProfile("Fid", "Sidry").id};
            tallTowersSub.organizationId = GetProfile("Tall Towers", "Tall Towers").id;
            tallTowersSub.level = SubscriptionTypeEnum.Silver;

            complicatedBadgerSub.admin = new Administrator {profileId = GetProfile("Jyssa", "Anuk").id};
            complicatedBadgerSub.organizationId = GetProfile("The Complicated Badger", "The Complicated Badger").id;
            complicatedBadgerSub.level = SubscriptionTypeEnum.Bronze;

            CreateSubscription(wigiTechSub);
            CreateSubscription(tallTowersSub);
            CreateSubscription(complicatedBadgerSub);
        }

        private static void GenConfigurations()
        {
            var accounts = GetSubscriptions();
            foreach (var sub in accounts.list)
            {
                var config = new Models.Application.Configuration
                {
                    appname = "Business in Real-Time",
                    description = "Sample application for the book Business in Real-Time by Bob Familiar and Jeff Barnes",
                    modified = DateTime.Now,
                    version = "1.0.0.0",
                    settings =
                {
                    ["AccountId"] = sub.id,
                    ["OrganizationId"] = sub.organizationId
                }
                };

                CreateConfiguration(config);
            }
        }

        private static void GenSimulatorDevices()
        {
            Console.WriteLine("Loading Company Profiles");
            var wigiTechProfiles = GetAllCompanyProfiles("WigiTech");
            var tallTowersProfiles = GetAllCompanyProfiles("Tall Towers");
            var complicatedBadgersProfiles = GetAllCompanyProfiles("The Complicated Badger");

            var wigiTechProfile = GetProfile("WigiTech", "WigiTech");
            var tallTowersProfile = GetProfile("Tall Towers", "Tall Towers");
            var complicatedBadgerProfile = GetProfile("The Complicated Badger", "The Complicated Badger");

            Console.WriteLine("Generating Device Manifests for " + wigiTechProfile.companyname);
            foreach (var p in wigiTechProfiles.list)
            {
                var manifest = new Manifest
                {
                    Modified = DateTime.UtcNow,
                    Latitude = wigiTechProfile.location.latitude,
                    Longitude = wigiTechProfile.location.longitude,
                    SerialNumber = Guid.NewGuid().ToString(),
                    Manufacturer = wigiTechProfile.companyname,
                    ModelNumber = "WT-SIM-001",
                    FirmwareVersion = "1.0.0.0",
                    HardwareVersion = "1.0.0.0",
                    DeviceDescription = "WigiTech Worker Health Simulator",
                    Type = DeviceTypeEnum.Simulator,
                    Timezone = "EST",
                    Utcoffset = "UTC-5:00"
                };

                manifest.Extensions.Add(new DeviceProperty("heartbeat", "30000"));
                manifest.Extensions.Add(new DeviceProperty("telemetry", "10000"));
                manifest.Extensions.Add(new DeviceProperty("customerId", wigiTechProfile.id));
                manifest.Extensions.Add(new DeviceProperty("teammateId", p.id));

                CreateDeviceManifest(manifest);
                Console.WriteLine("Created Manifest for " + manifest.SerialNumber);
            }

            Console.WriteLine("Generating Device Manifests for " + tallTowersProfile.companyname);
            foreach (var p in tallTowersProfiles.list)
            {
                var manifest = new Manifest
                {
                    Modified = DateTime.UtcNow,
                    Latitude = tallTowersProfile.location.latitude,
                    Longitude = tallTowersProfile.location.longitude,
                    SerialNumber = Guid.NewGuid().ToString(),
                    Manufacturer = tallTowersProfile.companyname,
                    ModelNumber = "TT-SIM-001",
                    FirmwareVersion = "1.0.0.0",
                    HardwareVersion = "1.0.0.0",
                    DeviceDescription = "Tall Tower Worker Health Simulator",
                    Type = DeviceTypeEnum.Simulator,
                    Timezone = "EST",
                    Utcoffset = "UTC-5:00"
                };

                manifest.Extensions.Add(new DeviceProperty("heartbeat", "30000"));
                manifest.Extensions.Add(new DeviceProperty("telemetry", "10000"));
                manifest.Extensions.Add(new DeviceProperty("customerId", tallTowersProfile.id));
                manifest.Extensions.Add(new DeviceProperty("teammateId", p.id));

                CreateDeviceManifest(manifest);
                Console.WriteLine("Created Manifest for " + manifest.SerialNumber);
            }

            Console.WriteLine("Generating Device Manifests for " + complicatedBadgerProfile.companyname);
            foreach (var p in complicatedBadgersProfiles.list)
            {
                var manifest = new Manifest
                {
                    Modified = DateTime.UtcNow,
                    Latitude = complicatedBadgerProfile.location.latitude,
                    Longitude = complicatedBadgerProfile.location.longitude,
                    SerialNumber = Guid.NewGuid().ToString(),
                    Manufacturer = complicatedBadgerProfile.companyname,
                    ModelNumber = "CB-SIM-001",
                    FirmwareVersion = "1.0.0.0",
                    HardwareVersion = "1.0.0.0",
                    DeviceDescription = "The Complicated Badger Worker Health Simulator",
                    Type = DeviceTypeEnum.Simulator,
                    Timezone = "CST",
                    Utcoffset = "UTC-6:00"
                };

                manifest.Extensions.Add(new DeviceProperty("heartbeat", "30000"));
                manifest.Extensions.Add(new DeviceProperty("telemetry", "10000"));
                manifest.Extensions.Add(new DeviceProperty("customerId", complicatedBadgerProfile.id));
                manifest.Extensions.Add(new DeviceProperty("teammateId", p.id));

                CreateDeviceManifest(manifest);
                Console.WriteLine("Created Manifest for " + manifest.SerialNumber);
            }
        }

        private static Profile GetProfile(string firstname, string lastname)
        {
            var uri = _registryApi + @"/profiles/firstname/" + firstname + "/lastname/" + lastname;

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var profile = ModelManager.JsonToModel<Profile>(json);

            return profile;
        }

        private static Profiles GetAllCompanyProfiles(string company)
        {
            var uri = _registryApi + @"/profiles/company/" + company;

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var profiles = ModelManager.JsonToModel<Profiles>(json);

            return profiles;
        }

        private static void CreateProfile(Profile profile)
        {
            var uri = _registryApi + @"/profiles";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = ModelManager.ModelToJson<Profile>(profile);

            Rest.Post(uriBuilder.Uri, json);
        }

        private static void CreateOrganization(Organization org)
        {
            var uri = _customerApi + @"/organizations";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = ModelManager.ModelToJson<Organization>(org);

            Rest.Post(uriBuilder.Uri, json);
        }

        private static void CreateSubscription(Subscription sub)
        {
            var uri = _accountApi + @"/subscriptions";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = ModelManager.ModelToJson<Subscription>(sub);

            Rest.Post(uriBuilder.Uri, json);
        }

        private static Accounts GetSubscriptions()
        {
            var uri = _accountApi + @"/subscriptions";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var accounts = ModelManager.JsonToModel<Accounts>(json);

            return accounts;
        }

        private static void CreateConfiguration(Models.Application.Configuration config)
        {
            var uri = _applicationApi + @"/configurations";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _opsSubKey
            };

            var json = ModelManager.ModelToJson<Models.Application.Configuration>(config);

            Rest.Post(uriBuilder.Uri, json);
        }

        private static void CreateDeviceManifest(Manifest manifest)
        {
            var uri = _deviceApi + @"/manifests";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = ModelManager.ModelToJson(manifest);

            Rest.Post(uriBuilder.Uri, json);
        }

        private static Devices GetAllManifests()
        {
            var uri = _deviceApi + @"/manifests";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            var json = Rest.Get(uriBuilder.Uri);

            var devices = ModelManager.JsonToModel<Devices>(json);

            return devices;
        }

        private static void DeleteDevice(string deviceId)
        {
            var uri = _deviceApi + $@"/manifests/id/{deviceId}";

            var uriBuilder = new UriBuilder(uri)
            {
                Query = _devSubKey
            };

            Rest.Delete(uriBuilder.Uri);
        }
    }
}
