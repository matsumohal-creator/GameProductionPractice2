using UnityEngine;

public class EventCardAddData
{
    public int characterIndex;
    public CharacterClass characterClass;
    public SkillData card;

    public EventCardAddData(
        int characterIndex,
        CharacterClass characterClass,
        SkillData card)
    {
        this.characterIndex = characterIndex;
        this.characterClass = characterClass;
        this.card = card;
    }
}