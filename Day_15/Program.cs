using System.Reflection.Emit;

namespace Day_15
{
    class Lens
    {
        public string Label { get; set; }
        public string FocalLength { get; set; }
    }

    class Box
    {
        public int Index { get; set; }
        public Box(int index)
        {
            Index = index;
        }
        public List<Lens> Lenses { get; set; } = new List<Lens>();

        public void Print()
        {
            if(Lenses.Count > 0)
            {
                Console.Write($"Box {Index}: ");
            }
            foreach(var lens in Lenses)
            {
                Console.Write($"[{lens.Label} {lens.FocalLength}]");
            }
            if (Lenses.Count > 0)
            {
                Console.WriteLine();
            }
        }

    }
    internal class Program
    {
        static int HashString(string s)
        {
            int currentValue = 0;
            foreach( char c in s)
            {
                currentValue += c;
                currentValue *= 17;
                currentValue = currentValue % 256;
            }
            return currentValue;
        }

        static void Main(string[] args)
        {
            string filename = "input.txt";
            string data = File.ReadAllText(filename);

            string[] instructions = data.Split(',');
            int part1Answer = instructions.Select(x => HashString(x)).Sum();
            Console.WriteLine($"Part 1: {part1Answer}");

            Box[] boxes = new Box[256];
            for(int i=0; i<256; i++)
                boxes[i] = new Box(i);

            foreach(var instruction in instructions)
            {             

                if(instruction.EndsWith('-'))
                {
                    string lable = instruction.TrimEnd('-');
                    int index = HashString(lable);
                    int lenseIndex = boxes[index].Lenses.FindIndex(z => z.Label == lable);
                    if(lenseIndex >= 0)
                    {
                        boxes[index].Lenses.RemoveAt(lenseIndex);
                    }
                    
                }
                else
                {
                    string[] parts = instruction.Split('=');
                    string lable = parts[0];
                    string count = parts[1];
                    int index = HashString(lable);
                    int lenseIndex = boxes[index].Lenses.FindIndex(z => z.Label == lable);
                    if(lenseIndex >= 0)
                    {
                        boxes[index].Lenses[lenseIndex].FocalLength = count;
                    }
                    else
                    {
                        boxes[index].Lenses.Add(new Lens() { Label = lable, FocalLength = count });
                    }
                }

                Console.WriteLine($"After \"{instruction}\"");
                for (int i = 0; i < 256; i++)
                    boxes[i].Print();
                Console.WriteLine();
            }


            int part2Answer = boxes.SelectMany((box, boxIndex) => box.Lenses
                .Select((lense, lenseIndex) => new { BoxIndex = boxIndex, LensIndex = lenseIndex, FocalLength = lense.FocalLength }))
                .Select(z => ((z.BoxIndex + 1) * (z.LensIndex + 1) * int.Parse(z.FocalLength)))
                .Sum();

            Console.WriteLine($"Part 2: {part2Answer}");
        }
    }
}