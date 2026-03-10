using System.ComponentModel.DataAnnotations;

namespace WindMill.Dto.Web;

public class HistoryDto
{
    [Required]
    public string TurbineId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Action { get; set; }
    public int? Value { get; set; }
    public double? Angle { get; set; }
    public string? Reason { get; set; }
}

public class HistoryResponseDto
{
    public long Id { get; set; }
    public string TurbineId { get; set; }
    public Guid UserId { get; set; }
    public string Action { get; set; }
    public int? Value { get; set; }
    public double? Angle { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
}