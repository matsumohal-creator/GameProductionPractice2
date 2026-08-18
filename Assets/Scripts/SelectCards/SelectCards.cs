using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCards : MonoBehaviour
{
    [Header("カード表示")]
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("選択表示")]
    [SerializeField] private GameObject selectedFrame;

    private SelectCardsManager owner;
    private CharacterClass targetClass;
    private SkillData cardData;
    private Button cachedButton;

    // どの職業枠のカードか（Fighter/Healer/Knight/Mage）
    public CharacterClass TargetClass => targetClass;

    // このスロットに表示中のカード
    public SkillData CardData => cardData;

    // 有効なカードが設定されているか
    public bool HasCard => cardData != null;

    private void Awake()
    {
        cachedButton = GetComponent<Button>();
    }

    // カード1枚分の表示内容を初期化
    public void Initialize(
        SelectCardsManager owner,
        CharacterClass targetClass,
        SkillData card)
    {
        this.owner = owner;
        this.targetClass = targetClass;
        cardData = card;

        if (cardImage == null ||
            nameText == null ||
            costText == null ||
            descriptionText == null)
        {
            Debug.LogWarning($"[SelectCards] UI参照不足: {gameObject.name}");
        }

        if (cardImage != null)
        {
            cardImage.sprite = card != null ? card.icon : null;
            cardImage.enabled = card != null;
        }

        if (nameText != null)
        {
            nameText.text = card != null ? card.skillName : "";
        }

        if (costText != null)
        {
            costText.text = card != null ? card.cost.ToString() : "";
        }

        if (descriptionText != null)
        {
            descriptionText.text = card != null ? card.description : "";
        }

        if (cachedButton != null)
        {
            cachedButton.interactable = card != null;
        }

        if (card == null)
        {
            Debug.LogWarning($"[SelectCards] カード未設定スロット: {targetClass} / {gameObject.name}");
        }

        SetSelected(false);
    }

    // 選択枠の表示ON/OFF
    public void SetSelected(bool isSelected)
    {
        if (selectedFrame != null)
        {
            selectedFrame.SetActive(isSelected);
        }
    }

    // ボタン押下時にManagerへ通知
    public void OnClick()
    {
        if (owner == null)
        {
            return;
        }

        owner.OnCardClicked(this);
    }
}
