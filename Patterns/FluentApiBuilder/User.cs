namespace FluentApiBuilder;

public class User
{
    public User(string name, string email, int age)
    {
        Name = name;
        Email = email;
        Age = age;
    }

    public string Name { get; }
    public string Email { get; }
    public int Age { get; }
}