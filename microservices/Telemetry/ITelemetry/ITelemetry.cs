using System;
using brt.Models.Telemetry;

namespace brt.Microservices.Telemetry.Interface
{
    public interface ITelemetry
    {
        TelemetryList GetTelemetry(string companyname, int usertype, string username, int count);
    }
}
