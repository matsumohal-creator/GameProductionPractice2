using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


// 手札の管理を行うクラス  
// 手札の管理は、プレイヤーが持っているカードのリストを保持し、デッキからカードを引く処理を行います。

//参照している物
public class HandManager : MonoBehaviour
{

    //インスペクターで初期手札
    private List<SkillData> hand = new();

    //手札を外部からアクセスできるようにする
    public IReadOnlyList<SkillData> Hand => hand;

    [Header("Manager")]
    [SerializeField] //デッキマネージャーのアタッチ
    private DeckManager deckManager;

    [SerializeField] //ハンドマネージャーのアタッチ
    private HandUIManager handUIManager;

    private void Awake()
    {
        // デッキマネージャーがアタッチされていない場合、同じGameObjectから取得
        if (deckManager == null)
        {
            deckManager = GetComponent<DeckManager>();
        }
    }


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
        RefreshUI();
    }

    // UIを更新するメソッド  
    public void RefreshUI()
    {
        if (handUIManager != null)
        {
            handUIManager.RefreshHand(hand);
        }
    }

    //手札からカードを削除する
    public void RemoveCard(SkillData card)
    {
        hand.Remove(card);
        RefreshUI();
    }

    //手札をすべて捨てる
    public void DiscardAll()
    {
        foreach (SkillData card in hand)
        {
            deckManager.AddToDiscardPile(card);
        }

        hand.Clear();

        RefreshUI();
    }
}