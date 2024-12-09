namespace unlightvbe_kai_core.Models
{
    public class Player
    {
        public required int PlayerId { get; set; }
        public required string Name { get; set; }
        public required Deck Deck { get; set; }
    }
}
