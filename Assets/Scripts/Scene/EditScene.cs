using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditScene : MonoBehaviour
{
    [SerializeField]
    private List<EditSceneCard> characterCards;
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

        // GameManagerに戦闘情報を保存
        GameManager.SetupBattle(selectedPlayers);

        //Debug.Log($"[EditScene] パーティ編成完了: {selectedPlayers.Count}人");

        foreach (int index in selectedPlayers)
        {
            Debug.Log( $"[EditScene] Selected Player Index = {index}");
        }

        //バトルシーンへ移行
        SceneManager.LoadScene("HomeScene");
    }
}
