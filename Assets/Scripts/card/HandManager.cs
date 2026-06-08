using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    //インスペクターで初期手札
    private List<SkillData> hand = new();

    //デッキマネージャーのアタッチ
    [SerializeField]
    private DeckManager deckManager;

    //手札を5枚になるまで引く
    public void DrawToFive()
    {
        while (hand.Count < 5)
        {
            //デッキからカードを引く
            SkillData card = deckManager.DrawCard();

            //引いたカードがnullならデッキが空なので終了
            if (card == null)
                break;

            //引いたカードを手札に追加
            hand.Add(card);
        }
    }
}