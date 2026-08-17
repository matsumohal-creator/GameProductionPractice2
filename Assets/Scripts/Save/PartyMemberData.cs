using System.Collections.Generic;

[System.Serializable]
public class PartyMemberData
{
    // キャラクターの識別用
    public int characterIndex;

    // キャラクタークラス
    public CharacterClass characterClass;

    // 現在HP
    public int currentHp;

    // 最大HP
    public int maxHp;

    // デッキ
    public List<SkillData> deck = new();
}