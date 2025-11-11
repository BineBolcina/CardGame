namespace Spomin
{
    public class Card
    {
        public string Name { get; set; }       // ime karte, npr. "A_Hearts"
        public bool IsFlipped { get; set; } = false;
        public bool IsMatched { get; set; } = false;

        public Card(string name)
        {
            Name = name;
        }
    }
}
