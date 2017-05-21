using System;
using System.Collections.Generic;

namespace brt.Models.Telemetry
{
    public class ModelBase
    {
        public ModelBase()
        {
            id = Guid.NewGuid().ToString();
            deviceid = string.Empty;
            longitude = 0.0d;
            latitude = 0.0d;
            timestamp = DateTime.UtcNow;
        }

        public string id { get; set; }
        public string deviceid { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class UserInfo
    {
        public UserInfo()
        {
            type = string.Empty;
            userid = string.Empty;
            companyname = string.Empty;
            imageurl = string.Empty;
            firstname = string.Empty;
            lastname = string.Empty;
            username = string.Empty;
            phone = string.Empty;
            email = string.Empty;
            gender = string.Empty;
            race = string.Empty;
        }

        public string type { get; set; }
        public string userid { get; set; }
        public string companyname { get; set; }
        public string imageurl { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string race { get; set; }
    }

    public class SensorReadings
    {
        public SensorReadings()
        {
            age = 0.0d;
            height = 0.0d;
            heartRateBPM = 0.0d;
            breathingRate = 0.0d;
            temperature = 0.0d;
            steps = 0.0d;
            velocity = 0.0d;
            altitude = 0.0d;
            ventilization = 0.0d;
            activity = 0.0d;
            cadence = 0.0d;
            speed = 0.0d;
            HIB = 0.0d;
            heartRateRedZone = 0.0d;
            heartrateVariability = 0.0d;
        }

        public double age { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public double heartRateBPM { get; set; }
        public double breathingRate { get; set; }
        public double temperature { get; set; }
        public double steps { get; set; }
        public double velocity { get; set; }
        public double altitude { get; set; }
        public double ventilization { get; set; }
        public double activity { get; set; }
        public double cadence { get; set; }
        public double speed { get; set; }
        public double HIB { get; set; }
        public double heartRateRedZone { get; set; }
        public double heartrateVariability { get; set; }
    }

    public class UserTelemetry : ModelBase
    {
        public UserTelemetry()
        {
            User = new UserInfo();
            Readings = new SensorReadings();
        }

        public UserInfo User { get; set; }
        public SensorReadings Readings { get; set; }
    }

    public class TelemetryList
    {
        public TelemetryList()
        {
            list = new List<UserTelemetry>();
        }

        public List<UserTelemetry> list { get; set; }
    }
}
