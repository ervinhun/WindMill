namespace WindMill.Dto.Mqtt;

public class TurbineMetric
{
    public required string TurbineId { get; set; }
    public required string FarmId { get; set; }
    public DateTime Timestamp { get; set; }
    public double WindSpeed { get; set; }
    public double PowerOutput { get; set; }
    public double Vibration { get; set; }
    public TurbineStatus Status { get; set; }
}

public enum TurbineStatus
{
    [System.Runtime.Serialization.EnumMember(Value = "stopped")]
    Stopped,
    [System.Runtime.Serialization.EnumMember(Value = "running")]
    Running
}