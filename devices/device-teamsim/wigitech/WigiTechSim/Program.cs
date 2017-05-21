using System.Text;
using System.Threading.Tasks;
using brt.Models.Message;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;

namespace brt.Devices.Simulator
{
    public class Program
    {
        private static SimData _simData;

        static void Main(string[] args)
        {
            _simData = new SimData();

            _simData.Initialize();

            Console.WriteLine($"Simulating {_simData.SimDataMap[0].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[0].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[0].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[0]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[1].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[1].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[1].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[1]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[2].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[2].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[2].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[2]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[3].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[3].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[3].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[3]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[4].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[4].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[4].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[4]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[5].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[5].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[5].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[5]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[6].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[6].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[6].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[6]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[7].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[7].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[7].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[7]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[8].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[8].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[8].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[8]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[9].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[9].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[9].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[9]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[10].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[10].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[10].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[10]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[11].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[11].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[11].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[11]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[12].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[12].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[12].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[12]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[13].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[13].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[13].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[13]);

            Console.WriteLine($"Simulating {_simData.SimDataMap[14].DataSetName}");
            Console.WriteLine($"  Profile Id : {_simData.SimDataMap[14].Profile.id}");
            Console.WriteLine($"  Device Id  : {_simData.SimDataMap[14].Manifest.SerialNumber}");
            SendTelemetry(_simData.SimDataMap[14]);

            Console.ReadLine();
        }

        private static void SendTelemetry(Teammate teammate)
        {
            var rowindex = 0;
            const int brzrkr = 1; // inflate readings to quickly simulate alarm conditions

            // initialize the simulation dataset
            teammate.DataSet = _simData.GetDataSet(teammate.DataSetName);

            // connect to IoT Hub
            var auth = AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(teammate.Manifest.SerialNumber, teammate.Manifest.Key.PrimaryKey);
            var client = DeviceClient.Create(teammate.Manifest.Hub, auth);

            // start a background thread to send telemetry
            var simTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var datarow = _simData.GetNextDataRow(teammate.DataSet.rows[rowindex++]);

                    if (rowindex >= teammate.DataSet.rows.Count)
                    {
                        rowindex = 0;
                    }

                    var reading = new SensorReading
                    {
                        UserId = teammate.Profile.id,
                        DeviceId = teammate.Manifest.SerialNumber,
                        Longitude = teammate.Manifest.Longitude,
                        Latitude = teammate.Manifest.Latitude,
                        Status = SensorStatus.Normal,
                        Timestamp = DateTime.Now,
                        Age = teammate.Profile.healthInformation.age,
                        Weight = teammate.Profile.healthInformation.weight,
                        Height = teammate.Profile.healthInformation.height,
                        BreathingRate = datarow.columns[0].dataValue * brzrkr,
                        Ventilization = datarow.columns[1].dataValue * brzrkr,
                        Activity = datarow.columns[2].dataValue * brzrkr,
                        HeartRateBPM = datarow.columns[3].dataValue * brzrkr,
                        Cadence = datarow.columns[4].dataValue * brzrkr,
                        Velocity = datarow.columns[5].dataValue * brzrkr,
                        Speed = datarow.columns[6].dataValue * brzrkr,
                        HIB = datarow.columns[7].dataValue * brzrkr,
                        HeartrateRedZone = datarow.columns[8].dataValue * brzrkr,
                        HeartrateVariability = datarow.columns[9].dataValue * brzrkr,
                        Temperature = datarow.columns[10].dataValue * brzrkr
                    };

                    var json = JsonConvert.SerializeObject((object)reading);

                    var message = new Message(Encoding.ASCII.GetBytes(json));

                    await client.SendEventAsync(message);

                    System.Threading.Thread.Sleep(System.Convert.ToInt32(teammate.Manifest.Extensions["telemetry"]));
                }
            });
        }
    }
}
