using System;

namespace Day_12
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Tap<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int index = 0;
            foreach (var item in source)
            {
                action(item, index);
                yield return item;
                index++;
            }
        }
    }

    internal class Program
    {
        public class DamagedSpringRow
        {
            public string RowData { get; set; }
            public List<int> DamagedGroups { get; set; }

            public DamagedSpringRow()
            {

            }

            public DamagedSpringRow(string rowData)
            {
                string[] parts = rowData.Split(" ");
                RowData = parts[0];
                DamagedGroups = parts[1].Trim().Split(',').Select(z => z.Trim()).Select(int.Parse).ToList();
            }

            public DamagedSpringRow Unfold()
            {
                string rowData = RowData;
                List<int> damagedGroups = new List<int>();
                damagedGroups.AddRange(DamagedGroups);

                for(int i=0; i<4; i++)
                {
                    rowData += '?' + RowData;
                    damagedGroups.AddRange(DamagedGroups);
                }

                return new DamagedSpringRow()
                {
                    RowData = rowData,
                    DamagedGroups = damagedGroups
                };
            }

            public IEnumerable<string> GetArrangements(int id)
            {
                // Console.WriteLine($"{id}. Calculating for: {RowData} {string.Join(',', DamagedGroups)}");

                var questionMarkIndexes = AllIndexesOf(RowData, '?').ToList();
                char[] data = RowData.ToCharArray();

                int[] counters = new int[questionMarkIndexes.Count];

                while (true)
                {
                    // Generate a permutation based on the current state of counters
                    for (int i = 0; i < counters.Length; i++)
                    {
                        data[questionMarkIndexes[i]] = counters[i] == 0 ? '.' : '#';
                    }

                    if(IsValidArrangement(data, DamagedGroups))
                    {
                        yield return new string(data);
                    }
                    

                    // Increment counters
                    int index = 0;
                    while (index < counters.Length)
                    {
                        counters[index]++;
                        if (counters[index] < 2) // 2 options: '.' or '#'
                        {
                            break;
                        }

                        // Reset the current counter and move to the next
                        counters[index] = 0;
                        index++;
                    }

                    // Check if we have completed all permutations
                    if (index == counters.Length)
                    {
                        break;
                    }
                }
            }

            static IEnumerable<int> AllIndexesOf(string str, char value)
            {
                for (int index = 0; index < str.Length; index++)
                {
                    if (str[index] == value)
                        yield return index;
                }
            }

            private static bool IsValidArrangement(char[] data, List<int> groupCounts)
            {
                int groupIndex = 0;
                int currentGroupLength = 0;

                foreach (char c in data)
                {
                    if (c == '#')
                    {
                        // Counting the length of the current group of '#'
                        currentGroupLength++;
                    }
                    else if (currentGroupLength > 0)
                    {
                        // If we reach a '.', it signifies the end of a group of '#'
                        if (groupIndex >= groupCounts.Count || currentGroupLength != groupCounts[groupIndex])
                        {
                            // If the group length does not match or we have more groups than expected
                            return false;
                        }

                        // Move to the next group in groupCounts
                        groupIndex++;
                        currentGroupLength = 0;
                    }
                }

                // Check the last group if it ends at the end of the string
                if (currentGroupLength > 0)
                {
                    if (groupIndex >= groupCounts.Count || currentGroupLength != groupCounts[groupIndex])
                    {
                        return false;
                    }
                    groupIndex++;
                }

                // Ensure that all groups are accounted for
                return groupIndex == groupCounts.Count;
            }
        }

        static void Main(string[] args)
        {
            string filename = "input.txt";

            List<DamagedSpringRow> springRows = File.ReadAllLines(filename).Select(line => new DamagedSpringRow(line)).ToList();
            var part1Answer = springRows.Select((r, index) => r.GetArrangements(index)).Select(z => z.Count()).Sum();
            Console.WriteLine($"Part 1: {part1Answer}");

            // PART 2: INCOMPLETE
            // The brute force approach does not scail

            List<DamagedSpringRow> unfoldedSpringRows = springRows.Select(z => z.Unfold()).ToList();
            var part2Answer = unfoldedSpringRows.Select((r, index) => r.GetArrangements(index)).Select(z => z.Count()).Sum();

            Console.WriteLine($"Part 2: {part2Answer}");


        }
    }
}