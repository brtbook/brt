var devKey = '[your-dev-key]';
var apimHost = 'https://[your-apim-host].azure-api.net/dev/v1/';
var manifests;

$( document ).ready( function () {
    GetAllCompanies();
});

function GetAllCompanies()
{
    var selectContent = '<option value="">Select a company</option>';

    $.getJSON( apimHost +  'registry/profiles/type/1?subscription-key=' + devKey, function ( data ) {

        $.each( data.list, function ()
        {
            selectContent += '<option value="' + this.id + '">' + this.companyname + '</option>';
        });

        $( '#companyList select' ).html( selectContent );
        $( document.getElementById( 'companyList' ).selectedIndex = 0 );
    });
}

function GetDevices( select )
{
    var tableContent = '';

    var uri = apimHost + 'device/manifests/customer/' + select.value + '?subscription-key=' + devKey;

    var idCount = 1;

    $.getJSON( uri, function ( data ) {
        manifests = data.List;

        $.each( manifests, function ()
        {
            tableContent += '<tr>';
            tableContent += '<td>' + this.SerialNumber + '</td>';
            tableContent += '<td>' + this.ModelNumber + '</td>';
            tableContent += '<td><input id="hb' + idCount + '" type="text" value="' + this.Extensions[0].Val + '"></td>';
            tableContent += '<td><input id="tl' + idCount + '" type="text" value="' + this.Extensions[1].Val + '"></td>';
            tableContent += '<td><button id="reboot' + idCount + '" width="200" onclick="Reboot(\'' + this.SerialNumber + '\')">Reboot Device</button></td>';
            tableContent += '<td><button id="update' + idCount + '" width="200" onclick="UpdateFirmware(\'' + this.SerialNumber + '\')">Update Firmware</button></td>';
            tableContent += '<td><button id="config' + idCount + '" width="200" onclick="Configure(\'' + this.SerialNumber + '\', \'' + idCount + '\')">Configure Cadence</button></td>';
            tableContent += '</tr>';
            idCount++;
        });
        $( '#deviceList table tbody' ).html( tableContent );
    });
}

function Reboot( serialNumber ) {

    var request = {
        DeviceId: serialNumber,
        Name: 'reboot',
        Properties: []
    };

    var twinPropertyRequest = JSON.stringify( request );

    var uri = apimHost + 'device/twin/properties/direct';
     
    $.ajax( {
        url: uri,
        type: "PUT",
        data: twinPropertyRequest,
        headers: {
            "Ocp-Apim-Trace": "true",
            "Ocp-Apim-Subscription-Key": devKey,
            "Content-Type":"application/json"
        },
        success: function ()
        {
            alert( "success" );
        },
        error: function ( xhr, status, error )
        {
            var err = eval( "(" + xhr.responseText + ")" );
            alert( status + ": " + err.Message );
        }
    });
}

function UpdateFirmware( serialNumber )
{
    var request = {
        DeviceId: serialNumber,
        Name: 'firmwareUpdate',
        Properties: [
            {
                Key: 'fwPackageUri',
                Val: 'https://[uri-to-firmware]'
            }
        ]
    };

    var twinPropertyRequest = JSON.stringify(request);

    var uri = apimHost + 'device/twin/properties/direct';

    $.ajax( {
        url: uri,
        type: "PUT",
        data: twinPropertyRequest,
        headers: {
            "Ocp-Apim-Trace": "true",
            "Ocp-Apim-Subscription-Key": devKey,
            "Content-Type": "application/json"
        },
        success: function ()
        {
            alert( "success" );
        },
        error: function ( xhr, status, error )
        {
            var err = eval( "(" + xhr.responseText + ")" );
            alert( status + ": " + err.Message );
        }
    });
}

function Configure( serialNumber, inputId ) {

    var hbInputId = 'hb' + String(inputId);
    var tlInputId = 'tl' + String(inputId);

    var heartbeat;
    $( heartbeat = document.getElementById( hbInputId ).value );

    var telemetry;
    $( telemetry = document.getElementById( tlInputId ).value);

    var request = {
        DeviceId: serialNumber,
        Name: 'cadenceConfig',
        Properties: [
            {
                Key: 'heartbeat',
                Val: heartbeat
            },
            {
                Key: 'telemetry',
                Val: telemetry
            }
        ]
    };

    var twinPropertyRequest = JSON.stringify( request );

    var uri = apimHost + 'device/twin/properties';

    $.ajax( {
        url: uri,
        type: "PUT",
        data: twinPropertyRequest,
        headers: {
            "Ocp-Apim-Trace": "true",
            "Ocp-Apim-Subscription-Key": devKey,
            "Content-Type": "application/json"
        },
        success: function ()
        {
            alert( "success" );
        },
        error: function ( xhr, status, error )
        {
            var err = eval( "(" + xhr.responseText + ")" );
            alert( status + ": " + err.Message );
        }
    });
}
