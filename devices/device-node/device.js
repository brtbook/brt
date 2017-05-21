//////////////////////////////////////////////////////////////////////////////////////////////////
// Node.JS Azure IoT Simulator Device Client
//////////////////////////////////////////////////////////////////////////////////////////////////

'use strict';

var Client = require('azure-iot-device').Client;
var Protocol = require('azure-iot-device-mqtt').Mqtt;
var Message = require('azure-iot-device').Message;
var https = require('https');

var Manifest;
var Profile;
var DeviceClient;
var HeartbeatCadence;
var TelemetryCadence;

// Update these variables with the information from your Azure environment
var ApiHost = '[your-apim-host].azure-api.net';
var ApiKey = '[your-dev-key]'; 
var DeviceId = '[your-device-id]';

//////////////////////////////////////////////////////////////////////////////////////////////////
// Message Classes
//////////////////////////////////////////////////////////////////////////////////////////////////

function HeartbeatMessage(manifest) 
{
    this.Id = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0,3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
    this.DeviceId = manifest.DeviceId;
    this.Longitude = manifest.Longitude;
    this.Latitude = manifest.Latitude;
    this.Timestamp = new Date();
    this.MessageType = 1;
    this.Ack = "Node Device is Active";
}

function TelemetryMessage(manifest)
{
    this.Id = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0,3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
    this.DeviceId = manifest.DeviceId;
    this.Longitude = manifest.Longitude;
    this.Latitude = manifest.Latitude;
    this.MessageType = 4;
    this.UserId = '';
    this.Age = 0.0;
    this.Height = 0.0;
    this.Weight = 0.0;
    this.HeartRateBPM = 0.0;
    this.HeartrateRedZone = 0.0;
    this.HeartrateVariability = 0.0;
    this.BreathingRate = 0.0;
    this.Temperature = 0.0;
    this.Steps = 0.0;
    this.Velocity = 0.0;
    this.Altitude = 0.0;
    this.Cadence = 0.0;
    this.Speed = 0.0;
    this.HIB = 0.0;
    this.Status = 0;
}

//////////////////////////////////////////////////////////////////////////////////////////////////
// Helper Functions
//////////////////////////////////////////////////////////////////////////////////////////////////

function S4() { // generate Guids
    return (((1+Math.random())*0x10000)|0).toString(16).substring(1); 
}

function printResultFor(op) {
  return function printResult(err, res) {
    if (err) console.log(op + ' error: ' + err.toString());
    if (res) console.log(op + ' status: ' + res.constructor.name);
  };
}

function ReST(endpoint, method, success) { // make ReST calls

  var headers = {
      'Ocp-Apim-Trace': 'true',
      'Ocp-Apim-Subscription-Key': ApiKey     
  };
  
  var options = {
    host: ApiHost,
    path: endpoint,
    method: method,
    headers: headers
  };

  var req = https.request(options, function(res) {
    res.setEncoding('utf-8');
    var responseString = '';

    res.on('data', function(data) {
      responseString += data;
    });

    res.on('end', function() {
      //console.log(responseString);
      var responseObject = JSON.parse(responseString);
      success(responseObject);
    });
  });

  req.write('success');
  req.end();
}

//////////////////////////////////////////////////////////////////////////////////////////////////
// Reboot Handler
//////////////////////////////////////////////////////////////////////////////////////////////////

var onReboot = function(request, response) {

    // Respond the cloud app for the direct method
    response.send(200, 'Reboot started', function(err) {
        if (!err) {
            console.error('An error occured when sending a method response:\n' + err.toString());
        } else {
            console.log('Response to method \'' + request.methodName + '\' sent successfully.');
        }
    });

    // Report the reboot before the physical restart
    var date = new Date();
    var patch = {
        iothubDM : {
            reboot : {
                lastReboot : date.toISOString(),
            }
        }
    };

    // Get device Twin
    DeviceClient.getTwin(function(err, twin) {
        if (err) {
            console.error('could not get twin');
        } else {
            console.log('twin acquired');
            twin.properties.reported.update(patch, function(err) {
                if (err) throw err;
                console.log('Device reboot twin state reported')
            });  
        }
    });

    // Add your device's reboot API for physical restart.
    console.log('Rebooting!');
};

//////////////////////////////////////////////////////////////////////////////////////////////////
// Firmware Update Handler
//////////////////////////////////////////////////////////////////////////////////////////////////

var reportFWUpdateThroughTwin = function(twin, firmwareUpdateValue) {
  var patch = {
      iothubDM : {
        firmwareUpdate : firmwareUpdateValue
      }
  };

  twin.properties.reported.update(patch, function(err) {
    if (err) throw err;
    console.log('twin state reported -->' + JSON.stringify(patch));
  });
};

var simulateDownloadImage = function(imageUrl, callback) {
  var error = null;
  var image = "[fake image data]";

  console.log("Downloading image from " + imageUrl);

  callback(error, image);
}

