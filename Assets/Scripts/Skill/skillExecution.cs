using UnityEngine;
using System.Collections.Generic;
// スキルの実行処理を担当するクラス
// スキルの効果をターゲットに適用するための静的クラス

public static class SkillExecution
{
    // スキルを実行するためのメイン関数
    public static void ExecuteSkill(
        PlayerBase user,
        IEnumerable<IStatusEffectTarget> targets,
        SkillData skill)
    {
        if (user == null || targets == null || skill == null)
        {
            return;
        }

        // コスト不足
        if (!user.TryUseEnergy(skill.cost))
        {
            return;
        }
        // ターゲットに対してスキルの効果を適用
        foreach (IStatusEffectTarget target in targets)
        {
            if (target == null) continue;
            // スキルの各効果をターゲットに適用
            foreach (SkillEffectData effect in skill.effects)
            {
                ApplyEffect(user, target, effect);
            }
        }
    }
    // スキル効果をターゲットに適用する関数
    private static void ApplyEffect(
        PlayerBase user,
        IStatusEffectTarget target,
        SkillEffectData effect)
    {
        switch (effect.effectType)
        {
            // ダメージ
            case SkillEffectType.Damage:
                ApplyDamage(user, target, effect);
                break;
            // 回復
            case SkillEffectType.Heal:
                ApplyHeal(target, effect);
                break;
            // 状態異常付与
            case SkillEffectType.ApplyStatus:
                ApplyStatus(target, effect);
                break;
            // シールド付与
            case SkillEffectType.Shield:
                ApplyShield(target, effect);
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

    // シールド付与
    private static void ApplyShield(
        IStatusEffectTarget target,
        SkillEffectData effect)
    {
        target.GainShield(effect.value);
    }
}
