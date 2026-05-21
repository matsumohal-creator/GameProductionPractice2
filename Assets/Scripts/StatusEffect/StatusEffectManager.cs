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
            case StatusEffectType.Poison:
                target.TakeDamage(instance.stack);
                break;

            case StatusEffectType.Burn:
                target.TakeDamage(instance.stack * 2);
                break;

            case StatusEffectType.Fatigue:
            case StatusEffectType.Weakness:
            case StatusEffectType.Vulnerable:
                break;
        }
    }
}
