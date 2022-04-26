namespace Patterns.Class.FluentApiBuilder;

public class User {
    public string Name { get; }
    public string Email { get; }
    public int Age { get; }

    public User(string name, string email, int age)
    {
        Name = name;
        Email = email;
        Age = age;
    }
}