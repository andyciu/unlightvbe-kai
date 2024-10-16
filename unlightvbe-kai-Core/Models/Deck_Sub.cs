namespace unlightvbe_kai_core.Models
{
    public class Deck_Sub
    {
        public required Character Character { get; set; }
        public List<EventCard>? EventCards { get; set; }
    }
}
