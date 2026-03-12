using System.Text.Json;
using Mqtt.Controllers;
using Newtonsoft.Json;
using WindMill.DataAccess;
using WindMill.Dto.Mqtt;
using WindMill.Util;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WindMill.Controller;

public class MyMqttController(ILogger<MyMqttController> logger, SaveData sd) : MqttController
{
    [MqttRoute("farm/eb778064-da41-4f54-b0f0-e532f349d6da/windmill/{turbineId}/telemetry")]
    public async Task SubscribeToTelemetry(string turbineId, object payload)
    {
        var data = JsonSerializer.Serialize(payload);
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var metric = JsonSerializer.Deserialize<TurbineMetric>(data, options);
            if (metric == null) throw new JsonSerializationException("Metric is null after deserialization.");
                await sd.SaveTelemetry(metric);
        }
        catch (JsonException ex)
        {
            logger.LogError($"Failed to deserialize telemetry data: {ex.Message}");
        }
    }

    [MqttRoute("farm/+/windmill/{turbineId}/alert")]
    public async Task SubscribeToAlerts(string turbineId, object payload)
    {
        var data = JsonSerializer.Serialize(payload);
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var alert = JsonSerializer.Deserialize<TurbineAlert>(data, options);
            if (alert == null) throw new JsonSerializationException("Alert is null after deserialization.");
            await sd.SaveAlert(alert);
        }
        catch (JsonException ex)
        {
            logger.LogError($"Failed to deserialize alert data: {ex.Message}");
        }
    }
}