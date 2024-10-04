namespace unlightvbe_kai_core.Models.IUserInterface
{
    public class ShowStartScreenModel
    {
        public int PlayerSelf_Id { get; set; }
        public int PlayerOpponent_Id { get; set; }
        public List<string> PlayerSelf_CharacterVBEID { get; set; }
        public List<string> PlayerOpponent_CharacterVBEID { get; set; }
    }
}
