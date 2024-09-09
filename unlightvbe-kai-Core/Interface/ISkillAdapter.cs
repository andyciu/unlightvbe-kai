using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
