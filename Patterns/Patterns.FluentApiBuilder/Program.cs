using Patterns.FluentApiBuilder;

/*
 * Simple builder
 * Potential for user to not enter all required fields
 */
User user = new UserBuilder()
    .WithName("John Doe")
    .WithAge(22)
    .WithEmail("john.doe@contoso.com")
    .Build();

Console.WriteLine(user.Name); // John Doe
Console.WriteLine(user.Age); // 22
Console.WriteLine(user.Email); // john.doe@contoso.com
Console.WriteLine();

/*
 * Guided builder
 * Force the builder down a specific path that covers all required fields.
 * Only when all required fields are done is the CreateApiClient method that returns a new ApiClient visible.
 * Also allows forking behaviour if needed.
 */
ApiClient apiClient1 = TwoLeggedApiClient
    .Configure()
    .WithClientId("AFO4tyzt71HCkL73cn2tAUSRS0OSGaRY")
    .AndClientSecret("wE3GFhuIsGJEi3d4")
    .ForAccount("f33e018a-d1f5-4ef3-ae67-606de6aeed87")
    .CreateApiClient();

Console.WriteLine(apiClient1.ClientId); // AFO4tyzt71HCkL73cn2tAUSRS0OSGaRY
Console.WriteLine(apiClient1.Configuration.ApiClientName); // Default
Console.WriteLine(apiClient1.Configuration.MaxRetries); // 4
Console.WriteLine(apiClient1.Configuration.SecondsBetweenRetries); // 15
Console.WriteLine(apiClient1.Configuration.DegreesOfParallelism); // 4
apiClient1.Example(); // ""
Console.WriteLine();

/*
 * This example shows a way to put optional configuration into a lambda that gets mapped onto a configuration class.
 * Any fields not explicitly configured in the lambda will get assigned their default values still.
 */
ApiClient apiClient2 = TwoLeggedApiClient
    .Configure()
    .WithClientId("AFO4tyzt71HCkL73cn2tAUSRS0OSGaRY")
    .AndClientSecret("wE3GFhuIsGJEi3d4")
    .ForAccount("f33e018a-d1f5-4ef3-ae67-606de6aeed87")
    .WithOptions(options =>
    {
        options.ApiClientName = "Application Name";
        options.MaxRetries = 12;
        options.SecondsBetweenRetries = 5;
        options.LoggingMethod = (s) => Console.WriteLine(s);
    })
    .CreateApiClient();

Console.WriteLine(apiClient2.ClientId); // AFO4tyzt71HCkL73cn2tAUSRS0OSGaRY
Console.WriteLine(apiClient2.Configuration.ApiClientName); // Application Name
Console.WriteLine(apiClient2.Configuration.MaxRetries); // 12
Console.WriteLine(apiClient2.Configuration.SecondsBetweenRetries); // 5
Console.WriteLine(apiClient1.Configuration.DegreesOfParallelism); // 4
apiClient2.Example(); // "Example of custom logging passed via lambda config."