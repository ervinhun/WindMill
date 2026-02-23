using System.Text.Json;
using Mqtt.Controllers;

namespace WindMill.Controller;

public class MyMqttController(ILogger<MyMqttController> logger) : MqttController
{
    [MqttRoute("farm/eb778064-da41-4f54-b0f0-e532f349d6da/windmill/{turbineId}/telemetry")]
    public void SubscribeToTelemetry(string turbineId, object payload)
    {
        logger.LogInformation(JsonSerializer.Serialize(payload));
    }
    
    [MqttRoute("farm/+/windmill/{turbineId}/alert")]
    public void SubscribeToAlerts(string turbineId, object payload)
    {
        logger.LogWarning($"Alert from Turbine: {turbineId} - {JsonSerializer.Serialize(payload)}");
    }
}