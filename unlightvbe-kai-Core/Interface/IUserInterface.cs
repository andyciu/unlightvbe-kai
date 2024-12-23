using unlightvbe_kai_core.Models.UserInterface;

namespace unlightvbe_kai_core.Interface
{
    public interface IUserInterface
    {
        public void ShowStartScreen(ShowStartScreenModel data);
        public void ShowBattleMessage(string message);
        public ReadActionModel ReadAction();
        public void DrawActionCard(DrawCardModel data);
        public void DrawEventCard(DrawCardModel data);
        public void UpdateData(UpdateDataModel data);
        public void UpdateDataMulti(UpdateDataMultiModel data);
        public void UpdateDataRelative(UpdateDataRelativeModel data);
        public void ReadActionReceive(ReadActionReceiveModel data);
        public void PhaseStart(PhaseStartModel data);
        public void OpenOppenentPlayingCard(OpenOppenentPlayingCardModel data);
        public ChangeCharacterActionModel ChangeCharacterAction();
        public void ShowJudgment(ShowJudgmentModel data);
        public void ShowSkillAnimate(ShowSkillAnimateModel data);
        public void BuffDataSet(BuffDataSetModel data);
        public void BuffDataUpdate(BuffDataUpdateModel data);
        public void BuffDataRemove(BuffDataRemoveModel data);
        public void ShowDice(ShowDiceModel data);
    }
}
