namespace Patterns.FluentApiBuilder;

public class UserBuilder
{
    private string _name;
    private string _email;
    private int _age;

    public UserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithAge(int age)
    {
        _age = age;
        return this;
    }
    
    public User Build()
    {
        // A proper implementation would implement error checking here
        return new User(_name, _email, _age);
    }
}