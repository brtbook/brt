using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace brt.Models.Application
{
    public static class SettingsKey
    {
        public const string ColorScheme = "ColorScheme";
    }

    public class Setting
    {
        public Setting()
        {
            key = string.Empty;
            val = string.Empty;
        }

        public Setting(string _key, string _val)
        {
            key = _key;
            val = _val;
        }

        public string key { get; set; }
        public string val { get; set; }
    }

    public class Settings : List<Setting> 
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
                    Add(new Setting(key, value));
            }
        }
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

    public class Configuration : ModelBase
    {
        public Configuration()
        {
            modified = DateTime.Now;
            version = "1.0.0.0";
            settings = new Settings();
            appname = string.Empty;
            description = string.Empty;
            version = "1.0.0.0";
        }

        public DateTime modified { get; set; }
        public string appname { get; set; }
        public string description { get; set; }
        public string version { get; set; }
        public Settings settings { get; set; }

        public bool isValid()
        {
            return ((id != string.Empty) && (appname != string.Empty) && settings != null);
        }
    }

    public class Applications : ModelBase
    {
        public Applications()
        {
            id = Guid.NewGuid().ToString();
            cachettl = 10;
            list = new List<Configuration>();
        }

        public List<Configuration> list { get; set; }
    }
}
