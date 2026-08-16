using System.Collections.Generic;
using UnityEngine;

public class EventEffectManager : MonoBehaviour
{
    public void ApplyEffects(
        List<EventEffectData> effects,
        StageNodeData stage)
    {
        if (effects == null)
        {
            return;
        }

        foreach (EventEffectData effect in effects)
        {
            if (effect == null)
            {
                continue;
            }

            ApplyEffect(effect);
        }
    }

    private void ApplyEffect(EventEffectData effect)
    {
        switch (effect.effectType)
        {
            case EventEffectType.HealParty:
                HealParty(effect.value);
                break;

            case EventEffectType.DamageParty:
                DamageParty(effect.value);
                break;

            case EventEffectType.HealSingle:
                HealSingle(
                    effect.targetCharacterIndex,
                    effect.value);
                break;

            case EventEffectType.DamageSingle:
                DamageSingle(
                    effect.targetCharacterIndex,
                    effect.value);
                break;

            case EventEffectType.AddCard:
                AddCard(effect.card);
                break;

            case EventEffectType.RemoveCard:
                RemoveCard(effect.card);
                break;
        }
    }

    private void HealParty(int amount)
    {
        // 後ほどSaveDataのpartyMembersを使用
    }

    private void DamageParty(int amount)
    {
        // 後ほどSaveDataのpartyMembersを使用
    }

    private void HealSingle(int index, int amount)
    {
        // 後ほど実装
    }

    private void DamageSingle(int index, int amount)
    {
        // 後ほど実装
    }

    private void AddCard(SkillData card)
    {
        // 後ほど実装
    }

    private void RemoveCard(SkillData card)
    {
        // 後ほど実装
    }
}