namespace unlightvbe_kai_core.Models
{
    public class Deck
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required List<Deck_Sub> Deck_Subs { get; set; }
    }
}
