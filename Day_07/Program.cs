using System.Runtime.InteropServices;

namespace Day_07
{
    public enum ERank
    {
        FIVE_OF_A_KIND = 7,
        FOUR_OF_A_KIND = 6,
        FULL_HOUSE = 5,
        THREE_OF_A_KIND = 4,
        TWO_PAIR = 3,
        ONE_PAIR = 2,
        HIGH_CARD = 1,
        UNKNOWN = 0,
    }

    public class Hand_Part1
    {
        public string Cards { get; set; }
        public string SortableCards { get; set; }
        public int Bid { get; set; }

        public ERank Strength
        {
            get
            {
                var cardGroups = SortableCards.ToCharArray().GroupBy(x => x).Select(group => new { Value = group.Key, Count = group.Count() }).ToList();

                if (cardGroups.Count == 1)
                    return ERank.FIVE_OF_A_KIND;

                if (cardGroups.Count == 2 && cardGroups.Any(g => g.Count == 1) && cardGroups.Any(g => g.Count == 4))
                    return ERank.FOUR_OF_A_KIND;

                if (cardGroups.Count == 2 && cardGroups.Any(g => g.Count == 2) && cardGroups.Any(g => g.Count == 3))
                    return ERank.FULL_HOUSE;

                if (cardGroups.Count == 3 && cardGroups.Any(g => g.Count == 3))
                    return ERank.THREE_OF_A_KIND;

                if (cardGroups.Count == 3 && cardGroups.Where(g => g.Count == 2).Count() == 2)
                    return ERank.TWO_PAIR;

                if (cardGroups.Count == 4 && cardGroups.Any(g => g.Count == 2))
                    return ERank.ONE_PAIR;

                if (cardGroups.Count == 5)
                    return ERank.HIGH_CARD;

                return ERank.UNKNOWN;
            }
        }

        public Hand_Part1(string input)
        {
            string[] parts = input.Split(" ");
            Cards = parts[0].Trim();
            Bid = int.Parse(parts[1].Trim());

            SortableCards = new string(Cards.Select(card =>
            {
                return card switch
                {
                    '2' => 'A',
                    '3' => 'B',
                    '4' => 'C',
                    '5' => 'D',
                    '6' => 'E',
                    '7' => 'F',
                    '8' => 'G',
                    '9' => 'H',
                    'T' => 'I',
                    'J' => 'J',
                    'Q' => 'K',
                    'K' => 'L',
                    'A' => 'M',
                    _ => throw new InvalidDataException()
                };
            }).ToArray());

        }
    }

    public class Hand_Part2
    {
        public string Cards { get; set; }
        public string SortableCards { get; set; }
        public int Bid { get; set; }

        public ERank Strength
        {
            get
            {
                int wildCount = SortableCards.Where(c => c == 'A').Count(); // A == J in the sortable list
                var cardGroups = SortableCards.ToCharArray()
                    .Where(z => z != 'A') // Filter out wilds before grouping
                    .GroupBy(x => x).Select(group => new { Value = group.Key, Count = group.Count() }).ToList();

                if(wildCount == 5)
                {
                    cardGroups.Add(new { Value = 'A', Count = 5 });
                }

                // disperse the wild into the groups
                char[] newSortableCardsArr = SortableCards.ToCharArray();
                for(int i=0; i<wildCount; i++)
                {
                    var bestCard = cardGroups.OrderByDescending(g => g.Count).ThenByDescending(g => g.Value).First().Value;
                    int index = Array.IndexOf(newSortableCardsArr, 'A');
                    newSortableCardsArr[index] = bestCard;
                }

                // regroup
                cardGroups = newSortableCardsArr
                    .GroupBy(x => x).Select(group => new { Value = group.Key, Count = group.Count() }).ToList();



                if (cardGroups.Count == 1)
                    return ERank.FIVE_OF_A_KIND;

                if (cardGroups.Count == 2 && cardGroups.Any(g => g.Count == 1) && cardGroups.Any(g => g.Count == 4))
                    return ERank.FOUR_OF_A_KIND;

                if (cardGroups.Count == 2 && cardGroups.Any(g => g.Count == 2) && cardGroups.Any(g => g.Count == 3))
                    return ERank.FULL_HOUSE;

                if (cardGroups.Count == 3 && cardGroups.Any(g => g.Count == 3))
                    return ERank.THREE_OF_A_KIND;

                if (cardGroups.Count == 3 && cardGroups.Where(g => g.Count == 2).Count() == 2)
                    return ERank.TWO_PAIR;

                if (cardGroups.Count == 4 && cardGroups.Any(g => g.Count == 2))
                    return ERank.ONE_PAIR;

                if (cardGroups.Count == 5)
                    return ERank.HIGH_CARD;

                return ERank.UNKNOWN;
            }
        }

        public Hand_Part2(string input)
        {
            string[] parts = input.Split(" ");
            Cards = parts[0].Trim();
            Bid = int.Parse(parts[1].Trim());

            SortableCards = new string(Cards.Select(card =>
            {
                return card switch
                {
                    'J' => 'A',
                    '2' => 'B',
                    '3' => 'C',
                    '4' => 'D',
                    '5' => 'E',
                    '6' => 'F',
                    '7' => 'G',
                    '8' => 'H',
                    '9' => 'I',
                    'T' => 'J',
                    'Q' => 'K',
                    'K' => 'L',
                    'A' => 'M',
                    _ => throw new InvalidDataException()
                };
            }).ToArray());

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";
            var lines = File.ReadAllLines(filename);

            {
                var hands = lines.Select(line => new Hand_Part1(line));

                var sortedHands = hands.OrderBy(h => h.Strength).ThenBy(h => h.SortableCards);
                int answer = sortedHands.Select((hand, index) => hand.Bid * (index + 1)).Sum();

                Console.WriteLine($"Part 1: {answer}");
            }


            {
                var hands = lines.Select(line => new Hand_Part2(line));

                var sortedHands = hands.OrderBy(h => h.Strength).ThenBy(h => h.SortableCards);
                int answer = sortedHands.Select((hand, index) => hand.Bid * (index + 1)).Sum();

                Console.WriteLine($"Part 2: {answer}");
            }
            


            

        }
    }
}