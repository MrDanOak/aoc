namespace AdventOfCode2024.Solutions;

[RunSolution]
public class Day6 : BaseSolution, ISolution
{
    public override int Day() => 6;

    private enum NodeType
    {
        Empty,
        Obstacle
    }

    private enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }

    private class Visit(Direction direction)
    {
        public Direction Direction = direction;
    }

    private record Graph(List<Node> Nodes, bool InfiniteLoop = false);

    private class Node(int x, int y, NodeType type, Visit? visit = null)
    {
        public readonly int X = x;
        public readonly int Y = y;
        public NodeType Type = type;
        public Visit? Visit = visit;

        public Node Clone() => new(X, Y, Type, Visit);
    }

    private record Guard(int X, int Y, Direction Direction);

    private readonly List<Node> _graph;

    private Guard _guard;

    public Day6()
    {
        var nodes = Lines
            .SelectMany((l, y) => l.Select((c, x) => new {x, y, c}))
            .ToList();
        
        _graph = nodes.Select(x => x.c switch
        {
            '#' => new Node(x.x, x.y, NodeType.Obstacle),
            _ => new Node(x.x, x.y, NodeType.Empty)
        }).ToList();

        var nonGuardNodes = new[] { '#', '.' };
        var guardNode = nodes
            .First(x => !nonGuardNodes.Contains(x.c));
        
        _guard = guardNode.c switch
            {
                '^' => new Guard(guardNode.x, guardNode.y, Direction.Up),
                '>' => new Guard(guardNode.x, guardNode.y, Direction.Right),
                'v' => new Guard(guardNode.x, guardNode.y, Direction.Down),
                '<' => new Guard(guardNode.x, guardNode.y, Direction.Left),
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    private Graph GetPath(Graph graph, Guard guard)
    {
        while (true) 
        {
            var nextNode = graph.Nodes.FirstOrDefault(n => guard.Direction switch
            {
                Direction.Left => n.X == guard.X - 1 && n.Y == guard.Y,
                Direction.Up => n.X == guard.X && n.Y == guard.Y - 1,
                Direction.Right => n.X == guard.X + 1 && n.Y == guard.Y,
                Direction.Down => n.X == guard.X && n.Y == guard.Y + 1,
                _ => throw new ArgumentOutOfRangeException()
            });

            if (nextNode == null) break;

            guard = nextNode.Type switch
            {
                NodeType.Empty => guard with
                {
                    X = nextNode.X,
                    Y = nextNode.Y,
                },
                NodeType.Obstacle => guard with
                {
                    Direction = guard.Direction switch
                    {
                        Direction.Left => Direction.Up,
                        Direction.Up => Direction.Right,
                        Direction.Right => Direction.Down,
                        Direction.Down => Direction.Left,
                        _ => throw new ArgumentOutOfRangeException()
                    }
                },
                _ => throw new ArgumentOutOfRangeException()
            };

            if (nextNode.Visit is not null && nextNode.Visit.Direction == guard.Direction)
            {
                return graph with {InfiniteLoop = true};
            }

            nextNode.Visit = nextNode.Type == NodeType.Empty ? new Visit(guard.Direction) : null;
        }

        return graph;
    }

    public object Part1()
    {
        var graph = new Graph(_graph.Select(item => (Node)item.Clone()).ToList());
        var guard = new Guard(_guard.X, _guard.Y, _guard.Direction);
        
        return GetPath(graph, guard).Nodes.Count(x => x.Visit is not null);
    }

    private Task<bool> PathIsInfiniteAsync(Graph graph, Guard guard) => Task.FromResult(GetPath(graph, guard).InfiniteLoop);

    public object Part2() {
        var graph = new Graph(_graph.Select(item => (Node)item.Clone()).ToList());
        var guard = new Guard(_guard.X, _guard.Y, _guard.Direction);
        var visited = GetPath(graph, guard).Nodes.Where(x => x.Visit is not null);

        var tasks = new List<Task<bool>>();
        Parallel.ForEach(visited, n =>
        {
            var newGraph = new Graph(_graph.Select(x => (Node)x.Clone()).ToList());
            var changeNode = newGraph.Nodes.First(x => x.X == n.X && x.Y == n.Y);
            changeNode.Type = NodeType.Obstacle;
            var task = PathIsInfiniteAsync(newGraph, new Guard(_guard.X, _guard.Y, _guard.Direction));
            lock (tasks)
            {
                tasks.Add(task);
            }
        });

        return Task.WhenAll(tasks).GetAwaiter().GetResult().Count(x => x is true);
    }
}