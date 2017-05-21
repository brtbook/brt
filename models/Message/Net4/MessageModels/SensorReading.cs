using System.Collections.Generic;

namespace brt.Models.Message
{
    public enum SensorStatus
    {
        Unknown = 0,
        Normal = 1,
        Warning = 2,
        Alert = 3,
        Alarm = 4
    }

    public class SimpleSensorReading : MessageBase
    {
        public SimpleSensorReading()
        {
            MessageType = MessageTypeEnum.SimpleSensorReading;
            Reading = 0.0d;
        }

        public double Reading;
    }

    public class SensorReading : MessageBase
    {
        public SensorReading()
        {
            MessageType = MessageTypeEnum.SensorReading;
            UserId = string.Empty;
            Age = 0.0d;
            Height = 0.0d;
            Weight = 0.0d;
            HeartRateBPM = 0.0d;
            HeartrateRedZone = 0.0d;
            HeartrateVariability = 0.0;
            BreathingRate = 0.0d;
            Temperature = 0.0d;
            Steps = 0.0d;
            Velocity = 0.0d;
            Altitude = 0.0d;
            Cadence = 0.0d;
            Speed = 0.0;
            HIB = 0.0d;
            Status = SensorStatus.Unknown;
        }

        public string UserId { get; set; }
        public double Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double HeartRateBPM { get; set; }
        public double BreathingRate { get; set; }
        public double Temperature { get; set; }
        public double Steps { get; set; }
        public double Velocity { get; set; }
        public double Altitude { get; set; }
        public double Ventilization { get; set; }
        public double Activity { get; set; }
        public double Cadence { get; set; }
        public double Speed { get; set; }
        public double HIB { get; set; }
        public double HeartrateRedZone { get; set; }
        public double HeartrateVariability { get; set; }
        public SensorStatus Status { get; set; }
    }

    public class SensorReadings : List<SensorReading> { }
}

