using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 技能執行指令解釋器類別
        /// </summary>
        protected class SkillCommandProxyClass : ISkillCommand, ISkillCommandProxy
        {
            private BattleSystem BattleSystem;
            private PlayerData[] playerDatas;
            private ISkillAdapter skillAdapter;

            public SkillCommandProxyClass(BattleSystem battleSystem)
            {
                BattleSystem = battleSystem;
                playerDatas = battleSystem.PlayerDatas;
            }

            public void SetSkillAdapter(ISkillAdapter skillAdapter)
            {
                this.skillAdapter = skillAdapter;
            }

            /// <summary>
            /// 執行指令呼叫執行
            /// </summary>
            /// <param name="commandList">指令集合</param>
            public void ExecuteCommands(int stageNum, UserPlayerType player, SkillType skillType, List<SkillCommandModel> commandList)
            {
                foreach (SkillCommandModel command in commandList)
                {
                    var method = this.GetType().GetMethod(command.Type.ToString());
                    if (method != null)
                    {
                        var data = new SkillCommandDataModel
                        {
                            Player = player,
                            SkillType = skillType,
                            StageNum = stageNum,
                            Message = command.Message
                        };
                        method.Invoke(this, new object[] { data });
                    }
                }
            }

            public void AtkingLineLight(SkillCommandDataModel data)
            {
                throw new NotImplementedException();
            }
        }
    }
}
