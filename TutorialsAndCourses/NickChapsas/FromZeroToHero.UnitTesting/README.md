﻿# From Zero to Hero: Unit testing for C# Developers
My repo & notes for https://nickchapsas.com/p/from-zero-to-hero-unit-testing-in-c

---

Tests are code that validates the behaviour of other code.

- Unit Testing
  - Tests what is called the "Unit". Usually a method or class.
  - In unit testing any I/O call (to file system, database etc) is mocked so that unit tests can remain fast and don't depend on anything.
  - It is not uncommon to try go wider with your unit tests while still keeping them in unit test scope -- sometimes called a 'component' and called component testing or 'functional testing' or 'narrow integration testing'.
  - Examples:
    - Checking that a method that splits a full name into a separate first and last name correctly converts 'Stewart Celani' into 'Stewart' and 'Celani'.
- Integration Testing
  - Testing the things that your system integrates with/depends on (databases, API, file system).
  - Actually calls dependencies.
    - Tests rely on an actual working environment or one that is spun up on demand (e.g. in docker or in memory).
  - For testing larger parts of a system.
  - Slower than unit testing.
- End to End Testing (E2E)
  - Test from beginning to end all parts of a system.
  - Slower, more business logic focused.
  - The idea is that you're validating the behaviour that the user calling your system would have
- Performance Testing / Load Testing
  - Validate the performance of an application

### Why is unit testing important?
The feedback loop of writing code and having very quick tests running against all changed code and getting immediate feedback is very powerful and stops most bugs from even getting close to production. 

Unit tests and code coverage give developers confidence to refactor code and be sure that they haven't accidentally introduced bugs/regressions into the system.

Some companies/people think testing is so important that they practice what is called 'Test Driven Development' (TDD) where the tests are written before writing actual code.

Unit tests are very important and are usually part of the CI/CD build pipeline. If any tests fail the whole build fails.

Many big companies have mandatory code coverage requirements before being able to commit code.

### The testing pyramid
The testing pyramid is a way of describing the types and amounts of each type of test you should aim for.
![TestingPyramid.png](TestingPyramid.png)

### The 3 core unit testing concepts
1. Testing Library
   - **xUnit** 
     - The most popuplar unit testing library. Created by the creators of nUnit. Used by Microsoft themselves.
   - nUnit
   - MSTest
2. Mocking Library
   - **NSubstitute**
   - Moq
   - FakeItEasy
3. Assertion Library
   - **Fluent Assertions**
   - Built-in Assert 
   - Shouldly

This course will use xUnit, NSubstitute and Fluent Assertions.


### Project structure & naming conventions
- Project structure should have src and test folder also represented by solution folders
![](ProjectStructure.png)
- Test project name should be: NameOfProject.Tests.{TypeOfTest} (e.g. CalculatorLibrary.Tests.Unit, CalculatorLibrary.Tests.Integration)
- Class name should be the name of the class being tested with 'Tests', if the 'Calculator.cs' class was being tested the test file name should be 'CalculatorTests.cs'
- Test methods names should start with the name of the method they are testing then follow the 'Should... When' approach (e.g. Add_**Should**AddTwoNumbers_**When**TwoNumbersAreIntegers)
- This is how Nick sees unit tests being named in the real world at big companies in the UK
- Sometimes people will put an underscore after each word but that is personal preference

### Arrange, Act, Asset
- Tests are usually arranged into the AAA format, its good to set up an aaa code snippet in your IDE of choice.
![](ArrangeActAssert.png)

### xUnits Test Execution Model
- The thing being tested/'acted' upon is called the 'system under test' or 'sut'
![](SystemUnderTest.png)
- A new instance of the test class is created on execution of each test
- It ensures every tests scope is self contained and one execution of a unit test wont have side effects on the execution of another

### Test Setup and Teardown
- Any setup you need should be done via the constructor, xUnit will automatically inject dependencies using DI
- Cleanup should be done via implementing the IDisposable interface
![](TestSetupAndTeardown.png)
- What if you have async code in your setup or teardown? (especially useful for Integration tests)
  - Implement the IAsyncLifetime interface which required implementing InitializeAsync and DisposeAsync methods
![](TestSetupAndTeardownAsync.png)