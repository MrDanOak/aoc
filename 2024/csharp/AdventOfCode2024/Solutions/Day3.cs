using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;

public partial class Day3 : BaseSolution, ISolution
{
    public override int Day() => 3;

    enum InstructionType
    {
        Mul,
        Do,
        Dont
    }

    private record Instruction(InstructionType Type, params int[] Operands);

    private Instruction AsMultInstructions(Match match)
    {
        var first = int.Parse(match.Groups[1].Value);
        var last = int.Parse(match.Groups[2].Value);
        return new Instruction(InstructionType.Mul, first, last);
    }

    private Instruction AsInstruction(Match match)
        => match.Groups[0].Value switch
        {
            var x when x.StartsWith("mul") => AsMultInstructions(match),
            var x when x.StartsWith("don't") => new Instruction(InstructionType.Dont),
            _ => new Instruction(InstructionType.Do),
        };

    public object Part1()
    {
        return Lines.Select(x => MulOnlyRegex().Matches(x))
            .SelectMany(matchCollection => matchCollection.Select(AsMultInstructions))
            .Select(x => x.Operands.Aggregate((a, b) => a * b))
            .Sum();
    }

    public object Part2()
    {
        var instructions = Lines.Select(x => InstructionRegex().Matches(x))
            .SelectMany(matchCollection => matchCollection.Select(AsInstruction));
        
        var @do = true;
        var aggregate = 0;
        foreach (var instruction in instructions)
        {
            @do = instruction.Type switch
            {
                InstructionType.Dont => false,
                InstructionType.Do => true,
                _ => @do
            };
            if (instruction.Type == InstructionType.Mul && @do)
            {
                aggregate += instruction.Operands.Aggregate((a, b) => a * b);
            }
        }

        return aggregate;
    }
    
    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulOnlyRegex();
    
    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)")]
    private static partial Regex InstructionRegex();
    
}