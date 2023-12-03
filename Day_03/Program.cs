using System.Text.RegularExpressions;

namespace Day_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";
            List<string> schema = File.ReadAllLines(filename).Select(line => $".{line}.").ToList();
            schema.Insert(0, new string('.', schema[0].Length));
            schema.Add(new string('.', schema[0].Length));

            List<string> partNumbers = new List<string>();
            List<string> excludedPartNumbers = new List<string>();

            List<List<string>> geers = new();

            for(int y=1; y<schema.Count-1; y++)
            {
                for(int x=1; x < schema[y].Length-1; x++)
                {
                    if (char.IsDigit(schema[y][x]))
                    {
                        // look for the end digit
                        int cx1 = x;
                        int len = 1;
                        while (char.IsDigit(schema[y][x + len])) len++;

                        string l1 = schema[y - 1].Substring(x-1, len+2);
                        string l2 = schema[y].Substring(x-1, len+2);
                        string l3 = schema[y + 1].Substring(x-1, len+2);

                        string combined = $"{l1}{l2}{l3}";
                        string partNum = schema[y].Substring(x, len);
                        if (combined.Any(c => !char.IsLetterOrDigit(c) && c != '.'))
                        { 
                            partNumbers.Add(partNum);
                        }
                        else
                        {
                            excludedPartNumbers.Add(partNum);
                        }

                        x += len - 1;

                    }
                }
            }

            int part1Answer = partNumbers.Select(z => int.Parse(z)).Sum();
            Console.WriteLine($"Part1 Answer: {part1Answer}");
            Console.WriteLine("----------------");

            for (int y = 1; y < schema.Count - 1; y++)
            {
                for (int x = 1; x < schema[y].Length - 1; x++)
                {
                    char c = schema[y][x];
                    if (c != '*') continue;

                    string l1 = schema[y - 1].Substring(x-1, 3);
                    string l2 = schema[y].Substring(x-1, 3);
                    string l3 = schema[y + 1].Substring(x-1, 3);

                    int adjacentNumberCount = CountNumbers(l1) + CountNumbers(l2) + CountNumbers(l3);
                    if (adjacentNumberCount != 2)
                        continue;

                    //Console.WriteLine(l1);
                    //Console.WriteLine(l2);
                    //Console.WriteLine(l3);
                    //Console.WriteLine("---");

                    List<string> gearPair = new List<string>();

                    if (CountNumbers(l1) != 0) gearPair.AddRange(SeekNumber(x, schema[y - 1]));
                    if (CountNumbers(l2) != 0) gearPair.AddRange(SeekNumber(x, schema[y]));
                    if (CountNumbers(l3) != 0) gearPair.AddRange(SeekNumber(x, schema[y + 1]));

                    geers.Add(gearPair);


                }
            }


            int part2Answer = geers.Select(pair => pair.Select(int.Parse).Aggregate(1, (total, next) => total * next)).Sum();
            Console.WriteLine($"Part2 Answer: {part2Answer}");

        }

        public static List<string> SeekNumber(int index, string s)
        {
            List<string> numbers = new();

            // find any numbers to the left of the index
            int start = index-1;
            while (char.IsDigit(s[start])) start -= 1;
            start +=1;
            int len = 1;

            if (char.IsDigit(s[start]))
            {
                while (char.IsDigit(s[start + len])) len++;

                numbers.Add(s.Substring(start, len));
            }
            
            

            // find numbers to the right of the index
            if (char.IsDigit(s[index]))
                return numbers;

            start = index + 1;
            len = 1;
            if (char.IsDigit(s[start]))
            {


                while (char.IsDigit(s[start + len])) len++;

                numbers.Add(s.Substring(start, len));
            }

            return numbers;
        }

        public static int CountNumbers(string s)
        {
            Regex regex = new Regex(@"\d+");
            return regex.Matches(s).Count;
        }
    }
}