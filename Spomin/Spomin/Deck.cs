using System;
using System.Collections.Generic;
using System.Linq;

namespace Spomin
{
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
            string[] names = { "A_Hearts", "2_Hearts", "3_Hearts", "4_Hearts",
                               "5_Hearts", "6_Hearts", "7_Hearts", "8_Hearts" };

            foreach (var n in names)
            {
                Cards.Add(new Card(n));
                Cards.Add(new Card(n)); // par
            }

            Shuffle();
        }

        public void Shuffle()
        {
            Random rng = new Random();
            Cards = Cards.OrderBy(a => rng.Next()).ToList();
        }
    }
}
