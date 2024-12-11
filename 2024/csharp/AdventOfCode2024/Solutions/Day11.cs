namespace AdventOfCode2024.Solutions;
[RunSolution]
public class Day11 : BaseSolution, ISolution
{
    public override int Day() => 11;

    private static Dictionary<(long, int, int), long> _cache = new();

    private static long _total = 0;

    private long Stones(int blinks)
    {
        var total = Lines
            .First()
            .Split(' ')
            .Select(long.Parse)
            .ToList()
            .Sum(stone => RecursiveScore(stone, 0, blinks));

        Console.WriteLine($"Blinks {blinks} stones {total}");
        return total;
    }

    static long RecursiveScore(long stone, int blink, int targetBlink, long currentTotal = 0)
    {
        if (blink == targetBlink)
        {
            currentTotal += 1;
            return currentTotal;
        }

        if (_cache.TryGetValue((stone, blink, targetBlink), out var cachedValue))
            return cachedValue;

        if (stone == 0) {
            currentTotal += RecursiveScore(1, blink + 1, targetBlink, currentTotal);
            _cache.TryAdd((stone, blink, targetBlink), currentTotal);
            return currentTotal;
        }

        var asString = stone.ToString();
        if (asString.Length % 2 != 0) 
        {
            currentTotal += RecursiveScore(stone * 2024, blink + 1, targetBlink, currentTotal);
            _cache.TryAdd((stone, blink, targetBlink), currentTotal);
            return currentTotal;
        }

        var halfLength = asString.Length / 2;
        currentTotal += RecursiveScore(long.Parse(asString[..halfLength]), blink + 1, targetBlink, currentTotal) +
            RecursiveScore(long.Parse(asString[halfLength..]), blink + 1, targetBlink, currentTotal);

        _cache.TryAdd((stone, blink, targetBlink), currentTotal);
        return currentTotal;
    }

    public object Part1() => Stones(25);

    public object Part2() => Stones(75);
}