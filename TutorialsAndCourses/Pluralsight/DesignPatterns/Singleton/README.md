# Singleton Pattern
The intent of the singleton pattern is to ensure that a class only has one instance, and to provide a global point of access to it.

- One instance is preferred to avoid unintended consequences
- Holding the class instance in a global variable doesn't prevent clients from creating other instances of the class
- Make the class responsible for ensuring only one instance of itself exists
- Make the constructor private or protected so it cannot be instantiated itelf
- Prefer Lazy instantiation; create and store the instance when it's requested for the first time, and return that instance on subsequent requests

Use cases:
- When there must be exactly one instance of a class, and it must be accessible to clients from a well-known access point
- When the sole instance should be extensible by subclassing, and clients should be able to use an extended instance without modifying their code
- Common use case is a Logger

Pattern consequences:
- Violates the single responsibility principle (SRP)
- Strict control over how and when clients access it
- Avoids polluting the namespace with global variables
- Subclassing allows configuring the application with an instance of the class you need at runtime
- Multiple instances can be allowed without having to alter the client

In modern applications:
- In practice Singletons are the responsibility of DI (Dependency Injection)/IoC (Inversion of Control) containers that are declared at runtime.
- Examples are Microsoft's inbuilt service collection, Autofac.

Related patterns that can be implemented as a singleton:
- Abstract Factory 
- Builder
- Prototype 
