/*
 * Short 1h course mainly involving demonstrations with DTOs in a Blazor frontend + ASPNET Core 6 API backend
 * Records are Immutable Reference Types
 * - Objects that shouldn't change shouldn't be changeable
 */

var course = new Course("Working with C# Records", "Roland Guijt");
Console.WriteLine(course.Name);
// course.Author = "Test"; // Wont compile -- can only assign values on initialization. Record props are { get; init; } 
var course2 = new Course("Working with C# Records", "Roland Guijt");
var areEqual = course == course2;
Console.WriteLine(areEqual); // True

/*
 * When you need to change properties on an existing record (returns new object, doesn't mutate original)
 */
var conf = new Conference
{
    Name = "BuzzWord.IO",
    Location = "Silicon Valley"
};
var rebrand = conf with { Name = "Buzz.JS.IO" };

/*
 * Inheritance
 */
Course course3 = new TimedCourse("Learning Buzz.JS", "Facebook", 420);
// course3.Duration; // Cant access duration as it is a TimedCourse property on a Course variable
var course3Cast = (TimedCourse)course3;
Console.WriteLine(course3Cast.Duration); // 420 -- however when casted back into its declared type the property is still there

Console.ReadLine();


public record Course(string Name, string Author);

public record TimedCourse(string Name, string Author, int Duration) : Course(Name, Author);

public record Conference
{
    public string Name { get; set; }
    public string Location { get; set; }
}

