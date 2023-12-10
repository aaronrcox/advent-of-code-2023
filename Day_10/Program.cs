using System.Globalization;
using System.IO.Enumeration;

namespace Day_10
{

    public record MapTile(int index, int xIndex, int yIndex, char lable);
    public record MapTileConnection(MapTile? from, MapTile to, EDirection dir);

    public class MapTileSearchNode
    {
        public MapTile Tile { get; set; }
        public MapTileSearchNode? Parent { get; set; }

        public MapTileSearchNode(MapTile node, MapTileSearchNode? parent)
        {
            Tile = node;
            Parent = parent;
        }

        public int DistanceToParent { get => Parent == null ? 0 : Parent.DistanceToParent + 1; }

    }

    public enum EDirection
    {
        NORTH,
        SOUTH,
        EAST,
        WEST,
        NONE
    }

    public class Map
    {
        public char[][] Tiles { get; set; }
        public int[][] PipeDistances { get; set; }
        public int RowLength { get => Tiles[0].Length; }
        public int ColLength { get => Tiles.Length; }

        Dictionary<int, List<MapTileConnection>> TileConnections = new Dictionary<int, List<MapTileConnection>>();

        public List<MapTile> PipeLoop { get; set; } = new List<MapTile>();

        public Map(List<string> input)
        {
            Tiles = input.Select(x => x.ToCharArray()).ToArray();
            TileConnections = AllTiles().Select(tile => new { Tile = tile, Connections = GetConnectedTiles(tile) })
                .ToDictionary(kvp => kvp.Tile.index, kvp => kvp.Connections.ToList());

            DijkstraSearch();

        }

        void DijkstraSearch()
        {
            PipeDistances = Tiles.Select(tile => Enumerable.Repeat(-1, tile.Length).ToArray()).ToArray();
            MapTile? startTile = AllTiles().FirstOrDefault(tile => tile.lable == 'S');
            if (startTile == null)
                return;

            PipeDistances[startTile.yIndex][startTile.xIndex] = 0;

            List<MapTileSearchNode> open = new List<MapTileSearchNode>();
            List<MapTile> visited = new List<MapTile>();

            open.Add(new MapTileSearchNode(startTile, null));
            
            while(open.Count != 0)
            {
                MapTileSearchNode node = open.First();
                open.RemoveAt(0);
                visited.Add(node.Tile);

                var children = TileConnections[node.Tile.index]
                    .Where(z => z.to.index != node.Parent?.Tile.index)
                    .Where(z => visited.Contains(z.to) == false)
                    .Select(z => z).ToList();

                foreach(var child in children)
                {
                    MapTileSearchNode childSearchNode = new MapTileSearchNode(child.to, node);

                    if (childSearchNode.Parent?.Tile == childSearchNode.Tile)
                        continue;

                    var distToParent = childSearchNode.DistanceToParent;

                    // is there an existing node on the open/closed list with a better distance than this child node
                    int distanceRecorded = PipeDistances[childSearchNode.Tile.yIndex][childSearchNode.Tile.xIndex];



                    if (distToParent > distanceRecorded || distanceRecorded == -1)
                    {
                        PipeDistances[childSearchNode.Tile.yIndex][childSearchNode.Tile.xIndex] = distToParent;
                        open.Add(childSearchNode);
                    }
                    else
                    {
                        
                    }
                }


                open = open.OrderBy(z => z.DistanceToParent).ToList();
            }
        }

        public int BestDistance()
        {
            return PipeDistances.SelectMany(innerArray => innerArray).Max();
        }


        public IEnumerable<MapTile> AllTiles()
        {
            for (int y = 0; y < Tiles.Length; y++)
            {
                for (int x = 0; x < Tiles[y].Length; x++)
                {
                    yield return new MapTile(CalculateTileIndex(x, y), x, y, Tiles[y][x]);
                }
            }
        }

        public IEnumerable<MapTileConnection> GetConnectedTiles(MapTile tile)
        {
            var north = GetTile(tile.xIndex, tile.yIndex - 1);
            var south = GetTile(tile.xIndex, tile.yIndex + 1);
            var east = GetTile(tile.xIndex + 1, tile.yIndex);
            var west = GetTile(tile.xIndex - 1, tile.yIndex);

            if ( (tile.lable == 'S' || tile.lable == '|' || tile.lable == 'L' || tile.lable == 'J') && (north?.lable == 'S' || north?.lable == '|' || north?.lable == '7' || north?.lable == 'F')) yield return new (tile, north, EDirection.NORTH);
            if ( (tile.lable == 'S' || tile.lable == '|' || tile.lable == '7' || tile.lable == 'F') && (south?.lable == 'S' || south?.lable == '|' || south?.lable == 'J' || south?.lable == 'L')) yield return new (tile, south, EDirection.SOUTH);
            if ( (tile.lable == 'S' || tile.lable == '-' || tile.lable == 'L' || tile.lable == 'F') && (east?.lable == 'S' || east?.lable == '-' || east?.lable == 'J' || east?.lable == '7')) yield return new (tile, east, EDirection.EAST);
            if ( (tile.lable == 'S' || tile.lable == '-' || tile.lable == '7' || tile.lable == 'J') && (west?.lable == 'S' || west?.lable == '-' || west?.lable == 'F' || west?.lable == 'L')) yield return new (tile, west, EDirection.WEST);
        }

