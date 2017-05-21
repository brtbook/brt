using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace brt.Models.Registry
{
    public enum ProfileTypeEnum
    {
        NotSet = 0,
        Organization = 1,
        Administrator = 2,
        Employee = 3,
        Contractor = 4,
        Temporary = 5,
        Partner = 6
    }

    public enum ProfileGenderEnum
    {
        NotSet = 0,
        Female = 1,
        Male = 2,
        Neutral = 3
    }

    public enum ProfileRaceEnum
    {
        NotSet = 0,
        AmerIndianEskimo = 1,
        AsianPacIslander = 2,
        Black = 3,
        White = 4,
        Other = 5
    }

    public class ModelBase
    {
        public ModelBase()
        {
            id = Guid.NewGuid().ToString();
            cachettl = 1;
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

    public class Address
    {
        public Address()
        {
            this.address1 = string.Empty;
            this.address2 = string.Empty;
            this.address3 = string.Empty;
            this.city = string.Empty;
            this.state = string.Empty;
            this.zip = string.Empty;
            this.country = string.Empty;
        }

        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }

    public class Social
    {
        public Social()
        {
            this.phone = string.Empty;
            this.email = string.Empty;
            this.facebook = string.Empty;
            this.twitter = string.Empty;
            this.linkedin = string.Empty;
            this.blog = string.Empty;
        }

        public string phone { get; set; }
        public string email { get; set; }
        public string linkedin { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string blog { get; set; }
    }

    public class Geo
    {
        public Geo()
        {
            longitude = 0.0;
            latitude = 0.0;
        }

        public double longitude { get; set; }
        public double latitude { get; set; }
    }

    public class Preference
    {
        public Preference()
        {
            key = string.Empty;
            val = string.Empty;
        }

        public Preference(string _key, string _val)
        {
            key = _key;
            val = _val;
        }

        public string key { get; set; }
        public string val { get; set; }
    }

    public class Preferences : List<Preference>
    {
        public string this[string key]
        {
            get
            {
                string val = null;
                for (var i = 0; i < Count; i++)
                {
                    if (this[i].key != key) continue;
                    val = this[i].val;
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
                    matched = (this[i].key == key);
                    if (matched) break;
                }
                if (matched)
                    this[i].val = value;
                else
                    Add(new Preference(key, value));
            }
        }
    }

    public class HealthInformation
    {
        public HealthInformation()
        {
            age = 0.0d;
            height = 0.0d;
            weight = 0.0d;
            gender = ProfileGenderEnum.NotSet;
            race = ProfileRaceEnum.NotSet;
        }

        public double age { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public ProfileGenderEnum gender { get; set; }
        public ProfileRaceEnum race { get; set; }
    }

    public class Profile : ModelBase
    {
        public Profile()
        {
            authid = string.Empty;
            firstname = string.Empty;
            lastname = string.Empty;
            username = string.Empty;
            imageUrl = string.Empty;
            type = ProfileTypeEnum.NotSet;
            address = new Address();
            social = new Social();
            healthInformation = new HealthInformation();
            preferences = new Preferences();
            location = new Geo();
        }

        public Profile(string authid, string firstname, string lastname, string username, string imageUrl, ProfileTypeEnum type)
        {
            this.authid = authid;
            this.firstname = firstname;
            this.lastname = lastname;
            this.username = username;
            this.imageUrl = imageUrl;
            this.type = type;
            address = new Address();
            social = new Social();
            healthInformation = new HealthInformation();
            preferences = new Preferences();
            location = new Geo();
        }

        public string authid { get; set; }
        public string companyname { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string imageUrl { get; set; }
        public ProfileTypeEnum type { get; set; }
        public Address address { get; set; }
        public Social social { get; set; }
        public HealthInformation healthInformation { get; set; }
        public Preferences preferences { get; set; }
        public Geo location { get; set; }

        public bool IsValid()
        {
            return ((firstname != string.Empty) &&
                    (lastname != string.Empty) &&
                    (username != string.Empty) &&
                    (type != ProfileTypeEnum.NotSet) &&
                    (address != null) &&
                    (social != null) &&
                    (preferences != null) &&
                    (location != null));
        }
    }

    public class Profiles : ModelBase
    {
        public Profiles()
        {
            list = new List<Profile>();
        }

        public List<Profile> list { get; set; }
    }
}
