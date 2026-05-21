using UnityEngine;

// スキルの実行処理を担当するクラス
// スキルの効果をターゲットに適用するための静的クラス

public static class SkillExecution
{
    public static void ExecuteSkill(
            PlayerBase user,
            IStatusEffectTarget target,
            SkillData skill)
    {
        if (user == null || target == null || skill == null)
        {
            return;
        }

        // コスト不足
        if (!user.TryUseEnergy(skill.cost))
        {
            return;
        }

        foreach (SkillEffectData effect in skill.effects)
        {
            ApplyEffect(user, target, effect);
        }
    }

    private static void ApplyEffect(
        PlayerBase user,
        IStatusEffectTarget target,
        SkillEffectData effect)
    {
        switch (effect.effectType)
        {
            case SkillEffectType.Damage:
                ApplyDamage(user, target, effect);
                break;

            case SkillEffectType.Heal:
                ApplyHeal(target, effect);
                break;

            case SkillEffectType.ApplyStatus:
                ApplyStatus(target, effect);
                break;
        }
    }

    // ダメージ処理
    private static void ApplyDamage(
        PlayerBase user,
        IStatusEffectTarget target,
        SkillEffectData effect)
    {
        int damage = DamageCalculator.CalculateDamage(
            effect.value,
            user,
            target);

        target.TakeDamage(damage);
    }

    // 回復処理
    private static void ApplyHeal(
        IStatusEffectTarget target,
        SkillEffectData effect)
    {
        target.Heal(effect.value);
    }

    // 状態異常付与処理
    private static void ApplyStatus(
        IStatusEffectTarget target,
        SkillEffectData effect)
    {
        if (effect.statusEffect == null)
        {
            return;
        }

        target.ApplyStatusEffect(
            effect.statusEffect,
            effect.duration,
            effect.stack);
    }
}
