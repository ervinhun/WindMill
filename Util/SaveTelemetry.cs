using WindMill.Dto.Mqtt;
using WindMill.DataAccess;

namespace WindMill.Util;

public class SaveTelemetry(DataAccess.MyDbContext ctx)
{

    public void Save(TurbineMetric metric)
    {
        Console.WriteLine("Menteni vagy nem menteni? " + metric.TurbineId);
        /*var turbine = ctx.Turbines.FirstOrDefault(t => t.TurbineIdentifier == metric.TurbineId);
        if (turbine == null) return;
        var telemetry = new TurbineTelemetry
        {
            TurbineId = turbine.Id,
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
        ctx.TurbineTelemetries.Add(telemetry);
        ctx.SaveChanges();*/
    }
}