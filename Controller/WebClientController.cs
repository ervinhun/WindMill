using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mqtt.Controllers;

namespace WindMill.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WebClientController(IMqttClientService mqtt, ILogger<MyMqttController> logger) : ControllerBase
{
    [HttpPost(nameof(SetAction))]
    public async Task<IActionResult> SetAction([FromBody] ActionRequest request)
    {
        if (IsBlank(request.action) ||
            !Enum.TryParse<ActionType>(request.action, true, out var validatedAction))
        {
            return BadRequest("Invalid action. Allowed values: setInterval, stop, start, setPitch.");
        }

        var topic = $"farm/{request.FarmId}/windmill/{request.TurbineId}/command";

        var command = new ActionToSend()
        {
            action = JsonNamingPolicy.CamelCase.ConvertName(validatedAction.ToString())
        };

        switch (validatedAction)
        {
            case ActionType.Stop:
                command.reason = request.reason;
                break;

            case ActionType.SetInterval:
                if (!request.value.HasValue || request.value is <= 0 or > 60)
                {
                    return BadRequest("Value is required for set interval. The value must be within 0-60 seconds");
                }

                command.value = request.value;
                break;

            case ActionType.SetPitch:
                if (!request.angle.HasValue || request.angle is < 0 or > 30)
                {
                    return BadRequest("Angle must be within 0-30 degrees");
                }

                command.angle = request.angle;
                break;
            case ActionType.Start:
                // Doesn't need any additional parameters
                break;

            default:
                return BadRequest("Invalid action. Allowed values: setInterval, stop, start, setPitch.");
        }

        var jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var payload = JsonSerializer.Serialize(command, jsonOptions);
        logger.LogInformation($"Publishing command to topic: {topic} with payload: {payload} at {DateTime.UtcNow}");
        await mqtt.PublishAsync(topic, payload);

        return Ok(new { status = "Command sent", action = validatedAction.ToString() });
    }

    private static bool IsBlank(string? value)
    {
        return string.IsNullOrEmpty(value);
    }
}

public class ActionRequest
{
    public required string FarmId { get; set; }
    public required string TurbineId { get; set; }
    public required string action { get; set; } = "stop";
    public int? value { get; set; }
    public string? reason { get; set; }
    public double? angle { get; set; }
}

public class ActionToSend
{
    public required string action { get; set; }
    public int? value { get; set; }
    public string? reason { get; set; }
    public double? angle { get; set; }
}

public enum ActionType
{
    [EnumMember(Value = "setInterval")] SetInterval,
    [EnumMember(Value = "stop")] Stop,
    [EnumMember(Value = "start")] Start,
    [EnumMember(Value = "setPitch")] SetPitch
}