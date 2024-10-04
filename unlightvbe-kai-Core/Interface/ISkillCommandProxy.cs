using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core.Interface
{
    /// <summary>
    /// 技能執行指令解釋器介面
    /// </summary>
    public interface ISkillCommandProxy
    {
        public void ExecuteCommands(SkillCommandProxyExecueCommandDataModel data, List<SkillCommandModel> commandList);
    }
}
