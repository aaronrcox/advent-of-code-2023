namespace Day_06
{

    public class RaceResult
    {
        public long HoldTime { get; set; }
        public long Distance { get; set; }

        public RaceResult(long holdTime, long distance) 
        {
            HoldTime = holdTime;
            Distance = distance;
        }
    }

    public class Race
    {
        public Race(long time, long distance)
        {
            Time = time;
            Distance = distance;
        }

        public long Time { get; set; }
        public long Distance { get; set; }

        public IEnumerable<RaceResult> CalcualteRaceResults()
        {
            for(long t=0; t<=Time; t++)
            {
                long speed = t;
                long timeLeft = Time - t;
                long distance = speed * timeLeft;
                yield return new RaceResult(t, distance);
            }
        }

        public IEnumerable<RaceResult> GetWinningRances()
        {
            return CalcualteRaceResults().Where(z => z.Distance > Distance);
        }
    }

    internal class Program
    {
        private static IEnumerable<int> ParseLine(string line)
        {
            return line.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries)
                   .Skip(1) // Skip the first element ("Time" or "Distance")
                   .Select(int.Parse)
                   .ToList();
        }
        static void Main(string[] args)
        {
            string filename = "input.txt";

            var lines = File.ReadAllLines(filename).ToList();

            var times = ParseLine(lines[0]).ToList();
            var distances = ParseLine(lines[1]).ToList();

            var racesP1 = times.Zip(distances, (time, distance) => new Race(time, distance)).ToList();
            int part1Answer = racesP1.Select(r => r.GetWinningRances().Count()).Aggregate(1, (total, next) => total * next);

            Console.WriteLine($"Part 1: {part1Answer}");

            var combinedTime = long.Parse(string.Join("", times.Select(t => t.ToString())));
            var combinedDist = long.Parse(string.Join("", distances.Select(d => d.ToString())));
            Race raceP2 = new Race(combinedTime, combinedDist);

            int part2Answer = raceP2.GetWinningRances().Count();
            Console.WriteLine($"Part 2: {part2Answer}");

        }
    }
}