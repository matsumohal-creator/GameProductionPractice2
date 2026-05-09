using UnityEngine;
using System.Collections.Generic;

// スキルデータを定義するクラス
// スキルデータは、スキルの基本情報、コスト、カテゴリ、ターゲットタイプ、効果、専用クラスなどを含むクラスです。
// スキルデータは、ScriptableObjectとして実装されており、Unityのエディタ上で簡単にスキルデータを追加・編集することができます。

[CreateAssetMenu(menuName = "Game/Skill")]
public class SkillData : ScriptableObject
{
    [Header("スキルの名前")]
    public string skillName;

    [TextArea]//スキル説明
    public string description;

    public Sprite icon;

    [Header("Cost")]
    public int cost; // 個人コスト

    [Header("対象")]
    public SkillTargetType targetType;// ターゲットの種類（単体、全体、ランダムなど）

    [Header("Effects")]// スキルの効果のリスト。スキルは複数の効果を持つことができるため、Listで管理します。
    public List<SkillEffectData> effects;

    // 専用クラス。スキルが特定のクラスにのみ使用可能な場合、そのクラスを指定します。
    // nullの場合は全てのクラスで使用可能です。
    [Header("スキルの専用クラス")]
    public CharacterClass exclusiveClass; 
}