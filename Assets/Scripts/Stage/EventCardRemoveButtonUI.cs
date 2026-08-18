using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventCardRemoveButtonUI : MonoBehaviour
{
    [Header("既存Card UI")]
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text explanationText;

    private EventCardRemoveUI owner;

    private SkillData card;
    private int characterIndex;
    private CharacterClass characterClass;

    // =========================================================
    // 初期化
    // =========================================================

    public void Initialize(
        EventCardRemoveUI owner,
        int characterIndex,
        CharacterClass characterClass,
        SkillData card)
    {
        this.owner = owner;
        this.characterIndex = characterIndex;
        this.characterClass = characterClass;
        this.card = card;

        if (card == null)
        {
            return;
        }

        // カード画像
        if (cardImage != null)
        {
            cardImage.sprite = card.icon;
        }

        // カード名
        if (nameText != null)
        {
            nameText.text = card.skillName;
        }

        // コスト
        if (costText != null)
        {
            costText.text = card.cost.ToString();
        }

        // 説明
        if (explanationText != null)
        {
            explanationText.text = card.description;
        }
    }

    // =========================================================
    // カード選択
    // =========================================================

    public void OnClick()
    {
        if (owner == null || card == null)
        {
            return;
        }

        owner.SelectCard(
            characterIndex,
            characterClass,
            card);
    }
}