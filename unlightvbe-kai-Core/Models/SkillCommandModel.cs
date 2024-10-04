using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models
{
    public class SkillCommandModel
    {
        public SkillCommandType Type { get; set; }
        public string[]? Message { get; set; }

        public SkillCommandModel(SkillCommandType type, params string[]? message)
        {
            Type = type;
            Message = message;
        }
    }
}
