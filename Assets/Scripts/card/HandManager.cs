using System.Collections.Generic;
using UnityEngine;


// 手札の管理を行うクラス  
// 手札の管理は、プレイヤーが持っているカードのリストを保持し、デッキからカードを引く処理を行います。

//参照している物
public class HandManager : MonoBehaviour
{
    //手札のUIマネージャーのアタッチ
    [SerializeField]
    private HandUIManager handUIManager;

    //インスペクターで初期手札
    private List<SkillData> hand = new();

    //手札を外部からアクセスできるようにする
    public IReadOnlyList<SkillData> Hand => hand;

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

        // UI更新
        //HandUIManager参照
        handUIManager.RefreshHand(hand);
    }
}