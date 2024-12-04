namespace AdventOfCode2024.Solutions;

public class Day4 : BaseSolution, ISolution
{
    enum Direction
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }
    
    private record Node(int X, int Y, char Letter);

    private List<Node> _graph;
    
    public override int Day() => 4;

    public Day4()
    {
        _graph = Lines.SelectMany((l, y) => 
            l.Select((letter, x) => new Node(x, y, letter)))
            .ToList();
    }

    private Node? GetNextNode(Node current, Direction direction)
        => _graph.FirstOrDefault(p => direction switch
        {
            Direction.Up => p.Y == current.Y - 1 && p.X == current.X,
            Direction.UpRight => p.Y == current.Y - 1 && p.X == current.X + 1,
            Direction.Right => p.Y == current.Y && p.X == current.X + 1,
            Direction.DownRight => p.Y == current.Y + 1 && p.X == current.X + 1,
            Direction.Down => p.Y == current.Y + 1 && p.X == current.X,
            Direction.DownLeft => p.Y == current.Y + 1 && p.X == current.X - 1,
            Direction.Left => p.Y == current.Y && p.X == current.X - 1,
            Direction.UpLeft => p.Y == current.Y - 1 && p.X == current.X - 1,
            _ => false
        });

    private bool FindXmas(Node node, Direction direction)
    {
        while (true)
        {
            if (node.Letter == 'S') 
                return true;
            
            var nextNode = GetNextNode(node, direction);
            
            if (nextNode == null) 
                return false;

            if ((node.Letter == 'X' && nextNode.Letter == 'M') || 
                (node.Letter == 'M' && nextNode.Letter == 'A') || 
                (node.Letter == 'A' && nextNode.Letter == 'S'))
            {
                node = nextNode;
                continue;
            }

            return false;
        }
    }

    public object Part1()
        => _graph.Where(x => x.Letter == 'X')
            .SelectMany(node => Enum.GetValues(typeof(Direction))
                .Cast<Direction>()
                .Where(direction => FindXmas(node, direction)))
            .Count();

    public object Part2()
    {
        return _graph.Where(x => x.Letter == 'A')
            .Count(n =>
            {
                var upLeft = GetNextNode(n, Direction.UpLeft);
                var upRight = GetNextNode(n, Direction.UpRight);
                var downRight = GetNextNode(n, Direction.DownRight);
                var downLeft = GetNextNode(n, Direction.DownLeft);

                var nodes = new List<Node?>() { upLeft, upRight, downRight, downLeft };
                var m = nodes.Where(x => x is { Letter: 'M' }).ToList();
                var s = nodes.Where(x => x is { Letter: 'S' }).ToList();
                return m.Count == 2 && s.Count == 2 && 
                    (m.First().X == m.Last().X || m.First().Y == m.Last().Y) && 
                    (s.First().X == s.Last().X || s.First().Y == s.Last().Y);
            });
    }
}