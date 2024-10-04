namespace unlightvbe_kai_core.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Deck_Sub> Deck_Subs { get; set; }
    }
}
