using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Azure.Devices.Client;
using brt.Models.Message;
using brt.Models.Device;
using Newtonsoft.Json;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartDevice
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Device Identity
        private const string DeviceSerialNumber = "[your-device-id]";
        private const string DeviceApi = "https://[your-apim-host].azure-api.net/dev/v1/device/manifests/id/" + DeviceSerialNumber;
        private const string SubscriptionKey = "subscription-key=[your-dev-key]";
        private static Manifest _deviceManifest;
        private static DeviceClient _deviceClient;

        // Thread Management
        private static Task _heartbeatTask;
        private static Task _telemetryTask;

        // GPIO
        private static GpioPinValue _value;
        private const int LedPin = 13;
        private static GpioPin _led;

        // SPI
        private static byte[] _readBuffer = new byte[3] { 0x00, 0x00, 0x00 };
        private static byte[] _writeBuffer = new byte[3] { 0x01, 0x80, 0x00 };
        private const string SpiControllerName = "SPI0";
        private const int SpiChipSelectLine = 0;
        private static SpiDevice _spiDisplay;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Status.Text = "Main Page Loaded";
            InitGPIO();
            Status.Text = "GPIO Initialized";
            InitSPI();
            Status.Text = "SPI Inititialized";

            _deviceManifest = await GetDeviceManifest();

            try
            {
                _deviceClient = DeviceClient.Create(_deviceManifest.Hub,
                AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(
                    _deviceManifest.SerialNumber,
                    _deviceManifest.Key.PrimaryKey),
                    TransportType.Amqp);

                Status.Text = $"{_deviceManifest.SerialNumber} Connected to Azure IoT Hub";
            }
            catch (Exception connectionErr)
            {
                Status.Text = connectionErr.Message;
            }

            StartHeartbeat();
            StartTelemetry();
        }

        private static void StartHeartbeat()
        {
            var cadence = Convert.ToInt32(_deviceManifest.Extensions["heartbeat"]);

            _heartbeatTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var heartbeat = new Heartbeat
                    {
                        Ack = $"{_deviceManifest.SerialNumber} is functioning",
                        Longitude = _deviceManifest.Longitude,
                        Latitude = _deviceManifest.Latitude,
                        DeviceId = _deviceManifest.SerialNumber
                    };

                    var json = JsonConvert.SerializeObject(heartbeat);

                    var message = new Message(Encoding.ASCII.GetBytes(json));

                    await _deviceClient.SendEventAsync(message);

                    await Task.Delay(cadence);
                }
            });
        }

        private static void StartTelemetry()
        {
            var cadence = Convert.ToInt32(_deviceManifest.Extensions["telemetry"]);

            //var random = new Random();

            _telemetryTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    // get the temperature setting from the sensor
                    _spiDisplay.TransferFullDuplex(_writeBuffer, _readBuffer);

                    // convert the value to farenheight
                    var temperature = ConvertToDouble(_readBuffer);
                    temperature = (((temperature * 5.0) / (1023 - 0.5)) * 100);

                    var sensorReading = new SimpleSensorReading
                    {
                        Longitude = _deviceManifest.Longitude,
                        Latitude = _deviceManifest.Latitude,
                        DeviceId = _deviceManifest.SerialNumber,
                        //Reading = random.Next(60,70)
                        Reading = temperature
                    };

                    var json = JsonConvert.SerializeObject(sensorReading);

                    var message = new Message(Encoding.ASCII.GetBytes(json));

                    await _deviceClient.SendEventAsync(message);

                    BlinkLED(500);

                    await Task.Delay(cadence);
                }
            });
        }

        private static async Task<Manifest> GetDeviceManifest()
        {
            var client = new HttpClient();
            var uriBuilder = new UriBuilder(DeviceApi)
            {
                Query = SubscriptionKey
            };
            var json = await client.GetStringAsync(uriBuilder.Uri);
            var deviceManifest = JsonConvert.DeserializeObject<Manifest>(json);
            return deviceManifest;
        }

        private static void InitGPIO()
        {
            _led = GpioController.GetDefault().OpenPin(LedPin);
            _led.Write(GpioPinValue.Low);
            _led.SetDriveMode(GpioPinDriveMode.Output);
        }

        public static async void BlinkLED(int duration)
        {
            _led.Write(GpioPinValue.High);
            await Task.Delay(duration);
            _led.Write(GpioPinValue.Low);
        }

        private static async void InitSPI()
        {
            try
            {
                var settings = new SpiConnectionSettings(SpiChipSelectLine)
                {
                    ClockFrequency = 500000,
                    Mode = SpiMode.Mode0
                };

                var spiAqs = SpiDevice.GetDeviceSelector(SpiControllerName);
                var deviceInfo = await DeviceInformation.FindAllAsync(spiAqs);

                _spiDisplay = await SpiDevice.FromIdAsync(deviceInfo[0].Id, settings);
            }
            catch (Exception ex)
            {
                throw new Exception("SPI Initialization Failed", ex);
            }
        }

        public static double ConvertToDouble(byte[] data)
        {
            int result = 0;
            int i = Convert.ToInt32("1100000000", 2);
            result = (data[1] << 8) & i;
            result |= (data[2] & 0xff);
            return result;
        }
    }
}
