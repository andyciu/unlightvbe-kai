using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Interface
{
    /// <summary>
    /// 技能執行器介面
    /// </summary>
    public interface ISkillAdapter
    {
        /// <summary>
        /// 開始執行階段
        /// </summary>
        /// <param name="stageNum">執行階段號</param>
        /// <param name="startPlayer">開始玩家方</param>
        /// <param name="isAuthMode">是否為驗證模式</param>
        /// <param name="isMulti">是否為雙方玩家執行</param>
        public void StageStart(int stageNum, UserPlayerType startPlayer, bool isAuthMode, bool isMulti);
        /// <summary>
        /// 開始執行階段程序
        /// </summary>
        /// <param name="stageNum">執行階段號</param>
        /// <param name="startPlayer">開始玩家方</param>
        /// <param name="isAuthMode">是否為驗證模式</param>
        /// <param name="isMulti">是否為雙方玩家執行</param>
        /// <param name="stageMessage">執行階段多用途紀錄資訊</param>
        public void StageStart(int stageNum, UserPlayerType startPlayer, bool isAuthMode, bool isMulti, string[]? stageMessage);
        /// <summary>
        /// 開始執行階段程序(指定單方個體)
        /// </summary>
        /// <param name="stageNum">執行階段號</param>
        /// <param name="player">指定玩家方</param>
        /// <param name="characterBattleIndex">指定角色Index</param>
        /// <param name="skillType">指定技能體系</param>
        /// <param name="skillIndex">指定技能Index</param>
        public void StageStartSkillOnly(int stageNum, UserPlayerType player, int characterBattleIndex, SkillType skillType, int skillIndex);
        /// <summary>
        /// 開始執行階段程序(指定單方個體)
        /// </summary>
        /// <param name="stageNum">執行階段號</param>
        /// <param name="player">指定玩家方</param>
        /// <param name="characterBattleIndex">指定角色Index</param>
        /// <param name="skillType">指定技能體系</param>
        /// <param name="skillIndex">指定技能Index</param>
        /// <param name="stageMessage">執行階段多用途紀錄資訊</param>
        public void StageStartSkillOnly(int stageNum, UserPlayerType player, int characterBattleIndex, SkillType skillType, int skillIndex, string[]? stageMessage);
    }
}
