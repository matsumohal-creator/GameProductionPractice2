using System.Collections.Generic;
using UnityEngine;

public class BattleRewardManager : MonoBehaviour
{
    [Header("スキルデータベース")]
    [SerializeField]
    private SkillDatabase skillDatabase;

    // 現在報酬を受け取っているメンバー
    private List<PartyMemberData> targetMembers = new();

    // 現在何人目か
    private int currentMemberIndex;

    // 現在表示している候補
    private List<SkillData> currentCandidates = new();

    // =========================================================
    // 報酬開始
    // =========================================================

    public void StartReward()
    {
        if (SaveManager.CurrentSave == null)
        {
            Debug.LogError(
                "[BattleReward] SaveDataが存在しません");
            return;
        }

        if (SaveManager.CurrentSave.partyMembers == null ||
            SaveManager.CurrentSave.partyMembers.Count == 0)
        {
            Debug.LogError(
                "[BattleReward] パーティメンバーが存在しません");
            return;
        }

        targetMembers.Clear();

        foreach (PartyMemberData member
                 in SaveManager.CurrentSave.partyMembers)
        {
            if (member == null)
            {
                continue;
            }

            targetMembers.Add(member);
        }

        if (targetMembers.Count == 0)
        {
            Debug.LogWarning(
                "[BattleReward] 報酬対象のキャラクターがいません");
            return;
        }

        currentMemberIndex = 0;

        Debug.Log(
            $"[BattleReward] 報酬開始: {targetMembers.Count}人");

        ShowNextReward();
    }

    // =========================================================
    // 次のキャラクター
    // =========================================================

    private void ShowNextReward()
    {
        if (currentMemberIndex >= targetMembers.Count)
        {
            FinishReward();
            return;
        }

        PartyMemberData member =
            targetMembers[currentMemberIndex];

        if (member == null)
        {
            currentMemberIndex++;
            ShowNextReward();
            return;
        }

        currentCandidates =
            GenerateCandidates(member.characterClass, 3);

        if (currentCandidates.Count == 0)
        {
            Debug.LogWarning(
                $"[BattleReward] " +
                $"CharacterIndex={member.characterIndex} " +
                "に候補カードを生成できません");

            currentMemberIndex++;
            ShowNextReward();
            return;
        }

        if (BattleRewardUI.Instance != null)
        {
            BattleRewardUI.Instance.ShowReward(
                member,
                currentCandidates);
        }

        Debug.Log(
            $"[BattleReward] " +
            $"{currentMemberIndex + 1}/{targetMembers.Count}人目 " +
            $"CharacterIndex={member.characterIndex}");
    }

    // =========================================================
    // 候補カード生成
    // =========================================================

    private List<SkillData> GenerateCandidates(
        CharacterClass characterClass,
        int count)
    {
        List<SkillData> candidates = new();

        if (skillDatabase == null)
        {
            Debug.LogError(
                "[BattleReward] SkillDatabaseが設定されていません");
            return candidates;
        }

        if (skillDatabase.skills == null ||
            skillDatabase.skills.Length == 0)
        {
            Debug.LogError(
                "[BattleReward] SkillDatabaseにカードがありません");
            return candidates;
        }

        List<SkillData> pool = new();

        foreach (SkillData skill in skillDatabase.skills)
        {
            if (skill == null)
            {
                continue;
            }

            // 共通カード
            if (skill.exclusiveClass == CharacterClass.None)
            {
                pool.Add(skill);
                continue;
            }

            // クラス専用カード
            if (skill.exclusiveClass == characterClass)
            {
                pool.Add(skill);
            }
        }

        if (pool.Count == 0)
        {
            return candidates;
        }

        // 同じカードを3枚出さない
        List<SkillData> available =
            new List<SkillData>(pool);

        int actualCount =
            Mathf.Min(count, available.Count);

        for (int i = 0; i < actualCount; i++)
        {
            int randomIndex =
                Random.Range(0, available.Count);

            candidates.Add(
                available[randomIndex]);

            available.RemoveAt(randomIndex);
        }

        return candidates;
    }

    // =========================================================
    // カード選択
    // =========================================================

    public void SelectCard(SkillData selectedCard)
    {
        if (selectedCard == null)
        {
            return;
        }

        if (currentMemberIndex >= targetMembers.Count)
        {
            return;
        }

        PartyMemberData member =
            targetMembers[currentMemberIndex];

        if (member == null)
        {
            return;
        }

        if (member.deck == null)
        {
            member.deck = new List<SkillData>();
        }

        member.deck.Add(selectedCard);

        Debug.Log(
            $"[BattleReward] " +
            $"CharacterIndex={member.characterIndex} が " +
            $"「{selectedCard.skillName}」を獲得");

        currentMemberIndex++;

        ShowNextReward();
    }

    // =========================================================
    // 報酬完了
    // =========================================================

    private void FinishReward()
    {
        Debug.Log(
            "[BattleReward] 全キャラクターの報酬選択完了");

        currentCandidates.Clear();
        targetMembers.Clear();

        currentMemberIndex = 0;

        if (BattleRewardUI.Instance != null)
        {
            BattleRewardUI.Instance.Hide();
        }

        if (BattleResultUI.Instance != null)
        {
            //BattleResultUI.Instance.ShowVictoryResult();
        }
    }

    // =========================================================
    // 外部参照
    // =========================================================

    public int CurrentMemberNumber
    {
        get
        {
            return currentMemberIndex + 1;
        }
    }

    public int TotalMemberCount
    {
        get
        {
            return targetMembers.Count;
        }
    }
}