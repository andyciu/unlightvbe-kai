using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.Skill;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Enum.StageMessage;
using unlightvbe_kai_core.Models.StageMessage;

namespace unlightvbe_kai_core
{
    public class StageMessageModelFactory
    {
        public static IStageMessageModel<T>? GetModel<T>(string[]? messages) where T : class
        {
            if (messages == null || messages.Length == 0) return null;

            var type = typeof(T);
            return typeof(T) switch
            {
                Type _ when type == typeof(StageMessageModel_46) => (IStageMessageModel<T>)new StageMessageModel_46
                {
                    TargetPlayer = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(messages[0]),
                    TargetCharacterIndex = Convert.ToInt32(messages[1]),
                    DamageType = (CharacterHPDamageType)Convert.ToInt32(messages[2]),
                    DamageValue = Convert.ToInt32(messages[3]),
                    TriggerPlayer = (CommandPlayerRelativeThreeVersionType)Convert.ToInt32(messages[4]),
                    TriggerSkill = (TriggerSkillType)Convert.ToInt32(messages[5])
                },
                Type _ when type == typeof(StageMessageModel_47) => (IStageMessageModel<T>)new StageMessageModel_47
                {
                    DistanceBefore = (CommandPlayerDistanceType)Convert.ToInt32(messages[0]),
                    DistanceAfter = (CommandPlayerDistanceType)Convert.ToInt32(messages[1]),
                    TriggerPlayer = (CommandPlayerRelativeThreeVersionType)Convert.ToInt32(messages[2])
                },
                Type _ when type == typeof(StageMessageModel_48) => (IStageMessageModel<T>)new StageMessageModel_48
                {
                    TargetPlayer = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(messages[0]),
                    TargetCharacterIndex = Convert.ToInt32(messages[1]),
                    HealValue = Convert.ToInt32(messages[2]),
                    TriggerPlayer = (CommandPlayerRelativeThreeVersionType)Convert.ToInt32(messages[3]),
                    TriggerSkill = (TriggerSkillType)Convert.ToInt32(messages[4])
                },
                Type _ when type == typeof(StageMessageModel_62) => (IStageMessageModel<T>)new StageMessageModel_62
                {
                    DiceTotal = [Convert.ToInt32(messages[0]), Convert.ToInt32(messages[1])],
                    DiceTrue = [Convert.ToInt32(messages[2]), Convert.ToInt32(messages[3])],
                    DiceTrueTotal = Convert.ToInt32(messages[4])
                },
                Type _ when type == typeof(StageMessageModel_73) => (IStageMessageModel<T>)new StageMessageModel_73
                {
                    RemoveType = (StageMessage73_RemoveType)Convert.ToInt32(messages[0])
                },
                Type _ when type == typeof(StageMessageModel_76) => (IStageMessageModel<T>)new StageMessageModel_76
                {
                    TargetPlayer = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(messages[0]),
                    SkillType = (SkillType)Convert.ToInt32(messages[1]),
                    Identifier = messages[2]
                },
                Type _ when type == typeof(StageMessageModel_77) => (IStageMessageModel<T>)new StageMessageModel_77
                {
                    TargetPlayer = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(messages[0]),
                    SkillType = (SkillType)Convert.ToInt32(messages[1]),
                    Identifier = messages[2]
                },
                _ => null,
            };
        }
    }
}
