using WindMill.Dto.Mqtt;
using WindMill.DataAccess;

namespace WindMill.Util;

public class SaveData(MyDbContext ctx)
{

    public async Task SaveTelemetry(TurbineMetric metric)
    {
        var telemetry = new TurbineTelemetry
        {
            TurbineId = metric.TurbineId,
            TurbineName = metric.TurbineName,
            FarmId = metric.FarmId,
            Timestamp = metric.Timestamp,
            WindSpeed = metric.WindSpeed,
            WindDirection = metric.WindDirection,
            AmbientTemperature = metric.AmbientTemperature,
            RotorSpeed = metric.RotorSpeed,
            PowerOutput = metric.PowerOutput,
            NacelleDirection = metric.NacelleDirection,
            BladePitch = metric.BladePitch,
            GeneratorTemp = metric.GeneratorTemp,
            GearboxTemp = metric.GearboxTemp,
            Vibration = metric.Vibration,
            IsRunning = metric.Status.Equals("running", StringComparison.OrdinalIgnoreCase),
            CreatedAt = DateTime.UtcNow
        };
        ctx.TurbineTelemetries.Add(telemetry);
        await ctx.SaveChangesAsync();
    }
    
    public async Task SaveAlert(TurbineAlert alert)
    {
        var dbAlert = new TurbineAlert
        {
            TurbineId = alert.TurbineId,
            Timestamp = alert.Timestamp,
            Severity = alert.Severity,
            Message = alert.Message,
            CreatedAt = DateTime.UtcNow
        };
        ctx.TurbineAlerts.Add(dbAlert);
        await ctx.SaveChangesAsync();
    }
}