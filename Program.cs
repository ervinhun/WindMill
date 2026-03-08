using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mqtt.Controllers;
using WindMill.Util;

var builder = WebApplication.CreateBuilder(args);
var env = ConfigurationHelper.ConfigureEnvironment(builder);
builder.Services.AddSingleton(env);

var db = env["DB_CONNECTION_STRING"];
builder.Services.AddMqttControllers();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(env["SECRET"]))
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<WindMill.DataAccess.MyDbContext>(options => options.UseNpgsql(db));
var app = builder.Build();

app.MapControllers();
app.UseOpenApi();
app.UseSwaggerUi();

var mqtt = app.Services.GetRequiredService<IMqttClientService>();
await mqtt.ConnectAsync("broker.hivemq.com", 1883);


await app.RunAsync();
