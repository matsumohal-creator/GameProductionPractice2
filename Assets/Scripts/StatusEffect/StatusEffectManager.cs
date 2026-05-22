public static class StatusEffectManager
{
    // 状態異常の種類ごとに、1ターン経過時の効果を適用する
    public static void ApplyTurnTick(IStatusEffectTarget target, StatusEffectInstance instance)
    {
        if (target == null || instance == null || instance.data == null)
        {
            return;
        }

        switch (instance.data.effectType)
        {
            // 毒ダメージ：スタック数分のダメージを受ける
            case StatusEffectType.Poison:
                target.TakeDamage(instance.stack);
                break;

            // 燃焼ダメージ：スタック数の2倍のダメージを受ける
            case StatusEffectType.Burn:
                target.TakeDamage(instance.stack * 2);
                break;

            // 疲労、弱体化、脱力：現在は特殊効果なし（スキルで個別に処理）
            case StatusEffectType.Fatigue:
            case StatusEffectType.Weakness:
            case StatusEffectType.Vulnerable:
                break;
        }
    }
}
