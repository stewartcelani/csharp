using UnderstandingTheNeedForGenerics;

/*
 * UNDERSTANDING THE NEED FOR GENERICS:
 * Implement a Stack class that works with double values
 */
var doubleStack = new SimpleStackDouble();
doubleStack.Push(1.2);
doubleStack.Push(2.8);
doubleStack.Push(3.0);

while (doubleStack.Count > 0)
{
    var last = doubleStack.Pop();
    Console.WriteLine(last);
}

/*
 * New requirement: Make the stack class also work with strings
 * Approach 1: Object approach
 * - Not type-safe, it will accept ANYTHING which will cause issues/exceptions when using the stack
 * - Value types will need to be boxed and unboxed (performance impact with a lot of data)
 */
var objectStack = new SimpleStackObject();
objectStack.Push("WiredBrainCoffee");
objectStack.Push(2022.6);
objectStack.Push("Pluralsight");

while (objectStack.Count > 0)
{
    var last = objectStack.Pop();
    Console.WriteLine(last);
}

/*
 * Approach 2: Copy-and-paste approach
 * - Type safety and performance is back
 * - However, it is not DRY, a lot of manual repetition
 * - Improving one implementation means you should manually update all copypasta'd derivatives
 */
var stringStack = new SimpleStackString();
stringStack.Push("WiredBrainCoffee");
stringStack.Push("Pluralsight");

while (stringStack.Count > 0)
{
    var last = stringStack.Pop();
    Console.WriteLine(last);
}

/*
 * Approach 3: Implement a generic Simple Stack class
 */
var genericStack = new SimpleStack<string>();
genericStack.Push("WiredBrainCoffee");
genericStack.Push("Pluralsight");

while (genericStack.Count > 0)
{
    var last = genericStack.Pop();
    Console.WriteLine(last);
}

Console.ReadLine();
