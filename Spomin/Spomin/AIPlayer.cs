using System;
using System.Collections.Generic;
using System.Linq;

namespace Spomin
{
    public class AIPlayer
    {
        // Spomin AI-ja: ime karte -> seznam indeksov, kjer je karta videna
        private Dictionary<string, List<int>> memory = new Dictionary<string, List<int>>();
        private Random random = new Random();

        /// <summary>
        /// Shrani karto, ki jo je AI videl.
        /// </summary>
        public void RememberCard(string name, int index)
        {
            if (!memory.ContainsKey(name))
                memory[name] = new List<int>();

            if (!memory[name].Contains(index))
                memory[name].Add(index);
        }

        /// <summary>
        /// Odstrani par iz spomina, ko je bil najden.
        /// </summary>
        public void RemoveMatchedPair(string name)
        {
            if (memory.ContainsKey(name))
                memory.Remove(name);
        }

        public (int, int) ChooseMove(List<Card> allCards)
        {
            // 1️⃣ Preveri, če ima znan par v spominu
            foreach (var entry in memory)
            {
                var indexes = entry.Value.Where(i => !allCards[i].IsMatched).ToList();
                if (indexes.Count >= 2)
                    return (indexes[0], indexes[1]); // znan par
            }

            // 2️ Če ni znanih parov → obrni eno naključno karto
            var availableIndexes = Enumerable.Range(0, allCards.Count)
                .Where(i => !allCards[i].IsMatched)
                .ToList();

            if (availableIndexes.Count < 2)
                return (0, 0); // fail-safe

            // Izberi prvo karto naključno
            int first = availableIndexes[random.Next(availableIndexes.Count)];
            string firstName = allCards[first].Name;

            int second; // <- deklariraj samo enkrat

            // Preveri, če AI že pozna par za to karto
            if (memory.ContainsKey(firstName))
            {
                var candidates = memory[firstName]
                    .Where(i => i != first && !allCards[i].IsMatched)
                    .ToList();
                if (candidates.Count > 0)
                {
                    second = candidates[0];
                    return (first, second);
                }
            }

            // Če ni znanega para, izberi drugo naključno karto
            do
            {
                second = availableIndexes[random.Next(availableIndexes.Count)];
            } while (second == first);

            return (first, second);
        }

    }
}
