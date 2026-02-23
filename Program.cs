using Mqtt.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMqttControllers();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();

var app = builder.Build();

app.MapControllers();
app.UseOpenApi();
app.UseSwaggerUi();

var mqtt = app.Services.GetRequiredService<IMqttClientService>();
await mqtt.ConnectAsync("broker.hivemq.com", 1883);


await app.RunAsync();



