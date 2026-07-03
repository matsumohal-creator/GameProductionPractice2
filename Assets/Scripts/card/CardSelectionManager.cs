using UnityEngine;

public class CardSelectionManager : MonoBehaviour
{
    public static CardSelectionManager Instance;

    private CardView selectedCard;

    public CardView SelectedCard => selectedCard;

    private void Awake()
    {
        Instance = this;
    }

    public void Select(CardView card)
    {
        //同じカードなら解除
        if (selectedCard == card)
        {
            selectedCard.SetSelected(false);
            selectedCard = null;
            return;
        }

        //以前のカード
        if (selectedCard != null)
        {
            selectedCard.SetSelected(false);
        }

        selectedCard = card;
        selectedCard.SetSelected(true);



    }

    public void Clear()
    {
        if (selectedCard != null)
        {
            selectedCard.SetSelected(false);
        }

        selectedCard = null;
    }

    private void UseCard(CardView card)
    {
        Debug.Log(card.SkillData.skillName + " を使用");

        selectedCard.SetSelected(false);
        selectedCard = null;
    }
}