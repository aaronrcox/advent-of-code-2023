namespace Day_04
{

    class Card
    {
        public int Id { get; set; }
        public List<int> Numbers { get; set; } = new List<int>();
        public List<int> Winning { get; set; } = new List<int>();

        public Card(string input)
        {
            string[] parts = input.Split(':');
            Id = int.Parse(parts[0].Replace("Card ", ""));

            string[] numberStrings = parts[1].Split("|");
            Winning = numberStrings[0].Trim().Split(" ").Where(z => !string.IsNullOrWhiteSpace(z)).Select(int.Parse).ToList();
            Numbers = numberStrings[1].Trim().Split(" ").Where(z => !string.IsNullOrWhiteSpace(z)).Select(int.Parse).ToList();
        }

        public Card(int id, List<int> numbers, List<int> winning)
        {
            Id = id;
            Numbers = numbers;
            Winning = winning;
        }

        public int MatchCount
        {
            get
            {
                return Numbers.Count(n => Winning.Contains(n));
            }
        }

        public int Points
        {
            get
            {
                if (MatchCount == 0) return 0;
                if (MatchCount == 1) return 1;
                int score = 1;
                for (int i = 1; i < MatchCount; i++) score *= 2;
                return score;
            }
        }

        public static List<Card> CalculateWinningCopies(List<Card> origionalCards)
        {
            List<Card> cardsToProcess = origionalCards.ToList();
            List<Card> cards = new List<Card>();
            
            while(cardsToProcess.Count > 0)
            {
                Card card = cardsToProcess.First();
                cardsToProcess.Remove(card);
                cards.Add(card);

                for (int i=0; i<card.MatchCount; i++)
                {
                    Card newCard = new Card(origionalCards[card.Id + i].Id, origionalCards[card.Id + i].Numbers, origionalCards[card.Id + i].Winning);
                    cardsToProcess.Add(newCard);
                }
            }

            return cards;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";
            var cards = File.ReadAllLines(filename).Select(line => new Card(line)).ToList();
            

            int part1Answer = cards.Select(z => z.Points).Sum();
            Console.WriteLine($"Part 1: {part1Answer}");


            var allCards = Card.CalculateWinningCopies(cards);
            int part2Answer = allCards.Count;
            Console.WriteLine($"Part 2: {part2Answer}");

        }
    }
}