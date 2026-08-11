using System.Collections.Generic;

// セーブデータのクラス
// ここでは、セーブデータとして保存したい情報(器)を定義します。
// 例えば、現在のステージ、パーティのHP、デッキのカード、現在のクエスト名などを保存することができます。

[System.Serializable]
public class SaveData
{
    // 現在のステージID
    public int currentStageId = 0;
    // 選択されたステージID
    public int selectedStageId = -1;
    // 現在のバトルステージID
    public int currentBattleStageId = -1;

    public string currentStageName;

    // パーティHP
    public List<int> partyHp = new();

    // デッキ
    public List<string> deckCardNames = new();
}