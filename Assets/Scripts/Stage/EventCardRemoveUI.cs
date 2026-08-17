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

    private int targetCharacterIndex = -1;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(EventEffectManager manager)
    {
        effectManager = manager;
    }

    public void Show(int characterIndex)
    {
        if (effectManager == null)
        {
            Debug.LogError(
                "EventEffectManagerが設定されていません");

            return;
        }

        targetCharacterIndex = characterIndex;

        List<SkillData> deck =
            effectManager.GetDeck(characterIndex);

        if (deck == null || deck.Count == 0)
        {
            Debug.LogWarning(
                $"characterIndex={characterIndex} のデッキにカードがありません");

            return;
        }

        gameObject.SetActive(true);

        if (titleText != null)
        {
            titleText.text = "削除するカードを選択";
        }

        ClearCards();

        foreach (SkillData card in deck)
        {
            if (card == null)
            {
                continue;
            }

            EventCardRemoveButtonUI button =
                Instantiate(cardButtonPrefab, cardRoot);

            button.Initialize(this, card);
        }
    }

    private void ClearCards()
    {
        for (int i = cardRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(cardRoot.GetChild(i).gameObject);
        }
    }

    public void SelectCard(SkillData card)
    {
        if (card == null)
        {
            return;
        }

        bool removed =
            effectManager.RemoveCard(
                targetCharacterIndex,
                card);

        if (!removed)
        {
            return;
        }

        Debug.Log(
            $"カード削除完了: {card.skillName}");

        gameObject.SetActive(false);

        ClearCards();

        targetCharacterIndex = -1;
    }
}