var simulateApplyImage = function(imageData, callback) {
  var error = null;

  if (!imageData) {
    error = {message: 'Apply image failed because of missing image data.'};
  }

  callback(error);
}

var waitToDownload = function(twin, fwPackageUriVal, callback) {
  var now = new Date();

  reportFWUpdateThroughTwin(twin, {
    fwPackageUri: fwPackageUriVal,
    status: 'waiting',
    error : null,
    startedWaitingTime : now.toISOString()
  });
  setTimeout(callback, 4000);
};

var downloadImage = function(twin, fwPackageUriVal, callback) {
  var now = new Date();   

  reportFWUpdateThroughTwin(twin, {
    status: 'downloading',
  });

  setTimeout(function() {
    // Simulate download
    simulateDownloadImage(fwPackageUriVal, function(err, image) {

      if (err)
      {
        reportFWUpdateThroughTwin(twin, {
          status: 'downloadfailed',
          error: {
            code: error_code,
            message: error_message,
          }
        });
      }
      else {        
        reportFWUpdateThroughTwin(twin, {
          status: 'downloadComplete',
          downloadCompleteTime: now.toISOString(),
        });

        setTimeout(function() { callback(image); }, 4000);   
      }
    });

  }, 4000);
}

var applyImage = function(twin, imageData, callback) {
  var now = new Date();   

  reportFWUpdateThroughTwin(twin, {
    status: 'applying',
    startedApplyingImage : now.toISOString()
  });

  setTimeout(function() {

    // Simulate apply firmware image
    simulateApplyImage(imageData, function(err) {
      if (err) {
        reportFWUpdateThroughTwin(twin, {
          status: 'applyFailed',
          error: {
            code: err.error_code,
            message: err.error_message,
          }
        });
      } else { 
        reportFWUpdateThroughTwin(twin, {
          status: 'applyComplete',
          lastFirmwareUpdate: now.toISOString()
        });    

      }
    });

    setTimeout(callback, 4000);

  }, 4000);
}

var onFirmwareUpdate = function(request, response) {

  // Respond the cloud app for the direct method
  response.send(200, 'FirmwareUpdate started', function(err) {
    if (!err) {
      console.error('onFirmwareUpdate --> an error occured when sending a method response:\n' + err.toString());
    } else {
      console.log('onFirmwareUpdate --> Response to method \'' + request.methodName + '\' sent successfully.');
    }
  });

  var payload = request.payload;
  var fwPackageUri = payload.fwPackageUri;

  console.log('onFirmwareUpdate --> package uri == ' + fwPackageUri)

  // Obtain the device twin
  DeviceClient.getTwin(function(err, twin) {
    if (err) {
      console.error('onFirmwareUpdate --> could not get device twin.');
    } else {
      console.log('onFirmwareUpdate --> device twin acquired.');

      // Start the multi-stage firmware update
      waitToDownload(twin, fwPackageUri, function() {
        downloadImage(twin, fwPackageUri, function(imageData) {
          applyImage(twin, imageData, function() {});    
        });  
      });
    }
  });
}

//////////////////////////////////////////////////////////////////////////////////////////////////
// Desired Property Handler
//////////////////////////////////////////////////////////////////////////////////////////////////

var initConfigChange = function(twin) {

    console.log('initConfigChange --> starting');

     var currentCadenceConfig = twin.properties.reported.cadenceConfig;
     currentCadenceConfig.pendingConfig = twin.properties.desired.cadenceConfig;

     currentCadenceConfig.status = "Pending";

     var patch = {
         cadenceConfig: currentCadenceConfig
     };

     twin.properties.reported.update(patch, function(err) {
         if (err) {
             console.log('initConfigChange --> could not report properties');
         } else {
             console.log('initConfigChange --> pending config change: ' + JSON.stringify(patch));
             setTimeout(function() { completeConfigChange(twin); }, 5000);
         }
     });
 }

 var completeConfigChange =  function(twin) {

     console.log('completeConfigChange --> starting');

     var currentCadenceConfig = twin.properties.reported.cadenceConfig;
     currentCadenceConfig.heartbeat = currentCadenceConfig.pendingConfig.heartbeat;
     currentCadenceConfig.telemetry = currentCadenceConfig.pendingConfig.telemetry;
     currentCadenceConfig.status = "Success";

     delete currentCadenceConfig.pendingConfig;

     var patch = {
         cadenceConfig: currentCadenceConfig
     };

     patch.cadenceConfig.pendingConfig = null;

     twin.properties.reported.update(patch, function(err) {
         if (err) {
             console.error('completeConfigChange --> error reporting properties: ' + err);
         } else {
             console.log('completeConfigChange --> completed config change: ' + JSON.stringify(patch));

             // update the variables that control the heartbeat and telemetry cadence
             HeartbeatCadence = Number(twin.properties.reported.cadenceConfig.heartbeat);
             TelemetryCadence = Number(twin.properties.reported.cadenceConfig.telemetry);
         }
     });
 };

