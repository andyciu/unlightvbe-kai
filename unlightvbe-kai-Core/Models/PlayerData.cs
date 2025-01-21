using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.Skill;
using unlightvbe_kai_core.Models.SkillCommand;

namespace unlightvbe_kai_core.Models
{
    public class PlayerData
    {
        public Player Player { get; }
        public List<CharacterData> CharacterDatas { get; set; }
        public CharacterData CurrentCharacter => CharacterDatas[0];
        /// <summary>
        /// 目前卡牌最大數量上限
        /// </summary>
        public int HoldMaxCount { get; set; }
        public MoveBarSelectType MoveBarSelect { get; set; }
        public bool IsOKButtonSelect { get; set; }
        public UserPlayerType PlayerType { get; }
        public int DiceTotal { get; set; }
        /// <summary>
        /// 執行指令-攻擊/防禦階段系統骰數變化量控制紀錄
        /// </summary>
        /// <remarks>
        /// Record: <br/>
        /// <list type="number">
        ///     <item>
        ///         <term>AssignMode(是否進入指定模式)</term>
        ///         <description>bool</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public PropertyWithRecord<Dictionary<UserPlayerRelativeType, Dictionary<SkillType, List<EventTotalDiceChangeRecordModel>>>, bool> SC_EventTotalDiceChangeRecord { get; set; } = new();
        /// <summary>
        /// 執行指令-攻擊/防禦階段角色白值能力對骰數變化量控制紀錄
        /// </summary>
        /// <remarks>
        /// Record: <br/>
        /// <list type="number">
        ///     <item>
        ///         <term>AssignMode(是否進入指定模式)</term>
        ///         <description>bool</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public PropertyWithRecord<Dictionary<UserPlayerRelativeType, Dictionary<SkillType, List<EventPersonAbilityDiceChangeRecordModel>>>, bool> SC_EventPersonAbilityDiceChangeRecord { get; set; } = new();
        /// <summary>
        /// 執行指令-人物移動階段總移動量控制紀錄
        /// </summary>
        public Dictionary<UserPlayerRelativeType, List<PersonMoveControlRecordModel>> SC_PersonMoveControlRecord { get; set; } = [];
        private readonly Dictionary<string, CharacterData> CharacterVBEIDDict = [];
        public PlayerData(Player player, UserPlayerType playerType)
        {
            Player = player;
            PlayerType = playerType;

            CharacterDatas = [];
            foreach (var deck_Sub in player.Deck.Deck_Subs)
            {
                var newData = new CharacterData(new(deck_Sub.Character));
                CharacterDatas.Add(newData);
                CharacterVBEIDDict.Add(deck_Sub.Character.VBEID, newData);
            }

            foreach (var userPlayerRelativeType in System.Enum.GetValues<UserPlayerRelativeType>())
            {
                SC_EventTotalDiceChangeRecord.MainProperty.Add(userPlayerRelativeType, []);
                SC_EventPersonAbilityDiceChangeRecord.MainProperty.Add(userPlayerRelativeType, []);
                SC_PersonMoveControlRecord.Add(userPlayerRelativeType, new());
                foreach (var skillType in System.Enum.GetValues<SkillType>())
                {
                    SC_EventTotalDiceChangeRecord.MainProperty[userPlayerRelativeType].Add(skillType, []);
                    SC_EventPersonAbilityDiceChangeRecord.MainProperty[userPlayerRelativeType].Add(skillType, []);
                }
            }
        }

        public CharacterData? GetCharacterData(string characterVBEID)
        {
            CharacterData? characterData;
            if (CharacterVBEIDDict.TryGetValue(characterVBEID, out characterData))
            {
                return characterData;
            }
            return null;
        }

        public int? GetCharacterDataIndex(string characterVBEID)
        {
            var result = CharacterDatas.FindIndex(x => x.Character.VBEID == characterVBEID);
            if (result == -1) return null;
            else return result;
        }
    }
}
