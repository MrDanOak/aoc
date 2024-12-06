using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

public partial class Day5 : BaseSolution, ISolution
{
    private record OrderingRule(int Before, int After);

    private record Row(List<int> Data, bool Reordered);
    
    public override int Day() => 5;

    private List<Row> GetRows()
    {
        var rules = Lines
            .Where(x => OrderingRulesRegex().IsMatch(x))
            .Select(x => OrderingRulesRegex().Matches(x))
            .Where(x => x.First().Groups.Count >= 2)
            .Select(match =>
            {
                var before = int.Parse(match.First().Groups[1].Value);
                var after = int.Parse(match.Last().Groups[2].Value);
                return new OrderingRule(before, after);
            })
            .ToList();

        var printingRows = Lines.Where(l => l.Contains(','))
            .Select(l => l.Split(',').Select(int.Parse).ToList()).ToList();

        var rows = new List<Row>();
        foreach (var row in printingRows)
        {
            var wasReordered = false;
            var reorderedRow = row;
            var applicableRules = rules
                .Where(r => row.Contains(r.Before) && row.Contains(r.After))
                .ToList();
            
            foreach (var secondPass in applicableRules)
            {
                foreach (var rule in applicableRules)
                {
                    var before = reorderedRow.Select((x, i) => new { x, i }).Where(x => x.x == rule.Before).ToList();
                    var firstAfter = reorderedRow.Select((x, i) => new { x, i }).First(x => x.x == rule.After);

                    if (!before.Any(x => x.i > firstAfter.i))
                        continue;

                    wasReordered = true;
                    reorderedRow.RemoveAll(x => x == before.First().x);
                    reorderedRow.InsertRange(firstAfter.i, before.Select(x => x.x));
                }
            }

            rows.Add(new Row(reorderedRow, wasReordered));
        }

        return rows;
    }
    
    public object Part1() 
        => GetRows()
            .Where(x => x.Reordered == false)
            .Select(x => x.Data[x.Data.Count / 2])
            .Aggregate((a, b) => a + b);

    public object Part2()
    {
        var rows = GetRows()
            .Where(x => x.Reordered);
        
        return rows.Select(x => x.Data[x.Data.Count / 2])
            .Aggregate((a, b) => a + b);
    }

    [GeneratedRegex(@"(\d+)\|(\d+)")]
    private static partial Regex OrderingRulesRegex();
}