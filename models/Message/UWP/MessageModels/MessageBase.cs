using System;

namespace brt.Models.Message
{
    public enum MessageTypeEnum
    {
        NotSet = 0,
        Heartbeat = 1,
        Command = 2,
        SimpleSensorReading = 3,
        SensorReading = 4
    }

    public class MessageBase
    {
        public MessageBase()
        {
            Id = Guid.NewGuid().ToString();
            DeviceId = string.Empty;
            MessageType = MessageTypeEnum.NotSet;
            Longitude = 0.0;
            Latitude = 0.0;
            Timestamp = DateTime.Now;
        }

        public string Id { get; set; }
        public string DeviceId { get; set; }
        public MessageTypeEnum MessageType { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
