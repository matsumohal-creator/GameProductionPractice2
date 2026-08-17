using UnityEngine;

[System.Serializable]
public class EventEffectData
{
    [Header("効果")]
    public EventEffectType effectType;

    [Header("数値")]
    public int value;

    [Header("対象キャラクター")]
    public int targetCharacterIndex = -1;
}