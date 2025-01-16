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
        public void EventBloodActionOff(SkillCommandDataModel data);
        public void EventBloodActionChange(SkillCommandDataModel data);
        public void EventBloodReflection(SkillCommandDataModel data);
        public void EventHealActionOff(SkillCommandDataModel data);
        public void EventHealActionChange(SkillCommandDataModel data);
        public void EventHealReflection(SkillCommandDataModel data);
        public void PersonAddBuff(SkillCommandDataModel data);
        public void BuffTurnEnd(SkillCommandDataModel data);
        public void BuffEnd(SkillCommandDataModel data);
        public void EventRemoveBuffActionOff(SkillCommandDataModel data);
        public void PersonRemoveBuffAll(SkillCommandDataModel data);
        public void PersonRemoveBuffSelect(SkillCommandDataModel data);
        public void PersonBuffTurnChange(SkillCommandDataModel data);
    }
}
