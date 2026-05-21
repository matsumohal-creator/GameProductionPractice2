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
}