using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewardCardUI : MonoBehaviour
{
    [Header("カード表示")]
    [SerializeField]
    private Image cardImage;

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text costText;

    [SerializeField]
    private TMP_Text explanationText;

    private BattleRewardUI owner;

    private SkillData card;

    // =========================================================
    // 初期化
    // =========================================================

    public void Initialize(
        BattleRewardUI owner,
        SkillData card)
    {
        this.owner = owner;
        this.card = card;

        if (card == null)
        {
            return;
        }

        if (cardImage != null)
        {
            cardImage.sprite = card.icon;
        }

        if (nameText != null)
        {
            nameText.text = card.skillName;
        }

        if (costText != null)
        {
            costText.text = card.cost.ToString();
        }

        if (explanationText != null)
        {
            explanationText.text = card.description;
        }
    }

    // =========================================================
    // 選択
    // =========================================================

    public void OnClick()
    {
        if (owner == null || card == null)
        {
            return;
        }

        owner.SelectCard(card);
    }
}