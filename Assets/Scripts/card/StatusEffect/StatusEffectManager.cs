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
                instance.stack--;
                break;

            // 燃焼（シールド貫通）
            case StatusEffectType.Burn:
                target.TakeDirectDamage(instance.stack);
                instance.stack--;
                break;

            // 疲労
            case StatusEffectType.Fatigue:
                instance.stack--;
                break;

            // 脱力
            case StatusEffectType.Weakness:
                instance.stack = 0;
                break;

            // 弱点
            case StatusEffectType.Vulnerable:
                instance.stack = 0;
                break;
        }
    }
}
