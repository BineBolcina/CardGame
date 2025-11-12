using System;
using System.Collections.Generic;

namespace Spomin
{
    public class Game
    {
        public Deck Deck { get; private set; }
        public Player Player1 { get; private set; }
        public int AiScore { get; set; }  // dodamo AI score
        public Card FirstSelected { get; set; }

        public Game(string playerName)
        {
            Player1 = new Player(playerName);
            AiScore = 0;  // začnemo pri 0
            Deck = new Deck();
            Deck.Shuffle();
        }

        // preveri, če sta dve karti ujemajoči
        public bool CheckMatch(Card card1, Card card2)
        {
            if (card1.Name == card2.Name)
            {
                card1.IsMatched = true;
                card2.IsMatched = true;
                return true;
            }
            return false;
        }
    }
}
