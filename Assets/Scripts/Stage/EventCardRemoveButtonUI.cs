using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventCardRemoveButtonUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image iconImage;

    private EventCardRemoveUI owner;

    private SkillData card;
    private int characterIndex;
    private CharacterClass characterClass;

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

        if (characterNameText != null)
        {
            characterNameText.text =
                $"キャラクター {characterIndex}";
        }

        if (cardNameText != null)
        {
            cardNameText.text = card.skillName;
        }

        if (descriptionText != null)
        {
            descriptionText.text = card.description;
        }

        if (iconImage != null)
        {
            iconImage.sprite = card.icon;
        }
    }

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