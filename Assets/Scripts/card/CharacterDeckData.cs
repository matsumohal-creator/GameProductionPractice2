using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Character Deck")]
public class CharacterDeckData : ScriptableObject
{
    public CharacterClass characterClass;

    public List<SkillData> startDeck;
}