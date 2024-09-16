using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models
{
    public class Skill<T> where T : class
    {
        /// <summary>
        /// 技能名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 技能唯一識別碼
        /// </summary>
        public string SkillID { get; }
        /// <summary>
        /// 欲使用之執行階段號
        /// </summary>
        public List<int> StageNumber { get; set; } = new();
        /// <summary>
        /// 回合階段
        /// </summary>
        public PhaseType Phase { get; set; }
        /// <summary>
        /// 距離
        /// </summary>
        public List<PlayerDistanceType> Distance { get; set; } = new();
        /// <summary>
        /// 卡片條件集合
        /// </summary>
        public List<SkillCardConditionModel> Cards { get; set; } = new();
        public T Function { get; }
        public Skill(T function, string vbeID)
        {
            Function = function;
            SkillID = vbeID;
        }
    }
}
