using UnityEngine;

// 状態異常のデータを定義するクラス
// 状態異常のデータは、状態異常の名前、説明、アイコン、効果の種類、効果の値などを含むクラスです。
// 状態異常のデータは、ScriptableObjectとして実装されており、Unityのエディタ上で簡単に状態異常データを追加・編集することができる
// 例えば、毒の状態異常を定義する場合は、effectNameに「毒」、descriptionに「ターン終了時にダメージを受ける」、
// iconに毒のアイコン、isBuffにfalse、maxStackに3などを設定します。

[CreateAssetMenu(menuName = "Game/StatusEffect")]
public class StatusEffectData : ScriptableObject
{
    public string effectName;

    [TextArea]
    public string description;

    public Sprite icon;
    // 状態異常の効果の種類を定義する列挙型
    public bool isBuff;
    // 状態異常の効果の値。バフの場合は強化量、デバフの場合は弱体化量を設定します。
    public int maxStack;
    
    public StatusEffectType effectType;
}

public class StatusEffect
{
    public StatusEffectData data;

    public int stack;

    public StatusEffect(StatusEffectData data, int amount)
    {
        this.data = data;
        this.stack = amount;
    }
}