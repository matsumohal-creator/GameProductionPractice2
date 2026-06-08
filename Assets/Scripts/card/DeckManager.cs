using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    //インスペクターで初期デッキを設定できるようにするためのフィールド
    [Header("初期デッキ")]
    [SerializeField]

    // スキルデータのリストを初期デッキとして設定
    private List<SkillData> startDeck = new();

    // ドローとディスカードの管理のためのリスト
    private List<SkillData> drawPile = new();

    // ドローしたカードを置くためのリスト
    private List<SkillData> discardPile = new();

    // ドローパイルとディスカードパイルを外部からアクセスできるようにする
    public List<SkillData> DrawPile => drawPile;

    // ドローパイルとディスカードパイルを外部からアクセスできるようにする
    public List<SkillData> DiscardPile => discardPile;

    private void Awake()
    {
        // デッキを初期化する
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        // ドローパイルとディスカードパイルをクリアして
        drawPile.Clear();
        discardPile.Clear();

        // 初期デッキを山札に追加
        drawPile.AddRange(startDeck);

        // 山札をシャッフルする
        Shuffle();
    }


    //カードを引くメソッド
    public SkillData DrawCard()
    {
        if (drawPile.Count == 0)
        {
            ReshuffleDiscardPile();
        }

        if (drawPile.Count == 0)
        {
            return null;
        }

        SkillData card = drawPile[0];

        drawPile.RemoveAt(0);

        return card;
    }

    //カードを捨てるメソッド
    public void AddToDiscardPile(SkillData card)
    {
        discardPile.Add(card);
    }

    private void ReshuffleDiscardPile()
    {
        drawPile.AddRange(discardPile);

        discardPile.Clear();

        Shuffle();
    }


    // 山札をシャッフルするメソッド
    public void Shuffle()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            int randomIndex = Random.Range(i, drawPile.Count);

            SkillData temp = drawPile[i];
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = temp;
        }
    }
}