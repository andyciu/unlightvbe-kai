namespace unlightvbe_kai_core.Models.StageMessage
{
    public record StageMessageModel_62 : IStageMessageModel<StageMessageModel_62>
    {
        public required int[] DiceTotal { get; init; }
        public required int[] DiceTrue { get; init; }
        public required int DiceTrueTotal { get; init; }
    }
}
