using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleRewardUI : MonoBehaviour
{
    public static BattleRewardUI Instance;

    [Header("Root")]
    [SerializeField]
    private GameObject rewardRoot;

    [Header("テキスト")]
    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private TMP_Text characterText;

    [Header("カード")]
    [SerializeField]
    private Transform cardRoot;

    [SerializeField]
    private BattleRewardCardUI cardPrefab;

    [Header("報酬管理")]
    [SerializeField]
    private BattleRewardManager rewardManager;

    private void Awake()
    {
        Instance = this;

        if (rewardRoot != null)
        {
            rewardRoot.SetActive(false);
        }
    }

    // =========================================================
    // 報酬表示
    // =========================================================

    public void ShowReward(
        PartyMemberData member,
        List<SkillData> candidates)
    {
        if (rewardRoot == null)
        {
            Debug.LogError(
                "[BattleRewardUI] rewardRootが設定されていません");
            return;
        }

        if (member == null)
        {
            return;
        }

        rewardRoot.SetActive(true);

        ClearCards();

        if (titleText != null)
        {
            titleText.text = "カード報酬";
        }

        if (characterText != null)
        {
            characterText.text =
                $"キャラクター {member.characterIndex}\n" +
                "獲得するカードを1枚選択";
        }

        foreach (SkillData card in candidates)
        {
            if (card == null)
            {
                continue;
            }

            BattleRewardCardUI cardUI =
                Instantiate(
                    cardPrefab,
                    cardRoot);

            cardUI.Initialize(
                this,
                card);
        }
    }

    // =========================================================
    // カード選択
    // =========================================================

    public void SelectCard(SkillData card)
    {
        if (card == null)
        {
            return;
        }

        if (rewardManager == null)
        {
            Debug.LogError(
                "[BattleRewardUI] " +
                "BattleRewardManagerが設定されていません");
            return;
        }

        rewardManager.SelectCard(card);
    }

    // =========================================================
    // 非表示
    // =========================================================

    public void Hide()
    {
        ClearCards();

        if (rewardRoot != null)
        {
            rewardRoot.SetActive(false);
        }
    }

    // =========================================================
    // カード削除
    // =========================================================

    private void ClearCards()
    {
        if (cardRoot == null)
        {
            return;
        }

        for (int i = cardRoot.childCount - 1;
             i >= 0;
             i--)
        {
            Destroy(
                cardRoot.GetChild(i).gameObject);
        }
    }
}