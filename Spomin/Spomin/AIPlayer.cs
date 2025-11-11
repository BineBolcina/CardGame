using System;
using System.Collections.Generic;
using System.Linq;

namespace Spomin
{
    public class AIPlayer
    {
        // ime karte -> seznam indeksov (pozicij), ki jih je AI videl
        private Dictionary<string, List<int>> memory = new Dictionary<string, List<int>>();
        private Random rng = new Random();

        // AI si zapomni karto, ko je bila dejansko obrnjena (index je indeks v pictureBoxes/deck)
        public void RememberCard(string name, int index)
        {
            if (!memory.ContainsKey(name))
                memory[name] = new List<int>();

            if (!memory[name].Contains(index))
                memory[name].Add(index);
        }

        // Ko je par ujemajoč, naj AI iz spomina odstrani to ime
        public void RemoveMatchedPair(string name)
        {
            if (memory.ContainsKey(name))
                memory.Remove(name);
        }

        // Poišče par samo, če ima v spominu vsaj 2 različna indeksa za isto ime
        public (int, int)? FindKnownPair(List<Card> deck)
        {
            foreach (var kv in memory)
            {
                var indices = kv.Value.Where(i => !deck[i].IsMatched).ToList();
                if (indices.Count >= 2)
                {
                    // vrnemo prvi dve videni, ki še nista matched
                    return (indices[0], indices[1]);
                }
            }
            return null;
        }

        // Izbira poteze: če pozna par, ga uporabi; sicer naključno izbere dve neobrnani karti
        public (int, int) ChooseMove(List<Card> deck)
        {
            var known = FindKnownPair(deck);
            if (known.HasValue)
                return known.Value;

            List<int> available = new List<int>();
            for (int i = 0; i < deck.Count; i++)
                if (!deck[i].IsMatched && !deck[i].IsFlipped)
                    available.Add(i);

            if (available.Count == 0)
                return (0, 0); // varnostna vrnitev (nič več kart)

            int first = available[rng.Next(available.Count)];
            available.Remove(first);

            int second;
            if (available.Count == 0)
            {
                // če je na voljo samo ena (redko), poiščemo katerokoli ne-matched index (varnost)
                var fallback = Enumerable.Range(0, deck.Count).Where(i => !deck[i].IsMatched && i != first).ToList();
                second = fallback.Count > 0 ? fallback[rng.Next(fallback.Count)] : first;
            }
            else
            {
                second = available[rng.Next(available.Count)];
            }

            return (first, second);
        }
    }
}
