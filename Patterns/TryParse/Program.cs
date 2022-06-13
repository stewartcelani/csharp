using TryParse;

const string doesNotExist = "doesNotExist";
if (!SecretsManager.TryParseEnvironmentVariable(doesNotExist, out var doesNotExistResult))
{
    Console.WriteLine($"Unable to parse environment variable: '{doesNotExist}'");
}

Console.WriteLine(string.IsNullOrEmpty(doesNotExistResult) + "\n"); // True

const string doesExistOnWindows = "PATH";
if (SecretsManager.TryParseEnvironmentVariable(doesExistOnWindows, out var doesExistOnWindowsResult))
{
    Console.WriteLine($"{doesExistOnWindows}: {doesExistOnWindowsResult}");
}

Console.ReadLine();