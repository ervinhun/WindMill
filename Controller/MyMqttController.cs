using System.Data.Common;
using System.Text.Json;
using Mqtt.Controllers;
using Newtonsoft.Json;
using WindMill.DataAccess;
using WindMill.Dto.Mqtt;
using WindMill.DataAccess;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WindMill.Controller;

public class MyMqttController(ILogger<MyMqttController> logger, MyDbContext ctx) : MqttController
{
    [MqttRoute("farm/eb778064-da41-4f54-b0f0-e532f349d6da/windmill/{turbineId}/telemetry")]
    public async Task SubscribeToTelemetry(string turbineId, object payload)
    {
        var data = JsonSerializer.Serialize(payload);

        // Deserialize the JSON payload into a TurbineMetric object and save the turbine's name and status
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var metric = JsonSerializer.Deserialize<TurbineMetric>(data, options);
            if (metric == null) throw new JsonSerializationException("Anyád!");
            logger.LogInformation(
                $"Processing {metric.TurbineName} (ID: {metric.TurbineId}). Status: {metric.Status}");
            
            var telemetry = new TurbineTelemetry
            {
                TurbineId = metric.TurbineId,
                Timestamp = metric.Timestamp,
                WindSpeed = metric.WindSpeed,
                PowerOutput = metric.PowerOutput,
                NacelleDirection = metric.NacelleDirection,
                BladePitch = metric.BladePitch,
                GeneratorTemp = metric.GeneratorTemperature,
                GearboxTemp = metric.GearboxTemperature,
                Vibration = metric.Vibration,
                IsRunning = metric.Status.Equals("running", StringComparison.OrdinalIgnoreCase),
                CreatedAt = DateTime.UtcNow
            };
            try
            {
                ctx.TurbineTelemetries.Add(telemetry);
                await ctx.SaveChangesAsync();
            }
            catch(DbException ex)
            {
                logger.LogError($"Db Error: {ex.Message}");
            }
        }
        catch (JsonException ex)
        {
            logger.LogError($"Failed to deserialize telemetry data: {ex.Message}");
        }
    }

    [MqttRoute("farm/+/windmill/{turbineId}/alert")]
    public void SubscribeToAlerts(string turbineId, object payload)
    {
        logger.LogWarning(JsonSerializer.Serialize(payload));
    }
}