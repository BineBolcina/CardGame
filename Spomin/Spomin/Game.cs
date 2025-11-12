namespace Spomin
{
    public class Game
    {
        public Player Player1 { get; set; }
        public Deck Deck { get; set; }
        public Card FirstSelected { get; set; }
        public int AiScore { get; set; }
        public bool LastMoveMatched { get; set; }

        public Game(string playerName)
        {
            Player1 = new Player(playerName);
            Deck = new Deck();
            AiScore = 0;
            FirstSelected = null;
            LastMoveMatched = false; // začetno stanje
        }

        public bool CheckMatch(Card first, Card second)
        {
            if (first.Name == second.Name)
            {
                first.IsMatched = true;
                second.IsMatched = true;

                // ✅ par najden
                LastMoveMatched = true;
                return true;
            }

            // ❌ ni para
            LastMoveMatched = false;
            return false;
        }
    }
}
