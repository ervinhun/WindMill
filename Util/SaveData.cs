using WindMill.Dto.Mqtt;
using WindMill.DataAccess;
using WindMill.Dto.Web;

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
            FarmId = alert.FarmId,
            TurbineId = alert.TurbineId,
            Timestamp = alert.Timestamp,
            Severity = alert.Severity,
            Message = alert.Message,
        };
        ctx.TurbineAlerts.Add(dbAlert);
        await ctx.SaveChangesAsync();
    }
    
    public async Task<HistoryResponseDto> SaveHistory(HistoryDto history)
    {
        var dbHistory = new TurbineCommandHistory()
        {
            TurbineId = history.TurbineId,
            UserId = history.UserId,
            Action = history.Action,
            Value = history.Value,
            Angle = history.Angle,
            Reason = history.Reason,
        };
        ctx.TurbineCommandHistories.Add(dbHistory);
        await ctx.SaveChangesAsync();
        return convertTurbineHistoryToResponseDto(dbHistory);
    }
    
    private HistoryResponseDto convertTurbineHistoryToResponseDto(TurbineCommandHistory history)
    {
        return new HistoryResponseDto()
        {
            Id = history.Id,
            TurbineId = history.TurbineId,
            UserId = history.UserId,
            Action = history.Action,
            Value = history.Value,
            Angle = history.Angle,
            Reason = history.Reason,
            CreatedAt = history.CreatedAt ?? DateTime.UtcNow
        };
    }
}