namespace Day_09
{

    class Sequence
    {
        public List<List<int>> Numbers { get; set; } = new List<List<int>>();

        public Sequence(string input)
        {
            var numbers = input.Split(" ").Select(x => x.Trim()).Select(int.Parse).ToList();
            Numbers.Add(numbers);
            Fill();
            PredictForward();
            PredictBackward();
        }

        public void Fill()
        {
            List<int> nextset = new List<int>();
            do
            {
                nextset = new List<int>();
                var lastSet = Numbers.Last();
                for (int i = 0; i < lastSet.Count - 1; i++)
                {
                    int diff = lastSet[i + 1] - lastSet[i];
                    nextset.Add(diff);
                }
                Numbers.Add(nextset);                
                
            } while (Numbers.Last().All(x => x == 0) == false);
        }



        public void PredictForward()
        {
            Numbers.Last().Add(0);
            for(int i = Numbers.Count-2; i>= 0; i--)
            {
                int val = Numbers[i].Last() + Numbers[i + 1].Last();
                Numbers[i].Add(val);
            }
        }

        public void PredictBackward()
        {
            Numbers.Last().Insert(0, 0);
            for (int i = Numbers.Count - 2; i >= 0; i--)
            {
                int val = Numbers[i].First() - Numbers[i + 1].First();
                Numbers[i].Insert(0, val);
            }
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";
            var lines = File.ReadAllLines(filename);

            List<Sequence> sequences = lines.Select(x => new Sequence(x)).ToList();
            int part1Answer = sequences.Select(sequence => sequence.Numbers[0].Last()  ).Sum();
            int part2Answer = sequences.Select(sequence => sequence.Numbers[0].First()).Sum();

            Console.WriteLine($"Part 1: {part1Answer}");
            Console.WriteLine($"Part 1: {part2Answer}");

        }
    }
}