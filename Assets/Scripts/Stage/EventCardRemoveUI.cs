using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventCardRemoveUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text titleText;

    [Header("カード一覧")]
    [SerializeField] private Transform cardRoot;
    [SerializeField] private EventCardRemoveButtonUI cardButtonPrefab;

    private EventEffectManager effectManager;

    // カード削除完了時にEventOverlayUIへ通知する
    private Action onRemoveCompleted;

    // =========================================================
    // カード削除対象
    // =========================================================

    // 今回カード削除を行うキャラクター一覧
    private List<PartyMemberData> targetMembers = new();

    // 現在何人目のキャラクターを処理しているか
    private int currentMemberIndex = 0;

    // =========================================================
    // 初期化
    // =========================================================
    public void Initialize(
        EventEffectManager manager,
        Action onRemoveCompleted = null)
    {
        effectManager = manager;
        this.onRemoveCompleted = onRemoveCompleted;
    }

    // =========================================================
    // カード削除UIを表示
    // =========================================================

    public void Show()
    {
        if (effectManager == null)
        {
            Debug.LogError(
                "EventEffectManagerが設定されていません");

            return;
        }

        if (SaveManager.CurrentSave == null)
        {
            Debug.LogWarning(
                "SaveDataが存在しません");

            return;
        }

        List<PartyMemberData> party =
            SaveManager.CurrentSave.partyMembers;

        if (party == null)
        {
            Debug.LogWarning(
                "PartyMemberDataが存在しません");

            return;
        }

        // =====================================================
        // 対象キャラクターを作成
        // =====================================================

        targetMembers.Clear();

        foreach (PartyMemberData member in party)
        {
            if (member == null)
            {
                continue;
            }

            // デッキが存在しない
            if (member.deck == null)
            {
                continue;
            }

            // デッキにカードがない
            if (member.deck.Count == 0)
            {
                continue;
            }

            targetMembers.Add(member);
        }

        // =====================================================
        // 削除可能なカードが存在しない
        // =====================================================

        if (targetMembers.Count == 0)
        {
            Debug.LogWarning(
                "削除可能なカードがありません");

            // カード削除処理自体は完了扱い
            onRemoveCompleted?.Invoke();

            return;
        }

        // 最初のキャラクターから開始
        currentMemberIndex = 0;

        gameObject.SetActive(true);

        ShowCurrentMember();
    }

    // =========================================================
    // 現在のキャラクターのカードを表示
    // =========================================================

    private void ShowCurrentMember()
    {
        ClearCards();

        // すべてのキャラクターが終了
        if (currentMemberIndex >= targetMembers.Count)
        {
            FinishRemoveProcess();
            return;
        }

        PartyMemberData member =
            targetMembers[currentMemberIndex];

        if (member == null)
        {
            currentMemberIndex++;
            ShowCurrentMember();
            return;
        }

        // デッキが空になっていた場合
        if (member.deck == null ||
            member.deck.Count == 0)
        {
            Debug.Log(
                $"CharacterIndex={member.characterIndex} のデッキが空のためスキップ");

            currentMemberIndex++;
            ShowCurrentMember();
            return;
        }

        // =====================================================
        // タイトル
        // =====================================================

        if (titleText != null)
        {
            titleText.text =
                $"キャラクター {member.characterIndex}\n" +
                "削除するカードを1枚選択";
        }

        // =====================================================
        // 現在のキャラクターのカードだけ表示
        // =====================================================

        foreach (SkillData card in member.deck)
        {
            if (card == null)
            {
                continue;
            }

            EventCardRemoveButtonUI button =
                Instantiate(
                    cardButtonPrefab,
                    cardRoot);

            button.Initialize(
                this,
                member.characterIndex,
                member.characterClass,
                card);
        }

        Debug.Log(
            $"[EventEffect] カード削除対象 " +
            $"{currentMemberIndex + 1}/{targetMembers.Count}人目: " +
            $"CharacterIndex={member.characterIndex}");
    }

    // =========================================================
    // カード選択
    // =========================================================

    public void SelectCard(
        int characterIndex,
        CharacterClass characterClass,
        SkillData card)
    {
        if (card == null)
        {
            return;
        }

        bool removed =
            effectManager.RemoveCard(
                characterIndex,
                card);

        if (!removed)
        {
            Debug.LogWarning(
                $"カード削除に失敗: {card.skillName}");

            return;
        }

        Debug.Log(
            $"[EventEffect] カード削除完了: " +
            $"CharacterIndex={characterIndex}, " +
            $"Class={characterClass}, " +
            $"Card={card.skillName}");

        // =====================================================
        // 次のキャラクターへ
        // =====================================================

        currentMemberIndex++;

        ShowCurrentMember();
    }

    // =========================================================
    // 全キャラクター終了
    // =========================================================

    private void FinishRemoveProcess()
    {
        Debug.Log(
            "[EventEffect] 4人分のカード削除処理が完了しました");

        ClearCards();

        targetMembers.Clear();

        currentMemberIndex = 0;

        gameObject.SetActive(false);

        // EventOverlayUIへ通知
        onRemoveCompleted?.Invoke();
    }

    // =========================================================
    // カードボタン削除
    // =========================================================

    private void ClearCards()
    {
        if (cardRoot == null)
        {
            return;
        }

        for (int i = cardRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(
                cardRoot.GetChild(i).gameObject);
        }
    }
}