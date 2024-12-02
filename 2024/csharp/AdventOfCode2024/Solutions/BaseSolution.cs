namespace AdventOfCode2024.Solutions;

public abstract class BaseSolution
{
    protected readonly List<string> Lines;

    protected BaseSolution()
    {
        Lines = File.ReadAllLines($"Input/day{Day()}.txt").ToList();
    }

    public abstract int Day();
}