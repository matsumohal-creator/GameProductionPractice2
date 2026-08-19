using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckListUI : MonoBehaviour
{
    [Header("キャラクター選択ボタン")]
    [SerializeField]
    private DeckCharacterButton[] characterButtons;

    [Header("カード表示場所")]
    [SerializeField]
    private Transform cardParent;

    [Header("カードプレハブ")]
    [SerializeField]
    private GameObject cardPrefab;

    [Header("戻るボタン")]
    [SerializeField]
    private Button backButton;

    [Header("キャラクター情報")]
    [SerializeField]
    private CharacterDeckDisplayData[] characterDisplayData;

    private int selectedCharacterIndex = -1;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // デッキUIを表示
    public void ShowDeck()
    {
        gameObject.SetActive(true);

        SetupCharacterButtons();

        if (SaveManager.CurrentSave != null &&
            SaveManager.CurrentSave.partyMembers.Count > 0)
        {
            SelectCharacter(0);
        }
    }

    // キャラクターボタンを設定
    private void SetupCharacterButtons()
    {
        SaveData saveData = SaveManager.CurrentSave;

        if (saveData == null ||
            saveData.partyMembers == null)
        {
            return;
        }

        for (int i = 0; i < characterButtons.Length; i++)
        {
            if (i >= saveData.partyMembers.Count)
            {
                characterButtons[i].gameObject.SetActive(false);
                continue;
            }

            characterButtons[i].gameObject.SetActive(true);

            PartyMemberData member = saveData.partyMembers[i];

            Sprite icon = null;

            if (i < characterDisplayData.Length &&
                characterDisplayData[i] != null)
            {
                icon = characterDisplayData[i].icon;
                
            }

            characterButtons[i].Initialize(
                this,
                i,
                icon
            );
        }
    }

    // キャラクターを選択
    public void SelectCharacter(int index)
    {
        SaveData saveData = SaveManager.CurrentSave;

        if (saveData == null ||
            saveData.partyMembers == null)
        {
            return;
        }

        if (index < 0 ||
            index >= saveData.partyMembers.Count)
        {
            return;
        }

        selectedCharacterIndex = index;

        DisplaySelectedCharacterDeck();
    }

    // 選択中キャラクターのデッキを表示
    private void DisplaySelectedCharacterDeck()
    {
        ClearCards();

        SaveData saveData = SaveManager.CurrentSave;

        if (saveData == null ||
            selectedCharacterIndex < 0 ||
            selectedCharacterIndex >= saveData.partyMembers.Count)
        {
            return;
        }

        PartyMemberData member =
            saveData.partyMembers[selectedCharacterIndex];

        if (member.deck == null ||
            member.deck.Count == 0)
        {
            return;
        }

        foreach (SkillData skill in member.deck)
        {
            if (skill == null)
            {
                continue;
            }

            GameObject cardObject =
                Instantiate(cardPrefab, cardParent);

            CardView cardView =
                cardObject.GetComponent<CardView>();

            if (cardView == null)
            {
                Debug.LogWarning(
                    "CardPrefabにCardViewがありません"
                );

                Destroy(cardObject);
                continue;
            }

            cardView.Setup(skill);
        }
    }

    // 表示中のカードを削除
    private void ClearCards()
    {
        if (cardParent == null)
        {
            return;
        }

        for (int i = cardParent.childCount - 1; i >= 0; i--)
        {
            Destroy(cardParent.GetChild(i).gameObject);
        }
    }

    // デッキUIを閉じる
    public void HideDeck()
    {
        ClearCards();

        selectedCharacterIndex = -1;

        gameObject.SetActive(false);
    }

    // 戻るボタン
    public void OnClickBack()
    {
        HideDeck();
    }
}