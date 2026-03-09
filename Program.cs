using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mqtt.Controllers;
using NSwag;
using NSwag.Generation.Processors.Security;
using WindMill.Util;

var builder = WebApplication.CreateBuilder(args);
var env = ConfigurationHelper.ConfigureEnvironment(builder);
builder.Services.AddSingleton(env);

var db = env["DB_CONNECTION_STRING"];
builder.Services.AddMqttControllers();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "WindMill API";
    document.Description = "API for controlling and monitoring wind turbines.";
    document.Version = "v1";
    document.AddSecurity(name: "JWT", swaggerSecurityScheme: new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    document.OperationProcessors.Add(item: new AspNetCoreOperationSecurityScopeProcessor(name: "JWT"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(env["SECRET"]))
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<WindMill.DataAccess.MyDbContext>(options => options.UseNpgsql(db));
builder.Services.AddScoped<SaveData>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<DataSeeder>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseOpenApi();
app.UseSwaggerUi();

var mqtt = app.Services.GetRequiredService<IMqttClientService>();
await mqtt.ConnectAsync("broker.hivemq.com", 1883);

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.Initialize();
}

await app.RunAsync();


