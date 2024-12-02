
using System.Reflection;
using AdventOfCode2024.Solutions;

var solutions = Assembly 
    .GetExecutingAssembly() 
    .GetTypes() 
    .Where(x => typeof(ISolution).IsAssignableFrom(x) && !x.IsAbstract && x.GetConstructor(Type.EmptyTypes) != null) 
    .Select(x => (ISolution)Activator.CreateInstance(x)) 
    .ToList();

foreach (var s in solutions)
{
    Console.WriteLine($"Day {((BaseSolution)s!).Day()} Part 1: {s.Part1()} Part 2: {s.Part2()}");
}