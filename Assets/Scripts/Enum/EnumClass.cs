using UnityEngine;

// ゲーム内で使用する様々な列挙型を定義するクラス

// キャラクターのクラスを定義する列挙型
public enum CharacterClass
{
    None,    // クラスなし（全てのクラスで使用可能なスキルに使用）
    Fighter, // 近接攻撃に優れたクラス
    Healer,  // 回復やサポートに特化したクラス
    Knight,  // 防御力が高く、盾を使うクラス
    Mage,    // 魔法攻撃に優れたクラス
    Gambler, // 運に頼るスキルを持つクラス
    Playboy  // プレイボーイ（様々なスキルを扱うクラス）
}

// スキルのターゲットタイプを定義する列挙型
public enum SkillTargetType
{
    EnemySingle,  // 単体の敵を対象とするスキル
    EnemyAll,     // 全体の敵を対象とするスキル
    AllySingle,   // 単体の味方を対象とするスキル
    AllyAll,      // 全体の味方を対象とするスキル
    Self          // 自分自身を対象とするスキル
}

// スキルの効果タイプを定義する列挙型
public enum SkillEffectType
{
    Damage,      // ダメージを与える効果
    Heal,        // HPを回復する効果
    Shield,      // ダメージを軽減する効果
    ApplyStatus, // 状態異常を付与する効果
    RemoveStatus,// 状態異常を解除する効果
    Draw,        // カードを引く効果
    CostGain,    // コストを増加させる効果
    Special      // 特殊な効果（例: 特定の条件で発動する効果）
}

public enum StatusEffectType
{
    Poison,    //  毒
    Burn,      // 燃焼
    Fatigue,   // 疲労
    Weakness,  // 弱点
    Vulnerable,// 脱力
    Strength   // 筋力（攻撃力上昇バフ）
}
