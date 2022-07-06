using Iterator;

PeopleCollection people = new();

people.Add(new Person("Kevin Dockx", "Belgium"));
people.Add(new Person("Gill Cleeren", "Belgium"));
people.Add(new Person("Roland Guijt", "The Netherlands"));
people.Add(new Person("Thomas Claudius Huber", "Germany"));

var peopleIterator = people.CreateIterator();

for (var person = peopleIterator.First(); !peopleIterator.IsDone; person = peopleIterator.Next())
{
    Console.WriteLine(person?.Name);
}

Console.ReadKey();