namespace WindMill.Util;

public static class ConfigurationHelper
{
    public static Dictionary<string, string> ConfigureEnvironment(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            DotNetEnv.Env.Load();
        }
   
        builder.Configuration.AddEnvironmentVariables();
        Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
   
        var config = new Dictionary<string, string>();
   
        //Check if the required env is available
        foreach (var envVar in RequiredEnvVariables.GetRequiredEnvVars())
        {
            var value = builder.Configuration[envVar];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"{envVar} not set in environment");
            }
            config[envVar] = value;
        }
   
        return config;
    }
}