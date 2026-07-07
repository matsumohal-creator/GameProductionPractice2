using System.Collections.Generic;

// セーブデータのクラス
// ここでは、セーブデータとして保存したい情報(器)を定義します。
// 例えば、現在のステージ、パーティのHP、デッキのカード、現在のクエスト名などを保存することができます。

[System.Serializable]
public class SaveData
{
    // 現在地点
    public int currentStageIndex;
    // 選択中クエストのインデックス(-1は未選択)
    public int selectedQuestIndex = -1;
    // 現在進行中クエスト
    public string currentQuestName;

    // パーティHP
    public List<int> partyHp = new();

    // デッキ
    public List<string> deckCardNames = new();
}