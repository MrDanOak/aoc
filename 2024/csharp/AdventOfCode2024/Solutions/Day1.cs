namespace AdventOfCode2024.Solutions;

public class Day1 : ISolution
{
    private readonly List<int> _first = [];
    private readonly List<int> _second = [];

    public Day1()
    {
        var parsed = File.ReadAllLines($"input/day{Day()}.txt")
            .Select(l => l.Split(" ")
            .Where(x => string.IsNullOrWhiteSpace(x) is false)
            .Select(int.Parse)
            .ToList());
        
        foreach (var numbers in parsed)
        {
            _first.Add(numbers.First());
            _second.Add(numbers.Last());
        }

        _first.Sort();
        _second.Sort();
    }
    
    public object Part1() => _first.Select((t, i) => Math.Abs(t - _second[i])).Sum();

    public object Part2() => _first.Select(x => x * _second.Count(y => y == x)).Sum();

    public int Day() => 1;
}