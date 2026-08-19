using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCharacterButton : MonoBehaviour
{
    [Header("キャラクターアイコン")]
    [SerializeField]
    private Image characterIcon;

    private int characterIndex;
    private DeckListUI deckListUI;

    public void Initialize(
        DeckListUI ui,
        int index,
        Sprite icon
        )
    {
        deckListUI = ui;
        characterIndex = index;

        if (characterIcon != null)
        {
            characterIcon.sprite = icon;
        }
    }

    public void OnClick()
    {
        if (deckListUI == null)
        {
            return;
        }

        deckListUI.SelectCharacter(characterIndex);
    }
}