namespace Day_02
{
    public class GameSet
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public int Power
        {
            get
            {
                return Red * Green * Blue;
            }
        }
                
    }

    public class Game
    {
        public int Id { get; set; }
        List<GameSet> GameSets { get; set; } = new List<GameSet>();



        public Game(string input)
        {
            string[] parts = input.Split(':');
            Id = int.Parse(parts[0].Replace("Game ", ""));
            GameSets = parts[1].Split(";").Select(setStr =>
            {
                string[] setCount = setStr.Split(",");
                int red = setCount.Where(z => z.EndsWith("red")).Select(z => z.Replace("red", "").Trim()).Select(int.Parse).Sum();
                int green = setCount.Where(z => z.EndsWith("green")).Select(z => z.Replace("green", "").Trim()).Select(int.Parse).Sum();
                int blue = setCount.Where(z => z.EndsWith("blue")).Select(z => z.Replace("blue", "").Trim()).Select(int.Parse).Sum();

                return new GameSet()
                {
                    Red = red,
                    Green = green,
                    Blue = blue
                };
            }).ToList();
        }

        public int TotalRed { get { return GameSets.Select(z => z.Red).Sum(); } }
        public int TotalGreen { get { return GameSets.Select(z => z.Green).Sum(); } }
        public int TotalBlue { get { return GameSets.Select(z => z.Blue).Sum(); } }

        public int MaxRed { get { return GameSets.Select(z => z.Red).Max(); } }
        public int MaxGreen { get { return GameSets.Select(z => z.Green).Max(); } }
        public int MaxBlue { get { return GameSets.Select(z => z.Blue).Max(); } }

        public GameSet MinSet
        { 
            get 
            {
                return new GameSet()
                {
                    Red = MaxRed,
                    Green = MaxGreen,
                    Blue = MaxBlue,
                };
            }
        }



    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";
            var Games = File.ReadAllLines(filename).Select(z => new Game(z)).ToList();

            var part1validGames = Games.Where(game => game.MaxRed <= 12 && game.MaxGreen <= 13 && game.MaxBlue <= 14).ToList();
            var part1Answer = part1validGames.Select(z => z.Id).Sum();


            var part2Answer = Games.Select(game => game.MinSet.Power).Sum();

            Console.WriteLine(part1Answer);
            Console.WriteLine(part2Answer);

            }
    }
}