//////////////////////////////////////////////////////////////////////////////////////////////////
// Main
//////////////////////////////////////////////////////////////////////////////////////////////////

var main = function() {
    getManifest( function() {
        getUserProfile( function() {
            connectIoTHub( function() {
                setRebootHandler( function() {
                    setFirmwareUpdateHandler( function() {
                        setDesiredPropetyChangeHandler( function() {
                            sendHeartbeat( function() {
                                sendTelemetry( function() {
                               });
                           });
                        });
                    });
                });
            });
        });
    });
}

var getManifest = function(callback)
{
    var uri = '/dev/v1/device/manifests/id/' + DeviceId;
    ReST(uri, 'GET', function(data) 
    {
        Manifest = data;

        console.log('getManifest --> retrieved manifest for device ' + Manifest.SerialNumber);

        HeartbeatCadence = Number(Manifest.Extensions[0].Val);
        TelemetryCadence = Number(Manifest.Extensions[1].Val);

        callback();
    });
}

var getUserProfile = function(callback) 
{
    var uri = '/dev/v1/registry/profiles/id/' + Manifest.Extensions[3].Val;;
    ReST(uri, 'GET', function(data) 
    {
        Profile = data;
        console.log('getUserProfile --> retrieved user profile for ' + Profile.firstname + ' ' + Profile.lastname);
        callback();
    });
} 

var connectIoTHub = function(callback)
{
    var connectionString = 'HostName=' + Manifest.Hub + ";" + "DeviceId=" + Manifest.SerialNumber + ";" + "SharedAccessKey=" + Manifest.Key.PrimaryKey
    DeviceClient = Client.fromConnectionString(connectionString, Protocol);       
    DeviceClient.open(function(err) 
    {
        if (err) {
            console.error('connectIoTHub --> could not connect ' + err);
        }  else {
            console.log('connectIoTHub --> client connected to IoT Hub');
            callback();
        }
    });
}

var setRebootHandler = function (callback)
{
    callback();
    console.log('setHandlers --> reboot');
    DeviceClient.onDeviceMethod('reboot', onReboot);
}

var setFirmwareUpdateHandler = function (callback)
{
    callback();
    console.log('setHandlers --> firmware upgrade');
    DeviceClient.onDeviceMethod('firmwareUpdate', onFirmwareUpdate);
}

var setDesiredPropetyChangeHandler = function (callback)
{
    callback();

    DeviceClient.getTwin(function(err, twin) {

        if (err) {
            console.error('setHandlers --> could not get twin');
        } else {

            console.log('setHandlers --> on twin property changed');

            // report default cadence properties
            twin.properties.reported.cadenceConfig  = {
                heartbeat: Manifest.Extensions[0].Val,
                telemetry: Manifest.Extensions[1].Val
            }

            twin.on('properties.desired', function(desiredChange) {
                console.log("twin.properties.desired --> property change: "+ JSON.stringify(desiredChange));
                initConfigChange(twin);
            });
        }
    });
}

var sendHeartbeat = function(callback) 
{
    console.log('sendHeartbeat --> initiated');

    callback();
        
    // Create a heartbeat message and send it to the IoT Hub
    setInterval(function() 
    {
        console.log('sendHeartbeat --> ' + HeartbeatCadence);
        var data = JSON.stringify( new HeartbeatMessage(Manifest) );
        var message = new Message(data);
        console.log("Sending message: " + message.getData());
        DeviceClient.sendEvent(message, printResultFor('send'));
    }, HeartbeatCadence);
}

var sendTelemetry = function(callback) 
{
    callback();

    setInterval(function() 
    {
        console.log('sendTelemetry --> ' + TelemetryCadence)
        var reading = new TelemetryMessage(Manifest);
        reading.UserId = Profile.id;
        reading.Age = Profile.healthInformation.age;
        reading.Height = Profile.healthInformation.height;
        reading.Weight = Profile.healthInformation.weight;
        reading.Status = 1;
        
        // --- other sensor reading data here ---

        var data = JSON.stringify( reading );
        var message = new Message(data);
        console.log("Sending message: " + message.getData());
        DeviceClient.sendEvent(message, printResultFor('send'));
    }, TelemetryCadence);
}

//////////////////////////////////////////////////////////////////////////////////////////////////
// Main triggers a series of cascading functions that perform the following steps:
//
//  1. get manifest
//  2. get user profile
//  3. set up event handlers for reboot, firmware upgrade and desired property changes
//  4. start sending heartbeat messages
//  5. start sending telemetry messages
//
//////////////////////////////////////////////////////////////////////////////////////////////////

main();
