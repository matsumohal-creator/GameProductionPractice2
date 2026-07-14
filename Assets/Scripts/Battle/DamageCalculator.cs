using UnityEngine;

// ここはダメージ計算のロジックをまとめるクラスです
// あくまでも簡易的なものですので必要に応じて拡張してください
// 脱力（Weakness）や弱点（Vulnerable）などのステータス効果も考慮して、最終的なダメージを計算します。

public static class DamageCalculator
{
    // 最終ダメージを計算する
    public static int CalculateDamage(
        int baseDamage,
        PlayerBase attacker,
        IStatusEffectTarget target)
    {
        float damage = Mathf.Max(0, baseDamage);

        // 筋力（Strength）攻撃側のバフ
        if (attacker != null)
        {
            int strength = attacker.GetStatusStack(StatusEffectType.Strength);

            // 筋力1につき攻撃力を1加算
            damage += strength;
        }

        // 脱力（Weakness）攻撃側のデバフ
        if (attacker != null)
        {
            int weakness = attacker.GetStatusStack(StatusEffectType.Weakness);

            // 1層 = 10%減少
            damage *= Mathf.Max(0f, 1f - (weakness * 0.1f));
        }

        // 弱点（Vulnerable）防御側のデバフ
        if (target != null)
        {
            int vulnerable = target.GetStatusStack(StatusEffectType.Vulnerable);

            // 1層 = 10%増加
            damage *= (1f + vulnerable * 0.1f);
        }

        // あとは必要に応じて、他のステータス効果や特性もここで考慮できます。
        return Mathf.Max(0, Mathf.RoundToInt(damage));
    }

    // 敵が攻撃する場合のダメージ計算（オーバーロード）
    public static int CalculateDamage(
        int baseDamage,
        IStatusEffectTarget attacker,
        IStatusEffectTarget target)
    {
        float damage = Mathf.Max(0, baseDamage);

        // 筋力（Strength）攻撃側のバフ
        if (attacker != null)
        {
            int strength = attacker.GetStatusStack(StatusEffectType.Strength);

            // 筋力1につき攻撃力を1加算
            damage += strength;
        }

        // 脱力（Weakness）攻撃側のデバフ
        if (attacker != null)
        {
            int weakness = attacker.GetStatusStack(StatusEffectType.Weakness);

            // 1層 = 10%減少
            damage *= Mathf.Max(0f, 1f - (weakness * 0.1f));
        }

        // 弱点（Vulnerable）防御側のデバフ
        if (target != null)
        {
            int vulnerable = target.GetStatusStack(StatusEffectType.Vulnerable);

            // 1層 = 10%増加
            damage *= (1f + vulnerable * 0.1f);
        }

        return Mathf.Max(0, Mathf.RoundToInt(damage));
    }
}