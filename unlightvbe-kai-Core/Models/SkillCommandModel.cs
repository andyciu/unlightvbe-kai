using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models
{
    public class SkillCommandModel(SkillCommandType type, params string[]? message)
    {
        public SkillCommandType Type { get; set; } = type;
        public string[]? Message { get; set; } = message;
    }
}
