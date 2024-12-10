using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

public partial class Day7 : BaseSolution, ISolution
{
    private enum OperationType
    {
        Add, 
        Multiply,
        Concatenate
    }

    private IEnumerable<IEnumerable<OperationType>> GenerateCombinations(int length, bool includeConcatenation = false)
    {
        var combination = new OperationType[length];
        var possibleValues = includeConcatenation ? 3 : 2;
        var combinations = (int)Math.Pow(possibleValues, length);
        for (var i = 0; i < combinations; i++)
        {
            for (var j = 0; j < length; j++)
            {
                combination[j] = (OperationType)(i / (int)Math.Pow(possibleValues, j) % possibleValues);
            } 
            yield return combination.ToArray();
        }
    }

    private long TotalMatching(bool includeConcatenation = false)
        => Lines
            .Select(l => ResultWithOperands()
                .Matches(l)
                .SelectMany(match => match.Captures.Select(capture => long.Parse(capture.ValueSpan))).ToList())
            .Select(numbers => new Operation(numbers.First(), numbers.Skip(1).ToList()))
            .Where(operation =>
            {
                var openSlots = operation.Operands.Count - 1;
                return GenerateCombinations(openSlots, includeConcatenation).Any(c =>
                {
                    var total = operation.Operands.First();
                    var others = operation.Operands.Skip(1).ToList();
                    var config = c.ToList();
                    for (var i = 0; i < config.Count; i++)
                    {
                        total = config[i] switch
                        {
                            OperationType.Multiply => total * others[i],
                            OperationType.Add => total + others[i],
                            OperationType.Concatenate => long.Parse($"{total}{others[i]}"),
                            _ => throw new NotImplementedException()
                        };
                    }
                    return total == operation.Target;
                });
            })
            .Sum(x => x.Target);

    private record Operation(long Target, List<long> Operands);

    [GeneratedRegex(@"(\d+)+")]
    private static partial Regex ResultWithOperands();
    
    public override int Day() => 7;

    public object Part1() => TotalMatching();

    public object Part2() => TotalMatching(true);
}