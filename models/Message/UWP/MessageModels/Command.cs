using System.Collections.Generic;

namespace brt.Models.Message
{
    public enum CommandTypeEnum
    {
        None = 0,
        StartTelemetry = 1,
        StopTelemetry = 2,
        FirmewareUpgrade = 3,
        SensorConfiguration = 4
    }

    public enum ParameterTypeEnum
    {
        NotSet = 0,
        Integer = 1,
        Double = 2,
        String = 3
    }

    public class Parameter
    {
        public Parameter()
        {
            Name = string.Empty;
            Type = ParameterTypeEnum.NotSet;
            Value = string.Empty;
        }

        public string Name { get; set; }
        public ParameterTypeEnum Type { get; set; }
        public string Value { get; set; }
    }

    public class Parameters
    {
        public Parameters()
        {
            list = new List<Parameter>();
        }

        public List<Parameter> list { get; set; }
    }

    public class Command : MessageBase
    {
        public Command(MessageTypeEnum sensorType)
        {
            MessageType = MessageTypeEnum.Command;
            CommandType = CommandTypeEnum.None;
            CommandParameters = string.Empty;
            SensorType = sensorType;
        }

        public CommandTypeEnum CommandType { get; set; }
        public MessageTypeEnum SensorType { get; set; }
        public string CommandParameters { get; set; }
    }
}
