using Patterns.Class.FluentApiBuilder;

/*
 * Simple builder
 * You would have to implement error checking in the build stage or use
 * default values
 */
User user = new UserBuilder()
    .WithName("John Doe")
    .WithAge(22)
    .WithEmail("john.doe@contoso.com")
    .Build();

Console.WriteLine(user.Name); // John Doe
Console.WriteLine(user.Age); // 22
Console.WriteLine(user.Email); // john.doe@contoso.com