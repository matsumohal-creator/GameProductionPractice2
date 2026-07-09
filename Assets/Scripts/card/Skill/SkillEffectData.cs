using UnityEngine;

// スキルの効果を定義するクラス
// スキルの効果は、ダメージ、回復、ステータス異常など様々な種類があるため、
// それらを一つのクラスで表現するために、SkillEffectTypeを使用して効果の種類を区別します。
// 例えば、ダメージ効果の場合はvalueにダメージ量を設定し、回復効果の場合はvalueに回復量を設定します。

[System.Serializable]
public class SkillEffectData
{
    public SkillEffectType effectType;
    [Header("Special")]
    public SpecialEffectType specialType;
    // 効果の値。ダメージ量、回復量、シールド量など、効果の種類に応じた値を設定します。
    // そのため、effectTypeによってvalueの値が変わります。
    [Header("Value")]
    public int value;
    // 状態異常効果の場合、statusEffectに状態異常のデータを設定します。
    // 例えば、毒の状態異常を付与する効果の場合は、statusEffectに毒のStatusEffectDataを設定します。
    [Header("Status")]
    public StatusEffectData statusEffect;
    // 効果の持続ターン数。ダメージや回復などの即時効果の場合は0に設定します。
    [Header("Duration")]
    public int duration; // 継続ターン数
    public int stack; //効果量
}