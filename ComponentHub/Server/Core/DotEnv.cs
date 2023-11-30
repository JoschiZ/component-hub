namespace ComponentHub.Server.Helper;

internal static class DotEnv
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }

    public static WebApplicationBuilder AddEnvToConfig(this WebApplicationBuilder builder)
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, ".env");
        Load(dotenv);
        builder.Configuration.AddEnvironmentVariables();
        return builder;
    }
}