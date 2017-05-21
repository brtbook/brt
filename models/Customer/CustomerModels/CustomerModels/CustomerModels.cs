using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace brt.Models.Customer
{
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

    public class Teammate : ModelBase
    {
        public Teammate()
        {
            profileId = string.Empty;
        }

        public string profileId { get; set; }
    }

    public class Team : ModelBase
    {
        public Team()
        {
            teammates = new List<Teammate>();
            name = string.Empty;
            description = string.Empty;
        }

        public List<Teammate> teammates { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Organization : ModelBase
    {
        public Organization()
        {
            profileId = string.Empty;
            list = new List<Team>();
            name = string.Empty;
            description = string.Empty;
        }

        public string profileId { get; set; }
        public List<Team> list { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public bool IsValid()
        {
            return ((profileId != string.Empty) &&
                    (name != string.Empty) &&
                    (description != string.Empty) &&
                    (list != null));
        }
    }

    public class Customers : ModelBase
    {
        public Customers()
        {
            list = new List<Organization>();
        }

        public List<Organization> list { get; set; }
    }
}
