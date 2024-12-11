namespace AdventOfCode2024.Solutions;

public class Day10 : BaseSolution, ISolution
{
    public override int Day() => 10;

    private record Node(int X, int Y, int Height);
    
    private IEnumerable<Node> Neighbours(Node node, IEnumerable<Node> graph) => graph.Where(x => 
        x.X == node.X && x.Y == node.Y - 1 || 
        x.X == node.X && x.Y == node.Y + 1 || 
        x.X == node.X - 1 && x.Y == node.Y ||
        x.X == node.X + 1 && x.Y == node.Y);

    private List<List<Node>> GetPaths(List<Node> currentPath, 
        List<Node> graph, 
        List<List<Node>> allPaths
        )
    {
        var head = currentPath.Last();
        var validNeighbours = Neighbours(head, graph).Where(x => x.Height == head.Height + 1).ToList();
        var newPaths = validNeighbours.Select(x => currentPath.Select(y => y).Concat([x]).ToList()).ToList();

        allPaths.AddRange(newPaths.Where(x => !allPaths.Contains(x)).ToList());
        
        return validNeighbours.Count != 0 ? 
            newPaths.SelectMany(newPath => GetPaths(newPath, graph, allPaths)).ToList() : 
            allPaths;
    } 

    public object Part1()
    {
        var nodes = Lines
            .SelectMany((l, y) => l.Select((c, x) => new Node(x, y, c - 48)))
            .ToList();

        return nodes.Where(x => x.Height == 0)
            .Select(startNode => GetPaths([startNode], nodes, [])
                .Select(x => x.Last())
                .Distinct()
                .Where(x => x.Height == 9)
                .ToList())
            .Select(paths => paths.Count)
            .Sum();
    }

    public object Part2()
    {
        
        var nodes = Lines
            .SelectMany((l, y) => l.Select((c, x) => new Node(x, y, c - 48)))
            .ToList();

        return nodes.Where(x => x.Height == 0)
            .Select(startNode => GetPaths([startNode], nodes, [])
                .Where(x => x.Last().Height == 9)
                .Distinct()
                .ToList())
            .Select(paths => paths.Count)
            .Sum();
    }
}