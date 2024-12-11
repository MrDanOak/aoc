using System.Collections.Concurrent;

namespace AdventOfCode2024.Solutions;

[RunSolution]
public class Day11 : BaseSolution, ISolution
{
    public override int Day() => 11;

    private long Stones(int blinks)
    {
        var stones = Lines
            .First()
            .Split(' ')
            .Select(long.Parse)
            .ToList();
        
        for (var i = 0; i < blinks; i++)
        {
            Console.WriteLine($"Blink {i}");
            var newStones = new List<long>();
            for (var j = 0; j < stones.Count; j++)
            {
                newStones.AddRange(ProcessStone(stones[j]));
            }

            stones = newStones;
        }
        Console.WriteLine($"Blinks {blinks} stones {stones.Count}");
        return stones.Count;
    }

    static IEnumerable<long> ProcessStone(long stone)
    {
        if (stone == 0)
        {
            return [1];
        }

        var asString = stone.ToString();
        if (asString.Length % 2 != 0) 
            return [stone * 2024];
        
        var halfLength = asString.Length / 2;
        return [long.Parse(asString[..halfLength]), long.Parse(asString[halfLength..])];
    }

    public object Part1() => Stones(25);

    public object Part2() => Stones(75);
}