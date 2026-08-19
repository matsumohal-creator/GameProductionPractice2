using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditScene : MonoBehaviour
{
    [SerializeField]
    private List<EditSceneCard> characterCards;

    [Header("キャラクター初期デッキ")]
    [SerializeField]
    private List<CharacterDeckData> characterDecks;

    // 編成可能な最大人数
    [SerializeField]
    private int maxSelectedCount = 4;

    // キャラ画像クリック時の共通処理
    private void OnCharacterClick(int index)
    {
        EditSceneCard currentCard = characterCards[index];

        if (currentCard.selectedFlg == 1)
        {
            currentCard.select();
        }
        else
        {
            // 編成上限に達している場合は新規編成しない
            int currentSelectedCount = 0;
            for (int i = 0; i < characterCards.Count; i++)
            {
                if (characterCards[i].selectedFlg == 1)
                {
                    currentSelectedCount++;
                }
            }

            if (currentSelectedCount >= maxSelectedCount)
            {
                return;
            }
            currentCard.select();
        }
    }

    // 各ボタンから呼ばれるクリックイベント
    public void Character0Click()
    {
        OnCharacterClick(0);
    }
    public void Character1Click()
    {
        OnCharacterClick(1);
    }
    public void Character2Click()
    {
        OnCharacterClick(2);
    }
    public void Character3Click()
    {
        OnCharacterClick(3);
    }
    public void Character4Click()
    {
        OnCharacterClick(4);
    }
    public void Character5Click()
    {
        OnCharacterClick(5);
    }

    public void GoToBattle()
    {
        // 選択されたキャラクターのインデックスを作成
        List<int> selectedPlayers = new List<int>();

        for (int i = 0; i < characterCards.Count; i++)
        {
            if (characterCards[i].selectedFlg == 1)
            {
                selectedPlayers.Add(i);
            }
        }

        // 選択キャラクターがいない場合
        if (selectedPlayers.Count == 0)
        {
            Debug.LogWarning("キャラクターが1人も選択されていません");
            return;
        }

        // セーブデータが存在するか確認
        if (SaveManager.CurrentSave == null)
        {
            Debug.LogError(
                "SaveManager.CurrentSave が存在しません");
            return;
        }

        // 現在のパーティを一度クリア
        SaveManager.CurrentSave.partyMembers.Clear();

        // =====================================================
        // 選択したキャラクターをPartyMemberDataへ保存
        // =====================================================
        foreach (int index in selectedPlayers)
        {
            CharacterDeckData deckData =
                characterDecks[index];

            if (deckData == null)
            {
                Debug.LogError(
                    $"CharacterIndex={index} のCharacterDeckDataが設定されていません");

                continue;
            }

            PartyMemberData member =
                new PartyMemberData();

            // キャラクター識別番号
            member.characterIndex = index;

            // キャラクタークラス
            member.characterClass =
                deckData.characterClass;

            // HP初期化
            member.maxHp = 100;
            member.currentHp = member.maxHp;

            // 初期デッキをコピー
            member.deck =
                deckData.startDeck != null
                    ? new List<SkillData>(deckData.startDeck)
                    : new List<SkillData>();

            // パーティに追加
            SaveManager.CurrentSave.partyMembers.Add(member);

            Debug.Log(
                $"[EditScene] パーティ保存: CharacterIndex = {index}, HP = {member.currentHp}/{member.maxHp}");
        }

        //ステージシーンへ移行
        SceneLoader.NextSceneName = "StageScene";
        SceneManager.LoadScene("LoadingScene");
    }
}
