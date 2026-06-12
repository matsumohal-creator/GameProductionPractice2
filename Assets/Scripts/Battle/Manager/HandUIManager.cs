using System.Collections.Generic;
using UnityEngine;

public class HandUIManager : MonoBehaviour
{
    [SerializeField]
    private Transform cardParent;

    [SerializeField]
    private CardView cardPrefab;

    public void RefreshHand(
        IReadOnlyList<SkillData> hand)
    {
        ClearHand();

        foreach (SkillData skill in hand)
        {
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