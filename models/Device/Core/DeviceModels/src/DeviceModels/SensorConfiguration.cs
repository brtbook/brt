using System.Collections.Generic;

namespace brt.Models.Device
{
    public enum SensorTypeEnum
    {
        NotSet = 0,
        Simulator = 1,
        Physical = 2
    }

    public enum SensorMeasurementTypeEnum
    {
        NotSet = 0,
        Temperature = 1,
        Humidity = 2,
        HeartRate = 3,
        BreathingRate = 4,
        Steps = 5,
        Velocity = 6,
        Altitude = 7
    }

    public enum AnalyticsOperationEnum
    {
        None = 0,
        FastFourierTransform = 1,
        Sum = 2,
        Average = 3,
        StandardDeviation = 4,
        LinearRegression = 5
    }

    public enum RuleEnum
    {
        None = 0,
        MustBeLessThan = 1,
        MustBeLessThenOrEqual = 2,
        MustBeGreaterThan = 3,
        MustBeGreaterThanOrEqual = 4,
        MustEqual = 5,
        MustNotEqual = 6,
        MustBeBetween = 7
    }

    public enum WindowActionType
    {
        Sms = 0,
        Api = 1,
        Log = 2
    }

    public enum PipeConfigurationType
    {
        NoteSet = 0,
        ReadPipe = 1,
        AnalyticsPipe = 2,
        RulesPipe = 3,
        WindowPipe = 4,
        FilterPipe = 5,
        SendPipe = 6
    }

    public class Rule
    {
        public Rule()
        {
            Type = RuleEnum.None;
            Val1 = 0.0;
            Val2 = 0.0;
        }

        public RuleEnum Type { get; set; }
        public double Val1 { get; set; }
        public double Val2 { get; set; }
    }

    public class RuleList : List<Rule> { }

    public class SensorRanges
    {
        public SensorRanges()
        {
            Normal = new double[2];
            Alert = new double[2];
            Alarm = new double[2];

            Normal[0] = 0.0;
            Normal[1] = 0.0;
            Alert[0] = 0.0;
            Alert[1] = 0.0;
            Alarm[0] = 0.0;
            Alarm[1] = 0.0;
        }

        public double[] Normal { get; set; }
        public double[] Alert { get; set; }
        public double[] Alarm { get; set; }
    }

    public class PipeConfiguration
    {
        public PipeConfiguration()
        {
            Type = PipeConfigurationType.NoteSet;
            IsActive = true;
        }

        public PipeConfigurationType Type { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReadPipeConfiguration : PipeConfiguration
    {
        public ReadPipeConfiguration()
        {
            Sleep = 5000;         
            Measurements = 1; 
            Type = PipeConfigurationType.ReadPipe;
            SensorType = SensorTypeEnum.NotSet;
            MeasurementType = SensorMeasurementTypeEnum.NotSet;
            Ranges = new SensorRanges();
        }

        public int Sleep { get; set; }
        public int Measurements { get; set; }
        public SensorTypeEnum SensorType { get; set; }
        public SensorMeasurementTypeEnum MeasurementType { get; set; }
        public SensorRanges Ranges { get; set; }
    }

    public class AnalyticsPipeConfiguration : PipeConfiguration
    {
        public AnalyticsPipeConfiguration()
        {
            Operation = AnalyticsOperationEnum.None;
            Type = PipeConfigurationType.AnalyticsPipe;
            IsActive = false;
        }

        public AnalyticsOperationEnum Operation { get; set; }
    }

    public class RulesPipeConfiguration : PipeConfiguration
    {
        public RulesPipeConfiguration()
        {
            Rules = new RuleList();
            Type = PipeConfigurationType.RulesPipe;
            IsActive = false;
        }

        public RuleList Rules { get; set; }
    }

    public class WindowAction
    {
        public WindowAction(WindowActionType type)
        {
            Type = type;
            IsActive = false;
            Action = string.Empty;
        }

        public WindowActionType Type { get; set; }
        public bool IsActive { get; set; }
        public string Action { get; set; }

    }

    public class WindowActionList : List<WindowAction>
    {
        public WindowAction GetAction(WindowActionType type)
        {
            WindowAction result = null;

            for (var i = 0; i < this.Count; i++)
            {
                if (this[i].Type == type)
                    result = this[i];
            }

            return result;
        }
    }

    public class WindowPipeConfiguration : PipeConfiguration
    {
        public WindowPipeConfiguration()
        {
            Threshold = 0;
            Readings = 0;
            Type = PipeConfigurationType.WindowPipe;
            IsActive = false;
            Actions = new WindowActionList();
        }

        public int Readings { get; set; }
        public int Threshold { get; set; }
        public WindowActionList Actions { get; set; }
    }

    public class FilterPipeConfiguration : PipeConfiguration
    {
        public FilterPipeConfiguration()
        {
            Type = PipeConfigurationType.FilterPipe;
            IsActive = false;
        }
    }

    public class SendPipeConfiguration : PipeConfiguration
    {
        public SendPipeConfiguration()
        {
            Type = PipeConfigurationType.SendPipe;
            IsActive = true;
            SendToCloud = true;
            SendToDisk = false;
            Filename = string.Empty;
        }

        public bool SendToCloud { get; set; }
        public bool SendToDisk { get; set; }
        public string Filename { get; set; }
    }

    public class SensorConfiguration
    {
        public SensorConfiguration()
        {
            ReadPipe = new ReadPipeConfiguration();
            AnalyticsPipe = new AnalyticsPipeConfiguration();
            RulesPipe = new RulesPipeConfiguration();
            WindowPipe = new WindowPipeConfiguration();
            FilterPipe = new FilterPipeConfiguration();
            SendPipe = new SendPipeConfiguration();
        }

        public SensorConfiguration(SensorTypeEnum sensorType, SensorMeasurementTypeEnum measurementType)
        {
            ReadPipe = new ReadPipeConfiguration
            {
                SensorType = sensorType,
                MeasurementType = measurementType
            };

            AnalyticsPipe = new AnalyticsPipeConfiguration();
            RulesPipe = new RulesPipeConfiguration();
            WindowPipe = new WindowPipeConfiguration();
            FilterPipe = new FilterPipeConfiguration();
            SendPipe = new SendPipeConfiguration();
        }

        public ReadPipeConfiguration ReadPipe { get; set; }
        public AnalyticsPipeConfiguration AnalyticsPipe { get; set; }
        public RulesPipeConfiguration RulesPipe { get; set; }
        public WindowPipeConfiguration WindowPipe { get; set; }
        public FilterPipeConfiguration FilterPipe { get; set; }
        public SendPipeConfiguration SendPipe { get; set; }
    }

    public class SensorConfigurationList : List<SensorConfiguration> { }
}
