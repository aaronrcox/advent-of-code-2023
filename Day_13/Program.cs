using System;
using System.Reflection.Metadata.Ecma335;

namespace Day_13
{

    public class Pattern
    {
        char[][] Data { get; set; }
        int Rows { get => Data.Length; }
        int Cols { get => Data[0].Length; }

        public readonly int verticalReflectionIndex;
        public readonly int horizontalReflectionIndex;

        public readonly int verticalReflectionSmudgeFixIndex;
        public readonly int horizontalReflectionSmudgeFixIndex;



        public Pattern(string input)
        {
            Data = input.Split("\n").Select(line => line.ToCharArray()).ToArray();

            

            verticalReflectionIndex = CheckVerticalReflection();
            horizontalReflectionIndex = CheckHorizontalReflection();

            var (sy, sx) = FindSmudge();
            Data[sy][sx] = Data[sy][sx] == '#' ? '.' : '#';

            verticalReflectionSmudgeFixIndex = CheckVerticalReflection();
            horizontalReflectionSmudgeFixIndex = CheckHorizontalReflection();
        }

        string GetRowAsString(int rowIndex)
        {
            return new string(Data[rowIndex]);
        }

        string GetColAsString(int colIndex)
        {
            return string.Concat(Data.Select(row => row[colIndex]));
        }

        

        

        public int CheckReflection(Func<int, string> getLine, int dimensionLength)
        {
            for (int index = 0; index < dimensionLength - 1; index++)
            {
                if (IsLineReflection(index, getLine, dimensionLength))
                {
                    return index;
                }
            }

            return -1; // No reflection found
        }

        private bool IsLineReflection(int index, Func<int, string> getLine, int dimensionLength)
        {
            for (int offset = 0; offset <= index; offset++)
            {
                int line1Index = index - offset;
                int line2Index = index + offset + 1;

                // Check bounds
                if (line1Index < 0 || line2Index >= dimensionLength)
                    break;

                string line1 = getLine(line1Index);
                string line2 = getLine(line2Index);

                if (line1 != line2)
                    return false;
            }

            return true;
        }

        int CheckVerticalReflection()
        {

            return CheckReflection(GetColAsString, Cols);
        }

        int CheckHorizontalReflection()
        {
            return CheckReflection(GetRowAsString, Rows);
        }

        public (int, int) FindSmudge()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    // Flip the cell's value
                    Data[row][col] = Data[row][col] == '.' ? '#' : '.';

                    // Check for a new line of reflection
                    if (CheckVerticalReflection() != verticalReflectionIndex ||
                        CheckHorizontalReflection() != horizontalReflectionIndex)
                    {
                        // Restore the cell's original value
                        Data[row][col] = Data[row][col] == '.' ? '#' : '.';
                        return (row, col); // Found the smudge
                    }

                    // Restore the cell's original value
                    Data[row][col] = Data[row][col] == '.' ? '#' : '.';
                }
            }

            return (-1, -1); // No smudge found
        }

        public void Print(bool useFixedReflectionIndex)
        {
            int vri = verticalReflectionIndex;
            int hri = horizontalReflectionIndex;

            if(useFixedReflectionIndex)
            {
                vri = verticalReflectionSmudgeFixIndex;
                hri = horizontalReflectionSmudgeFixIndex;
            }

            // Print column headers
            Console.Write("  ");
            for (int col = 0; col <Cols; col++)
            {
                Console.Write(col % 10); // Modulo 10 to avoid multi-digit numbers
            }
            Console.WriteLine();

            // Print row vertical reflection indicator, if present
            Console.Write("  ");
            for (int col = 0; col < Cols; col++)
            {
                if (col == vri)
                {
                    Console.WriteLine("><");
                    break;
                }
                else
                {
                    Console.Write("-" + ((col == Cols - 1) ? "\n" : ""));
                }
            }

            // Print each row with horizontal reflection indicator, if present
            for (int row = 0; row < Rows; row++)
            {
                // Print row index
                Console.Write($"{row % 10}");

                // Check for horizontal reflection line
                if (row == hri)
                {
                    Console.Write("V");
                }
                else if (row == hri + 1 && hri >= 0)
                {
                    Console.Write("^");
                }
                else
                {
                    Console.Write("|");
                }

                // Print the row data
                Console.WriteLine(GetRowAsString(row));
            }
        }
        
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input-test.txt";
            var patterns = File.ReadAllText(filename).Split("\n\n").Select(lines => new Pattern(lines)).ToList();

            int reflectedColumns = patterns.Where(z => z.verticalReflectionIndex >= 0).Select(z => z.verticalReflectionIndex + 1).Sum();
            int reflectedRows = patterns.Where(z => z.horizontalReflectionIndex >= 0).Select(z => z.horizontalReflectionIndex + 1).Sum();
            int part1Answer = reflectedColumns + (100 * reflectedRows);

            Console.WriteLine($"Part 1: {part1Answer}");

            int reflectedSmudgeFixColumns = patterns.Where(z => z.verticalReflectionSmudgeFixIndex >= 0).Select(z => z.verticalReflectionSmudgeFixIndex + 1).Sum();
            int reflectedSmudgeFixRows = patterns.Where(z => z.horizontalReflectionSmudgeFixIndex >= 0).Select(z => z.horizontalReflectionSmudgeFixIndex + 1).Sum();
            int part2Answer = reflectedSmudgeFixColumns + (100 * reflectedSmudgeFixRows);

            Console.WriteLine($"Part 2: {part2Answer}");

            foreach( var pattern in patterns )
            {
                pattern.Print(true);
            }

        }
    }
}