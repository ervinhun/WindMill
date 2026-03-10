using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mqtt.Controllers;
using WindMill.DataAccess;
using WindMill.Dto.Web;
using WindMill.Util;

namespace WindMill.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WebClientController(IMqttClientService mqtt, ILogger<MyMqttController> logger, MyDbContext ctx) : ControllerBase
{
    [HttpPost(nameof(SetAction))]
    public async Task<IActionResult> SetAction([FromBody] ActionRequest request)
    {
        var user = getUserFromUserName(User.Identity?.Name);
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
        
        var historyToInsert = new HistoryDto()
        {
            TurbineId = request.TurbineId,
            UserId = user.Id,
            Action = command.action,
        };

        switch (validatedAction)
        {
            case ActionType.Stop:
                command.reason = request.reason;
                historyToInsert.Reason = request.reason;
                break;

            case ActionType.SetInterval:
                if (request.value is null or <= 0 or > 60)
                {
                    return BadRequest("Value is required for set interval. The value must be within 0-60 seconds");
                }

                command.value = request.value;
                historyToInsert.Value = request.value;
                break;

            case ActionType.SetPitch:
                if (request.angle is null or < 0 or > 30)
                {
                    return BadRequest("Angle must be within 0-30 degrees");
                }

                command.angle = request.angle;
                historyToInsert.Angle = request.angle;
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
        var savedHistory = await new SaveData(ctx).SaveHistory(historyToInsert);
        return Ok(JsonSerializer.Serialize(savedHistory));
    }

    private static bool IsBlank(string? value)
    {
        return string.IsNullOrEmpty(value);
    }
    
    private User getUserFromUserName(string userName)
    {
        var user = ctx.Users.FirstOrDefault(u => u.Username == userName);
        if (user == null)
        {
            throw new Exception($"User with username {userName} not found.");
        }
        return user;
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