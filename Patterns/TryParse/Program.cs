public static class Program
{
    /*
     * Example of the Try-Parse pattern:
     * https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions-and-performance#try-parse-pattern
     */
    public static void Main(string[] args)
    {
        const string doesNotExist = "doesNotExist";
        if (!SecretsManager.TryParseEnvironmentVariable(doesNotExist, out var doesNotExistResult))
        {
            Console.WriteLine($"Unable to parse environment variable: '{doesNotExist}'");
        }
        else
        {
            // Execution wont reach here
            Console.WriteLine($"{doesNotExist}: {doesNotExistResult}");
        }

        Console.WriteLine();

        const string doesExistOnWindows = "PATH";
        if (SecretsManager.TryParseEnvironmentVariable(doesExistOnWindows, out var doesExistOnWindowsResult))
        {
            Console.WriteLine($"{doesExistOnWindows}: {doesExistOnWindowsResult}");
        }

        Console.ReadLine();
    }

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
}