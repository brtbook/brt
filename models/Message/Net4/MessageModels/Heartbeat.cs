namespace brt.Models.Message
{
    public class Heartbeat : MessageBase
    {
        public Heartbeat()
        {
            MessageType = MessageTypeEnum.Heartbeat;
            Ack = string.Empty;
        }

        public string Ack { get; set; }
    }
}
