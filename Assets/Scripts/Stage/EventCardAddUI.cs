using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventCardAddUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardCostText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image iconImage;

    [Header("次へボタン")]
    [SerializeField] private GameObject nextButton;

    private Action onCompleted;

    private List<EventCardAddData> acquiredCards =
        new List<EventCardAddData>();

    private int currentIndex;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(
        Action onCompleted)
    {
        this.onCompleted = onCompleted;
    }

    // =========================================================
    // カード獲得UIを表示
    // =========================================================

    public void Show(
        List<EventCardAddData> cards)
    {
        if (cards == null ||
            cards.Count == 0)
        {
            onCompleted?.Invoke();
            return;
        }

        acquiredCards =
            new List<EventCardAddData>(cards);

        currentIndex = 0;

        gameObject.SetActive(true);

        ShowCurrentCard();
    }

    // =========================================================
    // 現在のカードを表示
    // =========================================================

    private void ShowCurrentCard()
    {
        if (currentIndex >= acquiredCards.Count)
        {
            Finish();
            return;
        }

        EventCardAddData data =
            acquiredCards[currentIndex];

        if (data == null ||
            data.card == null)
        {
            currentIndex++;
            ShowCurrentCard();
            return;
        }

        if (titleText != null)
        {
            titleText.text = "カード獲得！";
        }

        if (characterNameText != null)
        {
            characterNameText.text =
                $"キャラクター {data.characterIndex}";
        }

        if (cardNameText != null)
        {
            cardNameText.text =
                data.card.skillName;
        }

        if (cardCostText != null)
        {
            cardCostText.text =
                data.card.cost.ToString();
        }

        if (descriptionText != null)
        {
            descriptionText.text =
                data.card.description;
        }

        if (iconImage != null)
        {
            iconImage.sprite =
                data.card.icon;
        }

        // 最後の1枚なら「次へ」ではなく
        // 「確認」などに変えることも可能
        if (nextButton != null)
        {
            nextButton.SetActive(true);
        }

        Debug.Log(
            $"[EventEffect] カード獲得UI: " +
            $"CharacterIndex={data.characterIndex}, " +
            $"Class={data.characterClass}, " +
            $"Card={data.card.skillName}");
    }

    // =========================================================
    // 次へ
    // =========================================================

    public void OnClickNext()
    {
        currentIndex++;

        ShowCurrentCard();
    }

    // =========================================================
    // 完了
    // =========================================================

    private void Finish()
    {
        acquiredCards.Clear();
        currentIndex = 0;

        gameObject.SetActive(false);

        onCompleted?.Invoke();
    }
}