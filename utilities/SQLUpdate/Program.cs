using System;
using System.Data;
using System.Data.SqlClient;

namespace brt.Tools.SQLUpdate
{
    class Program
    {
        public static string SqlServerName { get; private set; }
        public static string SqlDatabaseName { get; private set; }
        public static string SqlUserName { get; private set; }
        public static string SqlPassword { get; private set; }

        static void Main(string[] args)
        {
            SqlServerName = string.Empty;
            SqlDatabaseName = string.Empty;
            SqlUserName = string.Empty;
            SqlPassword = string.Empty;

            if (args.Length < 4)
            {
                Usage();
                return;
            }

            // parse command line arguments
            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-server":
                        i++;
                        SqlServerName = args[i];
                        break;
                    case "-database":
                        i++;
                        SqlDatabaseName = args[i];
                        break;
                    case "-username":
                        i++;
                        SqlUserName = args[i];
                        break;
                    case "-password":
                        i++;
                        SqlPassword = args[i];
                        break;
                    case "?": // help
                        Usage();
                        break;
                    default: // default
                        Usage();
                        break;
                }
            }

            if ((SqlServerName == string.Empty) ||
                (SqlDatabaseName == string.Empty) ||
                (SqlUserName == string.Empty) ||
                (SqlPassword == string.Empty))
            {
                Console.WriteLine("ERROR: missing command line arguments.");
                Usage();
                return;
            }

            var commandText = @"CREATE TABLE [dbo].[IotHubSensorReadings](
	                            [UserId] [char](256) NOT NULL,
	                            [Age] [float] NOT NULL,
	                            [Height] [float] NOT NULL,
	                            [Weight] [float] NOT NULL,
	                            [HeartRateBPM] [float] NOT NULL,
	                            [BreathingRate] [float] NOT NULL,
	                            [Temperature] [float] NOT NULL,
	                            [Steps] [float] NOT NULL,
	                            [Velocity] [float] NOT NULL,
	                            [Altitude] [float] NOT NULL,
	                            [Ventilization] [float] NOT NULL,
	                            [Activity] [float] NOT NULL,
	                            [Cadence] [float] NOT NULL,
	                            [Speed] [float] NOT NULL,
	                            [HIB] [float] NOT NULL,
	                            [HeartRateRedZone] [float] NOT NULL,
	                            [HeartrateVariability] [float] NOT NULL,
	                            [Status] [int] NOT NULL,
	                            [Id] [char](256) NOT NULL,
	                            [DeviceId] [char](256) NOT NULL,
	                            [MessageType] [int] NOT NULL,
	                            [Longitude] [float] NOT NULL,
	                            [Latitude] [float] NOT NULL,
	                            [Timestamp] [datetime2](7) NOT NULL,
	                            [EventProcessedUtcTime] [datetime2](7) NOT NULL,
	                            [PartitionId] [int] NOT NULL,
	                            [EventEnqueuedUtcTime] [datetime2](7) NOT NULL,
	                            [companyname] [char](256) NOT NULL,
	                            [imageUrl] [char](256) NOT NULL,
	                            [firstname] [char](256) NOT NULL,
	                            [lastname] [char](256) NOT NULL,
	                            [username] [char](256) NOT NULL,
	                            [type] [char](256) NOT NULL,
	                            [phone] [char](256) NOT NULL,
	                            [email] [char](256) NOT NULL,
	                            [gender] [char](256) NOT NULL,
	                            [race] [char](256) NOT NULL
                            )
                            CREATE CLUSTERED INDEX[IotHubSensorReadings_IX] ON[dbo].[IotHubSensorReadings]
                            (
	                            [Id] ASC
                            )";
            CreateTable(commandText);
        }

        private static void CreateTable(string commandText)
        {
            try
            {
                var scsBuilder = new SqlConnectionStringBuilder
                {
                    ["Server"] = "tcp:" + SqlServerName + ".database.windows.net,1433",
                    ["User ID"] = SqlUserName + "@" + SqlServerName,
                    ["Password"] = SqlPassword,
                    ["Database"] = SqlDatabaseName,
                    ["Trusted_Connection"] = false,
                    ["Integrated Security"] = false,
                    ["Encrypt"] = true,
                    ["Connection Timeout"] = 30
                };

                SqlConnection sqlConnection;

                using (sqlConnection = new SqlConnection(scsBuilder.ToString()))
                {
                    sqlConnection.Open();
                    IDbCommand command = new SqlCommand();
                    command.Connection = sqlConnection;
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error: {0}", err.Message);
            }
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: SQLUpdate -server [server name] -database [database name] -username [user name] -password [password]");
            Console.WriteLine("");
            Console.WriteLine("   -server     name of the sql database server");
            Console.WriteLine("   -database   name of the database");
            Console.WriteLine("   -username   sql server username");
            Console.WriteLine("   -password   sql server password");
            Console.WriteLine("");
        }
    }
}
