using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core.Interface
{
    /// <summary>
    /// 技能執行指令介面
    /// </summary>
    public interface ISkillCommand
    {
        public void SkillLineLight(SkillCommandDataModel data);
        public void SkillTurnOnOff(SkillCommandDataModel data);
        public void SkillLineLightAnother(SkillCommandDataModel data);
        public void SkillTurnOnOffAnother(SkillCommandDataModel data);
        public void SkillAnimateStartPlay(SkillCommandDataModel data);
        public void BattleTurnControl(SkillCommandDataModel data);
        public void BattleSendMessage(SkillCommandDataModel data);
        public void EventTotalDiceChange(SkillCommandDataModel data);
        public void EventPersonAbilityDiceChange(SkillCommandDataModel data);
        public void PersonTotalDiceControl(SkillCommandDataModel data);
        public void AttackTrueDiceControl(SkillCommandDataModel data);
        public void BattleStartDice(SkillCommandDataModel data);
        public void BattleMoveControl(SkillCommandDataModel data);
        public void EventMoveActionOff(SkillCommandDataModel data);
        public void PersonMoveControl(SkillCommandDataModel data);
        public void PersonMoveActionChange(SkillCommandDataModel data);
        public void PersonAttackFirstControl(SkillCommandDataModel data);
        public void PersonBloodControl(SkillCommandDataModel data);
    }
}
