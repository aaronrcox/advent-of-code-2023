using System.Numerics;

namespace Day_08
{
    public class Node
    {
        public string Name { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
        public bool IsEndNode { get; set; }

        public Node(string input)
        {
            string[] parts = input.Split('=').Select(z => z.Trim()).ToArray();
            Name = parts[0];
            parts = parts[1].Replace("(", "").Replace(")", "").Split(",").Select(z => z.Trim()).ToArray();
            Left = parts[0];
            Right = parts[1];

            IsEndNode = Name.Last() == 'Z';
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            

            // PART 1: 
            // ==============================
            {
                string filename = "input-test-a.txt";
                var lines = File.ReadAllLines(filename);

                string instructions = lines[0];
                Dictionary<string, Node> nodes = lines.Skip(2)
                    .Select(line => new Node(line))
                    .ToDictionary(node => node.Name, node => node);

                Node current = nodes["AAA"];
                int instructionIndex = 0;
                int steps = 0;
                while (current.Name != "ZZZ")
                {
                    steps += 1;
                    string next = "";
                    if (instructions[instructionIndex] == 'R') next = current.Right;
                    if (instructions[instructionIndex] == 'L') next = current.Left;

                    current = nodes[next];
                    instructionIndex = (instructionIndex + 1) % instructions.Length;
                }

                Console.WriteLine($"Part1: {steps}");
            }

            

            // PART 2: 
            // ==============================
            {
                string filename = "input.txt";
                var lines = File.ReadAllLines(filename);

                string instructions = lines[0];
                Dictionary<string, Node> nodes = lines.Skip(2)
                    .Select(line => new Node(line))
                    .ToDictionary(node => node.Name, node => node);


                List<Node> currentNodes = nodes.Values.Where(z => z.Name.EndsWith('A')).ToList();
                List<long> stepsToEnd = new List<long>();
                int instructionIndex = 0;
                long steps = 0;
                
                while(stepsToEnd.Count < currentNodes.Count)
                {
                    steps += 1;
                    for (int i=0; i<currentNodes.Count; i++)
                    {
                        if (currentNodes[i].IsEndNode)
                            continue;

                        string next = "";
                        if (instructions[instructionIndex] == 'R') next = currentNodes[i].Right;
                        if (instructions[instructionIndex] == 'L') next = currentNodes[i].Left;

                        currentNodes[i] = nodes[next];
                        if (currentNodes[i].IsEndNode)
                        {
                            stepsToEnd.Add(steps);
                            Console.WriteLine(steps);
                        }
                            
                    }
                    
                    instructionIndex = (instructionIndex + 1) % instructions.Length;
                }

                long part2Answer = LCM(stepsToEnd);

                Console.WriteLine($"Part 2: {part2Answer}");

            }



        }

        // Method to calculate the LCM of a list of numbers
        public static long LCM(List<long> numbers)
        {
            if (numbers == null || numbers.Count == 0)
            {
                throw new ArgumentException("List of numbers cannot be null or empty.");
            }

            long lcm = numbers[0];
            for (int i = 1; i < numbers.Count; i++)
            {
                lcm = LCM(lcm, numbers[i]);
            }
            return lcm;
        }

        // Helper method to calculate LCM of two numbers
        private static long LCM(long a, long b)
        {
            return (a / (long)BigInteger.GreatestCommonDivisor(new BigInteger(a), new BigInteger(b))) * b;
        }
    }
}