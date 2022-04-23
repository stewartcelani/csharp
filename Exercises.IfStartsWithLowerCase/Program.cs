/*
 * If starts with lower case
 * Given a string, write a method that checks if each word in the string starts with lower case and if so,
 * removes this letter from the string.
 * 
 * IfStartsWithLowerCase("Alfa Beta gamma") → "Alfa Beta amma"
 */

using Exercises.IfStartsWithLowerCase;

string result = Library.IfStartsWithLowerCase("Alfa Beta gamma");
Console.WriteLine(result); // Alfa Beta amma

namespace Exercises.IfStartsWithLowerCase
{
    public static class Library
    {
        public static string IfStartsWithLowerCase(string input)
        {
            string[] split = input.Split(" ");

            for (var i = 0; i < split.Length; i++)
            {
                if (char.IsLower(split[i][0]))
                {
                    split[i] = split[i][1..];
                }
            }

            return string.Join(" ", split);
        }
    }
}