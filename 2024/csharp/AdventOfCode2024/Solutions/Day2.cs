namespace AdventOfCode2024.Solutions;

public class Day2 : BaseSolution, ISolution
{
    private List<List<int>> _reports;
    private bool SafeDistance(int a, int b)
        => Math.Abs(a - b) switch
        {
            >= 1 and <= 3 => true,
            _ => false
        };
    
    private bool SafeReport(List<int> report)
    {
        var increasing = report[0] < report[1];
        for (var i = 1; i < report.Count; i++)
        {
            var nextIncreasing = report[i - 1] < report[i];
            var safeDistance = SafeDistance(report[i], report[i - 1]);
            
            if (safeDistance && ((increasing && nextIncreasing) || (!increasing && !nextIncreasing)))
                continue;

            return false;
        }

        return true;
    }

    public Day2()
    {
        _reports = Lines.Select(l => l.Split(" ")
                .Where(x => string.IsNullOrWhiteSpace(x) is false)
                .Select(int.Parse)
                .ToList()).ToList();
    }
    
    public object Part1()
    {
        return _reports.Count(SafeReport);
    }

    public object Part2()
    {
        var unsafeReports = _reports.Where(x => SafeReport(x) is false);
        var madeSafe = unsafeReports.Count(report =>
            report
                .Select((_, i) => (List<int>)report.Select((x, y) => new { Value = x, Index = y })
                    .Where(x => x.Index != i)
                    .Select(x => x.Value)
                    .ToList())
                .Any(SafeReport)
        );

        return madeSafe + (int)Part1();
    }

    public override int Day() => 2;
}