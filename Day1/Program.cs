namespace Day1
{
    class CharMap
    {
        public int Index { get; set; }
        public string Character { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";
            var lines = File.ReadAllLines(filename);
            var lineDigits = lines.Select(line =>
            {
                var charMap = new List<CharMap>();
                charMap.AddRange(AllIndexesOf(line, "one").Select(i => new CharMap() { Character = "1", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "1").Select(i => new CharMap() { Character = "1", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "two").Select(i => new CharMap() { Character = "2", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "2").Select(i => new CharMap() { Character = "2", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "three").Select(i => new CharMap() { Character = "3", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "3").Select(i => new CharMap() { Character = "3", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "four").Select(i => new CharMap() { Character = "4", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "4").Select(i => new CharMap() { Character = "4", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "five").Select(i => new CharMap() { Character = "5", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "5").Select(i => new CharMap() { Character = "5", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "six").Select(i => new CharMap() { Character = "6", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "6").Select(i => new CharMap() { Character = "6", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "seven").Select(i => new CharMap() { Character = "7", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "7").Select(i => new CharMap() { Character = "7", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "eight").Select(i => new CharMap() { Character = "8", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "8").Select(i => new CharMap() { Character = "8", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "nine").Select(i => new CharMap() { Character = "9", Index = i }));
                charMap.AddRange(AllIndexesOf(line, "9").Select(i => new CharMap() { Character = "9", Index = i }));
                return charMap.OrderBy(z => z.Index);
            });


            var lineNums = lineDigits.Select(line => int.Parse( line.First().Character.ToString() + line.Last().Character.ToString() ));
            var total = lineNums.Sum();
            Console.WriteLine(total);
        }

        static IEnumerable<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                yield break;

            int startIndex = 0;
            while ((startIndex = str.IndexOf(value, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                yield return startIndex;
                startIndex += value.Length; // Use startIndex++ to find overlapping matches
            }
        }
    }
}