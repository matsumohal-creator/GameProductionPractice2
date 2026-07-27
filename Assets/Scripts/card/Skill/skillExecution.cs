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
            // 状態異常解除
            case SkillEffectType.RemoveStatus:
                ApplyRemoveStatus(target, effect);
                break;
            // シールド付与
            case SkillEffectType.Shield:
                ApplyShield(target, effect);
                break;
            // ドロー
            case SkillEffectType.Draw:
                ApplyDraw(user, effect);
                break;
            // コスト獲得
            case SkillEffectType.CostGain:
                ApplyCostGain(user, effect);
                break;
            // 特殊効果
            case SkillEffectType.Special:
                ApplySpecial(user, target, effect);
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

        if (target is PlayerBase player)
        {
            player.ReceiveDamage(damage, user);
        }
        else
        {
            target.TakeDamage(damage, user);
        }
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

    // 状態異常解除処理
    private static void ApplyRemoveStatus(
    IStatusEffectTarget target,
    SkillEffectData effect)
    {
        if (effect.statusEffect == null)
        {
            return;
        }

        target.RemoveStatusEffect(
            effect.statusEffect.effectType);
    }

    // シールド付与
    private static void ApplyShield(
        IStatusEffectTarget target,
        SkillEffectData effect)
    {
        target.GainShield(effect.value);
    }

    private static void ApplyCostGain(
    PlayerBase user,
    SkillEffectData effect)
    {
        user.GainEnergy(effect.value);
    }

    private static void ApplyDraw(
        PlayerBase user,
        SkillEffectData effect)
    {
        HandManager handManager = Object.FindFirstObjectByType<HandManager>();

        if (handManager == null)
            return;

        handManager.DrawCards(effect.value);
    }

    // 特殊効果の処理
    private static void ApplySpecial(
    PlayerBase user,
    IStatusEffectTarget target,
    SkillEffectData effect)
    {
        switch (effect.specialType)
        {
            case SpecialEffectType.SacrificeStrike:
                ApplySacrificeStrike(user, target);
                break;

            case SpecialEffectType.BloodCharge:
                ApplyBloodCharge(user);
                break;

            case SpecialEffectType.ShieldBash:
                ApplyShieldBash(user, target);
                break;

            case SpecialEffectType.Sharpen_the_thorn:
                ApplySharpenTheThorn(user);
                break;

            case SpecialEffectType.Erosion:
                ApplyErosion(target);
                break;
        }
    }

    // 犠牲の一撃の処理
    private static void ApplySacrificeStrike(
    PlayerBase user,
    IStatusEffectTarget target)
    {
        int bonusDamage = user.ConsumeHpPercent(20);

        if (target is PlayerBase player)
        {
            player.ReceiveDamage(bonusDamage, user);
        }
        else
        {
            target.TakeDamage(bonusDamage, user);
        }
    }

    private static void ApplyBloodCharge(PlayerBase user)
    {
        user.ConsumeHpPercent(50);
        user.GainEnergy(3);

        HandManager handManager = Object.FindFirstObjectByType<HandManager>();

        if (handManager != null)
        {
            handManager.DrawCards(3);
        }
    }

    // シールドバッシュの処理
    private static void ApplyShieldBash(
    PlayerBase user,
    IStatusEffectTarget target)
    {
        int damage = user.Shield;

        if (target is PlayerBase player)
        {
            player.ReceiveDamage(damage, user);
        }
        else
        {
            target.TakeDamage(damage, user);
        }
    }

    // トゲを研ぐの処理
    private static void ApplySharpenTheThorn(
    PlayerBase user)
    {
        if (!user.TryGetStatusEffect(
            StatusEffectType.Thorn,
            out StatusEffectInstance thorns))
        {
            return;
        }

        thorns.stack *= 2;
        // 小数点切り捨ての場合はこれを使用
        //thorns.remainingTurns /= 2;
        // 小数点は切り上げ、この効果で1未満にならないようにする
        thorns.remainingTurns = Mathf.Max(1, thorns.remainingTurns / 2);
    }

    // 侵食の処理
    private static void ApplyErosion(
    IStatusEffectTarget target)
    {
        if (!target.TryGetStatusEffect(
            StatusEffectType.Poison,
            out StatusEffectInstance poison))
        {
            return;
        }

        StatusEffectManager.TriggerStatusEffect(target, poison);
    }
}
