using System.Collections.Generic;
using UnityEngine;

public class EventEffectManager : MonoBehaviour
{
    [Header("スキルデータベース")]
    [SerializeField]
    private SkillDatabase skillDatabase;

    // =========================================================
    // 外部からイベント効果を実行
    // =========================================================

    public bool ApplyEffects(
        List<EventEffectData> effects,
        StageNodeData stage)
    {
        if (effects == null)
        {
            return false;
        }

        bool requiresCardRemoval = false;

        foreach (EventEffectData effect in effects)
        {
            if (effect == null)
            {
                continue;
            }

            if (effect.effectType == EventEffectType.RemoveCard)
            {
                // カード削除はUIから行う
                requiresCardRemoval = true;
                continue;
            }

            ApplyEffect(effect);
        }

        return requiresCardRemoval;
    }

    // =========================================================
    // 効果の種類ごとの処理
    // =========================================================

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
                AddRandomCardToParty();
                break;
        }
    }

    // =========================================================
    // パーティ取得
    // =========================================================

    private List<PartyMemberData> GetPartyMembers()
    {
        if (SaveManager.CurrentSave == null)
        {
            Debug.LogWarning("SaveDataが存在しません");
            return null;
        }

        if (SaveManager.CurrentSave.partyMembers == null)
        {
            Debug.LogWarning("partyMembersが存在しません");
            return null;
        }

        return SaveManager.CurrentSave.partyMembers;
    }

    // =========================================================
    // HP回復
    // =========================================================

    private void HealParty(int amount)
    {
        List<PartyMemberData> party = GetPartyMembers();

        if (party == null)
        {
            return;
        }

        amount = Mathf.Max(0, amount);

        foreach (PartyMemberData member in party)
        {
            if (member == null)
            {
                continue;
            }

            member.currentHp = Mathf.Min(
                member.currentHp + amount,
                member.maxHp);
        }

        Debug.Log(
            $"[EventEffect] パーティ全体を {amount} 回復");
    }

    // =========================================================
    // パーティ全体ダメージ
    // =========================================================

    private void DamageParty(int amount)
    {
        List<PartyMemberData> party = GetPartyMembers();

        if (party == null)
        {
            return;
        }

        amount = Mathf.Max(0, amount);

        foreach (PartyMemberData member in party)
        {
            if (member == null)
            {
                continue;
            }

            member.currentHp = Mathf.Max(
                0,
                member.currentHp - amount);
        }

        Debug.Log(
            $"[EventEffect] パーティ全体に {amount} ダメージ");
    }

    // =========================================================
    // 単体回復
    // =========================================================

    private void HealSingle(int index, int amount)
    {
        PartyMemberData member = GetPartyMember(index);

        if (member == null)
        {
            return;
        }

        amount = Mathf.Max(0, amount);

        member.currentHp = Mathf.Min(
            member.currentHp + amount,
            member.maxHp);

        Debug.Log(
            $"[EventEffect] {index}番キャラクターを {amount} 回復");
    }

    // =========================================================
    // 単体ダメージ
    // =========================================================

    private void DamageSingle(int index, int amount)
    {
        PartyMemberData member = GetPartyMember(index);

        if (member == null)
        {
            return;
        }

        amount = Mathf.Max(0, amount);

        member.currentHp = Mathf.Max(
            0,
            member.currentHp - amount);

        Debug.Log(
            $"[EventEffect] {index}番キャラクターに {amount} ダメージ");
    }

    // =========================================================
    // 指定キャラクター取得
    // =========================================================

    private PartyMemberData GetPartyMember(int characterIndex)
    {
        List<PartyMemberData> party = GetPartyMembers();

        if (party == null)
        {
            return null;
        }

        foreach (PartyMemberData member in party)
        {
            if (member == null)
            {
                continue;
            }

            if (member.characterIndex == characterIndex)
            {
                return member;
            }
        }

        Debug.LogWarning(
            $"characterIndex={characterIndex} のキャラクターがPartyに存在しません");

        return null;
    }

    // =========================================================
    // ランダムカード取得
    // =========================================================

    private SkillData GetRandomCard(CharacterClass characterClass)
    {
        if (skillDatabase == null)
        {
            Debug.LogWarning(
                "SkillDatabaseが設定されていません");

            return null;
        }

        if (skillDatabase.skills == null ||
            skillDatabase.skills.Length == 0)
        {
            Debug.LogWarning(
                "SkillDatabaseにカードが登録されていません");

            return null;
        }

        List<SkillData> candidates = new();

        foreach (SkillData skill in skillDatabase.skills)
        {
            if (skill == null)
            {
                continue;
            }

            // 全クラス共通カード
            if (skill.exclusiveClass == CharacterClass.None)
            {
                candidates.Add(skill);
                continue;
            }

            // キャラクター専用カード
            if (skill.exclusiveClass == characterClass)
            {
                candidates.Add(skill);
            }
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning(
                $"CharacterClass={characterClass} が取得できるカードがありません");

            return null;
        }

        return candidates[
            Random.Range(0, candidates.Count)];
    }

    // =========================================================
    // ランダムカード追加
    // =========================================================

    private void AddRandomCardToParty()
    {
        List<PartyMemberData> party = GetPartyMembers();

        if (party == null)
        {
            return;
        }

        foreach (PartyMemberData member in party)
        {
            if (member == null)
            {
                continue;
            }

            SkillData card =
                GetRandomCard(member.characterClass);

            if (card == null)
            {
                continue;
            }

            if (member.deck == null)
            {
                member.deck = new List<SkillData>();
            }

            member.deck.Add(card);

            Debug.Log(
                $"[EventEffect] {member.characterIndex}番 " +
                $"({member.characterClass}) が " +
                $"「{card.skillName}」を獲得");
        }
    }

    // =========================================================
    // カード削除
    // =========================================================

    public bool RemoveCard(
        int characterIndex,
        SkillData card)
    {
        if (card == null)
        {
            return false;
        }

        PartyMemberData member =
            GetPartyMember(characterIndex);

        if (member == null)
        {
            return false;
        }

        if (member.deck == null ||
            !member.deck.Contains(card))
        {
            return false;
        }

        member.deck.Remove(card);

        Debug.Log(
            $"[EventEffect] " +
            $"{member.characterIndex}番のデッキから " +
            $"「{card.skillName}」を削除");

        return true;
    }

    public List<SkillData> GetDeck(int characterIndex)
    {
        PartyMemberData member =
            GetPartyMember(characterIndex);

        if (member == null)
        {
            return null;
        }

        if (member.deck == null)
        {
            member.deck = new List<SkillData>();
        }

        return member.deck;
    }
}