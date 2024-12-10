namespace AdventOfCode2024.Solutions;
[RunSolution]
internal class Day9 : BaseSolution, ISolution
{
    public override int Day() => 9;

    public object Part1()
    {
        var disk = Lines.First();
        var blocks = disk.Chunk(2)
            .SelectMany((x, i) =>
                Enumerable
                .Range(0, x[0] - 48)
                .Select(_ => (long?)i)
                .Concat(
                    Enumerable
                    .Range(0, x.Length > 1 ? x[1] - 48 : 0)
                    .Select(_ => (long?)null)
                )
            ).ToList();

        var sortedBlocks = blocks.Select(x => x).ToList();
        while (sortedBlocks.Any(x => x is null))
        {
            var firstNull = sortedBlocks.IndexOf(null);
            var lastNotNull = sortedBlocks.FindLastIndex(x => x.HasValue);
            if (firstNull <= 0 || lastNotNull <= 0) 
                continue;

            sortedBlocks[firstNull] = sortedBlocks[lastNotNull];
            sortedBlocks.RemoveAt(lastNotNull);
        }

        var result = sortedBlocks.Select((x, i) =>
            new
            {
                x.Value,
                Index = i
            })
            .Aggregate((long)0, (acc, block) => acc + block.Value * block.Index);

        return result;
    }

    public object Part2()
    {
        var disk = Lines.First();
        var blocks = disk.Chunk(2)
            .SelectMany((x, i) =>
                Enumerable
                    .Range(0, x[0] - 48)
                    .Select(_ => (long?)i)
                    .Concat(
                        Enumerable
                            .Range(0, x.Length > 1 ? x[1] - 48 : 0)
                            .Select(_ => (long?)null)
                    )
            ).ToList();

        var sortedBlocks = blocks.Select(x => x).ToList();
        for (var i = blocks.Where(x => x is not null).Max(); i >= 0; i--)
        {
            var toBeMoved = GetContiguous(i, sortedBlocks).First().ToList();
            var moveLocation = GetContiguous(null, sortedBlocks)
                .FirstOrDefault(emptySpace => 
                    emptySpace.Max(y => y.Item2) < toBeMoved.First().Item2 && emptySpace.Count() >= toBeMoved.Count)?.ToList();
            
            if (moveLocation is null) 
                continue;

            for (var j = 0; j < toBeMoved.Count; j++)
            {
                sortedBlocks[moveLocation[j].Item2] = toBeMoved[j].Item1;
                sortedBlocks[toBeMoved[j].Item2] = null;
            }
        }

        var result = sortedBlocks.Select((x, i) =>
                new
                {
                    Value = x ?? 0,
                    Index = i
                })
            .Aggregate((long)0, (acc, block) => acc + block.Value * block.Index);

        return result;

        IEnumerable<IEnumerable<(long?, int)>> GetContiguous(long? value, List<long?> list)
        {
            // this method returns a collection of a collection of blocks that are contiguous with the given value that could be null or a number
            var contiguous = new List<(long?, int)>();
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    contiguous.Add((value, i));
                    if (i == list.Count - 1)
                    {
                       yield return contiguous;
                    }
                }
                else if (contiguous.Any())
                {
                    yield return contiguous;
                    contiguous.Clear();
                }
            }
        }
    }
}
