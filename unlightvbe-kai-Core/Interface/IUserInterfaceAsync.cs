using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_core.Interface
{
    public interface IUserInterfaceAsync : IUserInterface
    {
        public Task ShowStartScreenAsync(ShowStartScreenModel data);
        public Task ShowBattleMessageAsync(string message);
        public Task DrawActionCardAsync(DrawCardModel data);
        public Task DrawEventCardAsync(DrawCardModel data);
        public Task UpdateDataAsync(UpdateDataModel data);
        public Task UpdateDataMultiAsync(UpdateDataMultiModel data);
        public Task UpdateDataRelativeAsync(UpdateDataRelativeModel data);
        public Task ReadActionReceiveAsync(ReadActionReceiveModel data);
        public Task PhaseStartAsync(PhaseStartModel data);
        public Task OpenOppenentPlayingCardAsync(OpenOppenentPlayingCardModel data);
        public Task ShowJudgmentAsync(ShowJudgmentModel data);
        public Task ShowSkillAnimateAsync(ShowSkillAnimateModel data);
    }
}
