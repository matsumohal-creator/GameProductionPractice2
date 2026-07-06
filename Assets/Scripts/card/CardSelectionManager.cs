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

    // カードを選択する関数
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

    // 選択したカードを使用する関数
    public void UseSelectedCard(IStatusEffectTarget target)
    {
        //nullチェック
        if (selectedCard == null)
            return;

        PlayerBase player = BattleManager.Instance.Players[0];

        SkillData skill = selectedCard.SkillData;

        // エナジー確認
        if (!player.TryUseEnergy(skill.cost))
        {
            Debug.Log("エナジー不足");
            return;
        }

        // スキル使用
        BattleManager.Instance.UseSkill(
            player,
            skill,
            target);

        // 手札から削除
        HandManager hand =
            player.GetComponent<HandManager>();

        hand.RemoveCard(skill);

        // 捨て札へ
        DeckManager deck =
            player.GetComponent<DeckManager>();

        deck.AddToDiscardPile(skill);

        // UI更新
        hand.RefreshUI();

        // 選択解除
        Clear();
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