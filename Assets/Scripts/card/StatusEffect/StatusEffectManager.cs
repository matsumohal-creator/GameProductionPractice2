using UnityEngine;

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

            // 燃焼（シールド貫通）
            case StatusEffectType.Burn:
                target.TakeDirectDamage(instance.stack);
                break;

            // 疲労
            case StatusEffectType.Fatigue:
                break;

            // 脱力
            case StatusEffectType.Weakness:
                instance.stack = 0;
                break;

            // 弱点
            case StatusEffectType.Vulnerable:
                instance.stack = 0;
                break;

            // 強化
            case StatusEffectType.Strength:
            break;

            // 再生
            case StatusEffectType.Regeneration:
                // 最大HPの8%回復(小数点以下切り捨て)
                int heal = Mathf.FloorToInt(target.MaxHp * 0.08f);
                target.Heal(heal);
                break;

        }
    }

    // 状態異常の種類ごとに、トリガー時の効果を適用する
    public static void TriggerStatusEffect(
    IStatusEffectTarget target,
    StatusEffectInstance instance)
    {
        if (target == null || instance == null || instance.data == null)
        {
            return;
        }

        switch (instance.data.effectType)
        {
            case StatusEffectType.Poison:
                target.TakeDamage(instance.stack);
                instance.remainingTurns--;
                break;

            case StatusEffectType.Burn:
                target.TakeDirectDamage(instance.stack);
                instance.stack--;
                break;
        }
    }
}
