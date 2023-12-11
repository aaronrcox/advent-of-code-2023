namespace Day_11
{

    

    public class Universe
    {
        List<List<char>> galaxyMap;
        List<int> yOffsets;
        List<int> xOffsets;
        public int RowLength { get => galaxyMap[0].Count; }
        public int ColLength { get => galaxyMap.Count; }

        List<int> GalexyTiles { get; set; }

        public Universe(List<string> input)
        {
            galaxyMap = input.Select(x => x.ToCharArray().ToList()).ToList();
            CalculateGalexyTiles();
        }

        public void CalculateGalexyTiles()
        {
            GalexyTiles = new List<int>();
            for(int y=0; y< ColLength; y++)
            {
                for(int x=0; x<RowLength; x++)
                {
                    if (galaxyMap[y][x] == '#')
                        GalexyTiles.Add(CalculateTileIndex(x, y));
                }
            }
        }

        public int CalculateTileIndex(int xIndex, int yIndex)
        {
            return yIndex * RowLength + xIndex;
        }

        public int CalculateSteps(int startX, int startY, int endX, int endY)
        {
            int horizontalSteps = Math.Abs(endX - startX);
            int verticalSteps = Math.Abs(endY - startY);

            return horizontalSteps + verticalSteps;
        }

        public void Expand2(int expandBy = 1)
        {
            int currentRowOffset = 0;
            yOffsets = new List<int>(Enumerable.Repeat<int>(0, ColLength));
            for (int y = 0; y < ColLength; y++)
            {
                bool canExpand = true;
                for (int x = 0; x < RowLength; x++)
                {
                    if (galaxyMap[y][x] != '.')
                    {
                        canExpand = false;
                        break;
                    }
                }

                if (canExpand)
                {
                    currentRowOffset += expandBy-1;
                }

                yOffsets[y] = currentRowOffset + y;

            }



            xOffsets = new List<int>(Enumerable.Repeat<int>(0, RowLength));
            int currentColOffset = 0;
            for (int x = 0; x < RowLength; x++)
            {
                bool canExpand = true;
                for (int y = 0; y < ColLength; y++)
                {
                    if (galaxyMap[y][x] != '.')
                    {
                        canExpand = false;
                        break;
                    }
                }

                if (canExpand)
                {
                    currentColOffset += expandBy-1;
                }
                xOffsets[x] = currentColOffset + x;
            }
        }

        public void Expand(int expandBy = 1)
        {
            List<int> rowIndexToExpand = new List<int>();
            for(int y=0; y<galaxyMap.Count; y++)
            {
                bool canExpand = true;
                for(int x=0; x < galaxyMap[y].Count; x++)
                {
                    if (galaxyMap[y][x] != '.')
                    {
                        canExpand = false;
                        break;
                    }
                }

                if (canExpand)
                {
                    rowIndexToExpand.Add(y);
                }
            }

            // expand the rows
            // loop backwards so that inserted rows dont affect the index
            for(int i= rowIndexToExpand.Count-1; i >=0; i--)
            {
                List<char> rowToAdd = (new string('.', ColLength)).ToCharArray().ToList();
                for(int j= 0; j < expandBy; j++)
                    galaxyMap.Insert(rowIndexToExpand[i], rowToAdd);
            }

            List<int> colIndexToExpand = new List<int>();
            for (int x = 0; x < RowLength; x++)
            {
                bool canExpand = true;
                for (int y = 0; y < ColLength; y++)
                {
                    if (galaxyMap[y][x] != '.')
                    {
                        canExpand = false;
                        break;
                    }
                }

                if (canExpand)
                {
                    colIndexToExpand.Add(x);
                }
            }

            // expand the columns
            for (int i = colIndexToExpand.Count - 1; i >= 0; i--)
            {
                int col = colIndexToExpand[i];
                for(int y = 0; y < ColLength; y++)
                {
                    for (int j = 0; j < expandBy; j++)
                        galaxyMap[y].Insert(col, '.');
                }
            }

            CalculateGalexyTiles();
        }

        public void Print()
        {
            for(int y=0; y < ColLength; y++)
            {
                for(int x=0; x < RowLength; x++)
                {
                    Console.Write(galaxyMap[y][x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public List<long> GetAllDistances()
        {
            var pairs = GalexyTiles.SelectMany((value, index) => GalexyTiles.Skip(index + 1),
                        (first, second) => (first, second)).ToList();

            List<long> distances = new List<long>();

            for(int i=0; i<pairs.Count; i++)
            {
                int startIndex = pairs[i].first;
                int endIndex = pairs[i].second;

                int sx = startIndex % RowLength;
                sx = xOffsets[sx];
                int sy = startIndex / RowLength;
                sy = yOffsets[sy];

                int ex = endIndex % RowLength;
                ex = xOffsets[ex];
                int ey = endIndex / RowLength;
                ey = yOffsets[ey];

                distances.Add(CalculateSteps(sx, sy, ex, ey));
            }

            return distances;

        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";
            var lines = File.ReadAllLines(filename).ToList();

            var universe1 = new Universe(lines);
            universe1.Expand2();            
            long p1Answer = universe1.GetAllDistances().Sum();
            Console.WriteLine($"Part 1: {p1Answer}");


            var universe2 = new Universe(lines);
            universe2.Expand2(1000000);
            long p2Answer = universe2.GetAllDistances().Sum();

            
            Console.WriteLine($"Part 2: {p2Answer}");

        }
    }
}