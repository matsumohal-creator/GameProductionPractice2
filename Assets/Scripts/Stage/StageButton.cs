using UnityEngine;
using UnityEngine.UI;

// ここでは、ステージ選択ボタンのスクリプトを作成します。
// このスクリプトは、ボタンがクリックされたときに、
// StageManagerに選択されたクエストのインデックスを通知します。

// Inspectorには下記のような設定を行ってください
/* 例
Stage_0なら
QuestIndex = 0

Stage_1なら
QuestIndex = 1
*/

public class StageButton : MonoBehaviour
{
    [SerializeField]
    private int questIndex;

    private StageManager stageManager;

    public void Initialize(StageManager manager)
    {
        stageManager = manager;
    }

    // ボタンのインタラクティブ状態を設定するメソッド
    //　例えば、クエストが選択可能かどうかに応じてボタンの状態を変更することができます。
    public void SetInteractable(bool value)
    {
        GetComponent<Button>().interactable = value;
    }

    public void OnClick()
    {
        stageManager.SelectQuest(questIndex);
    }
}