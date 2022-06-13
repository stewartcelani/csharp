namespace TryParse;

public static class SecretsManager
{
    public static bool TryParseEnvironmentVariable(string name, out string? result)
    {
        try
        {
            result = GetEnvironmentVariable(name);
            return !string.IsNullOrEmpty(result);
        } catch (InvalidOperationException)
        {
            result = null;
            return false;
        }
    }

    public static string GetEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name) ??
               throw new InvalidOperationException($"Environment variable '{name}' does not exist");
    }
}