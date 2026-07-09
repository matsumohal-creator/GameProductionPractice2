using System.Collections.Generic;
using UnityEngine;

public class HandUIManager : MonoBehaviour
{
    //手札のカードを配置する親オブジェクト
    [SerializeField]
    private Transform cardParent;

    //カードのプレハブ
    [SerializeField]
    private CardView cardPrefab;

    //手札のUIを更新する
    public void RefreshHand(
        IReadOnlyList<SkillData> hand)
    {

        Debug.Log("RefreshHand呼ばれた");
        Debug.Log("カード枚数 = " + hand.Count);

        //手札のUIをクリアする
        ClearHand();

        //手札のカードを生成する
        foreach (SkillData skill in hand)
        {
            Debug.Log("生成：" + skill.skillName);

            CardView card =
                Instantiate(
                    cardPrefab,
                    cardParent);

            card.Setup(skill);
        }
    }

    public void ClearHand()
    {
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }
    }
}