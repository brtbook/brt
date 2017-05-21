using System;
using System.Data;
using System.Data.SqlClient;
using brt.Microservices.Telemetry.Interface;
using brt.Models.Telemetry;

namespace brt.Microservices.Telemetry.Service
{
    public class TelemetryService : ITelemetry
    {
        private SqlConnection _conn;
        private readonly string _sqlConnStr;

        public TelemetryService(string sqlConnStr)
        {
            _conn = null;
            _sqlConnStr = sqlConnStr;
        }

        private void Open()
        {
            _conn = new SqlConnection(_sqlConnStr);
            _conn.Open();
        }

        private void Close()
        {
            _conn.Close();
        }

        private static TelemetryList TransformTelemetry(IDataReader rdr)
        {
            var telemetryList = new TelemetryList();

            while (rdr.Read())
            {
                var telemetry = new UserTelemetry();

                telemetry.deviceid = rdr[0].ToString().Trim();
                telemetry.longitude = (double) rdr[1];
                telemetry.latitude = (double) rdr[2];
                telemetry.timestamp = Convert.ToDateTime(rdr[3].ToString());

                telemetry.User.userid = rdr[4].ToString().Trim();
                telemetry.User.companyname = rdr[5].ToString().Trim();
                telemetry.User.imageurl = rdr[6].ToString().Trim();
                telemetry.User.firstname = rdr[7].ToString().Trim();
                telemetry.User.lastname = rdr[8].ToString().Trim();
                telemetry.User.username = rdr[9].ToString().Trim();
                telemetry.User.type = rdr[10].ToString().Trim();
                telemetry.User.phone = rdr[11].ToString().Trim();
                telemetry.User.email = rdr[12].ToString().Trim();
                telemetry.User.gender = rdr[13].ToString().Trim();
                telemetry.User.race = rdr[14].ToString().Trim();

                telemetry.Readings.age = (double) rdr[15];
                telemetry.Readings.height = (double) rdr[16];
                telemetry.Readings.weight = (double) rdr[17];
                telemetry.Readings.heartRateBPM = (double) rdr[18];
                telemetry.Readings.heartRateRedZone = (double) rdr[19];
                telemetry.Readings.heartrateVariability = (double) rdr[20];
                telemetry.Readings.temperature = (double) rdr[21];
                telemetry.Readings.steps = (double) rdr[22];
                telemetry.Readings.velocity = (double) rdr[23];
                telemetry.Readings.altitude = (double) rdr[24];
                telemetry.Readings.ventilization = (double) rdr[25];
                telemetry.Readings.activity = (double) rdr[26];
                telemetry.Readings.cadence = (double) rdr[27];
                telemetry.Readings.speed = (double) rdr[28];

                telemetryList.list.Add(telemetry);
            }

            rdr.Close();

            return telemetryList;
        }

        public TelemetryList GetTelemetry(string companyname, int usertype, string username, int count)
        {
            // type == 2 means a manager is requesting data for their employees so restriction is based on companyname
            // type == 3 means an employee is requsting their data s the query is restricted by companyname and their username

            string whereClause;

            switch (usertype)
            {
                case 2:
                    whereClause = $"where type='3' and companyname='{companyname}'";
                    break;
                case 3:
                    whereClause = $"where type='3' and companyname='{companyname}' and username='{username}'";
                    break;
                default:
                    throw new Exception("Error: User Type not specified");
            }

            var topClause = (count > 0) ? "top " + count.ToString() : "";

            Open();

            var query = "select " + topClause +
                        " [DeviceId],[Longitude],[Latitude],[Timestamp],[UserId],[companyname],[imageUrl],[firstname],[lastname],[username],[type],[phone],[email],[gender],[race],[Age],[Height],[Weight],[HeartRateBPM],[BreathingRate],[HeartRateRedZone],[HeartrateVariability],[Temperature],[Steps],[Velocity],[Altitude],[Ventilization],[Activity],[Cadence],[Speed],[HIB] from IoTHubSensorReadings " +
                        whereClause + " order by timestamp desc";

            var cmd = new SqlCommand(query, _conn);

            var rdr = cmd.ExecuteReader();

            TelemetryList tempTelemetryList = null;

            if (rdr.HasRows)
                tempTelemetryList = TransformTelemetry(rdr);

            Close();

            return tempTelemetryList;
        }
    }
}
