using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace brt.Models.Account
{
    public enum SubscriptionTypeEnum
    {
        NotSet = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3
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

    public class Administrator
    {
        public Administrator()
        {
            profileId = string.Empty;
        }

        public string profileId { get; set; }
    }

    public class Subscription : ModelBase
    {
        public Subscription()
        {
            organizationId = string.Empty;
            admin = new Administrator();
        }

        public string organizationId { get; set; }
        public Administrator admin { get; set; }
        public SubscriptionTypeEnum level { get; set; }

        public bool IsValid()
        {
            return ((organizationId != string.Empty) && (admin.profileId != string.Empty) && (level != SubscriptionTypeEnum.NotSet));
        }
    }

    public class Accounts : ModelBase
    {
        public Accounts()
        {
            list = new List<Subscription>();
        }

        public List<Subscription> list { get; set; }
    }
}