        public MapTile? GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= RowLength || y >= ColLength)
                return null;

            return new MapTile(CalculateTileIndex(x, y), x, y, Tiles[y][x]);
        }

        public bool IsPipeTile(int xIndex, int yIndex)
        {
            char t = Tiles[yIndex][xIndex];
            return t == 'S' || t == '|' || t == '-' || t == 'L' || t == 'J' || t == '7' || t == 'F';
        }

        public int CalculateTileIndex(int xIndex, int yIndex)
        {
            return yIndex * RowLength + xIndex;
        }

        public static string PrintMapTile(Map m, int xIndex, int yIndex)
        {
            return m.Tiles[yIndex][xIndex].ToString();
        }

        public static string PrintMapTileIndex(Map m, int xIndex, int yIndex)
        {
            return $"{m.CalculateTileIndex(xIndex, yIndex):D3} ";
        }

        public static string PrintMapTileXY(Map m, int xIndex, int yIndex)
        {
            return $"[{xIndex:D3},{yIndex:D3}] ";
        }

        public static string PrintLoopTile(Map m, int xIndex, int yIndex)
        {
            if (m.PipeDistances[yIndex][xIndex] >= 0)
                return m.Tiles[yIndex][xIndex].ToString();
            return ".";
        }

        public static string PrintLoopAndInnerTiles(Map m, int xIndex, int yIndex)
        {
            // print the pipe tile
            if (m.PipeDistances[yIndex][xIndex] >= 0)
                return m.Tiles[yIndex][xIndex].ToString();

            if (m.IsTileInLoop(xIndex, yIndex))
                return "*";

            return ".";

        }

        public static string PrintMapDistances(Map m, int xIndex, int yIndex)
        {
            int dist = m.PipeDistances[yIndex][xIndex];
            if(!m.IsPipeTile(xIndex, yIndex)) return "[   ] ";
            return $"[{dist:D3}] ";
        }

        public void Print(Func<Map, int, int, string> printTileFn)
        {

            for(int y=0; y<Tiles.Length; y++)
            {
                for(int x=0; x < Tiles[y].Length; x++)
                {
                    Console.Write(printTileFn(this, x, y));
                }
                Console.WriteLine();
            }
        }

        public bool IsTileInLoop(int xIndex, int yIndex)
        {
            // is the tile a pipe tile - if so, return false
            if (PipeDistances[yIndex][xIndex] >= 0)
                return false;

            // search in any direction.. we're going to move left from the current tile
            // if we encounter an odd number of pipe tiles, than we ARE within the loop.
            // if we encounter an even number of pipe tiles, than we are NOT within the loop
            int intersectPipeCount = 0;

            for(int i=xIndex-1; i>=0; i--)
            {
                if(PipeDistances[yIndex][xIndex-i] >= 0)
                {
                    // traverse the index to the end of the pipe eg for a pipe like ---- we need to traverse to the end
                    char currentTile = Tiles[yIndex][xIndex-i];
                    char nextTile = Tiles[yIndex][xIndex-i-1];
                    while(true)
                    {
                        // if ((currentTile == 'S' || currentTile == '-' || currentTile == '7' || currentTile == 'J') && (nextTile == 'S' || nextTile == '-' || nextTile == 'F' || nextTile == 'L'))
                        if(nextTile == '|')
                        {
                            currentTile = nextTile;
                            i -= 1;
                            nextTile = Tiles[yIndex][xIndex - i];
                        }
                        else
                        {
                            break;
                        }
                    }

                    intersectPipeCount += 1;

                }
            }

            return intersectPipeCount % 2 == 1;



        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input-test-2.txt";
            var lines = File.ReadAllLines(filename).ToList();

            Map map = new Map(lines);
            map.Print(Map.PrintMapTile);
            map.Print(Map.PrintLoopAndInnerTiles);

            int part1Answer = map.BestDistance();
            Console.WriteLine($"Part1: {part1Answer}");

        }
    }
}