### [SOLID Principles for C# Developers](https://app.pluralsight.com/library/courses/csharp-solid-principles/table-of-contents) by Steve Smith

It's easy to write software that fulfills its users' immediate needs, but is difficult to extend and maintain. Such software becomes a burden for companies striving to remain competitive. In this course, SOLID Principles for C# Developers, you will learn five fundamental principles of object-oriented design that will keep your software loosely coupled, testable, and maintainable. First, you will see how to keep classes small and focused, and how to extend their behavior without having to edit their source code. Then, you will discover the importance of properly designing interfaces and abstractions in your systems. Finally, you will explore how to arrange dependencies in your system so different implementations can be added or plugged in as needed, allowing a truly modular design. When you are finished with this course, you will understand how to build maintainable, extensible, and testable applications using C# and .NET.

1. **SRP - Single Responsibility Principle**
   - Each software module should have one *and only one* reason to change
   - Classes should encapsulate doing a particular task in a particular way
   - Multipurpose tools don't perform as well as dedicated tools
   - Dedicated tools are easier to use
   - A problem with one part of a multipurpose tool can impact all parts
   - Examples of a 'responsibility':
     - Persistence
       - Might need to change from files to database or from one database to another
     - Logging
       - May prove insufficient and a new framework or provider may need to be added
     - Validation
       - Validation rules or the way validation is performed may need to be updated in the future
     - Business Logic
   - Responsibilities change at different times for different reasons. Each one is an axis of change.
   - Modules should be as loosely coupled as possible
   - Separation of Concerns; keep plumbing code separate from high level business logic
   - Class elements that belong together are **cohesive**. Classes that have many-responsibilities will tend to have less cohesion than a class with a single responsibility.
   - Relationships within each class represent cohesion, while relationships between classes are represent coupling. In many cases, loose coupling is preferred as it represents code that is easier to change and test.
   

2. **OCP - Open/Closed Principle**
3. **LSP - Liskov Substitution Principle**
4. **ISP - Interface Segregation Principle**
5. **DIP - Dependency Inversion Principle**

After watching this course you might be tempted to use these principles everywhere, for everything.

Instead, to really learn the principles you should practice PDD.

#### Pain Driven Development (PDD)

- Develop using the simplest methods you know
- Avoid premature optimization
- If the current design is painful to work with, use the principles to guide redesign