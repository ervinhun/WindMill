namespace WindMill.Util;

public static class RequiredEnvVariables
{
    public static string[] GetRequiredEnvVars()
    {
        return
        [
            "FARM_ID", "DB_CONNECTION_STRING", "SECRET", "SUPER_USER_NAME", "SUPER_PASSWORD", "ENGINEER_USER_NAME",
            "ENGINEER_PASSWORD"
        ];
    }
}