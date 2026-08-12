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
    private StageNodeData stageData;

    [Header("選択表示")]
    [SerializeField] private GameObject selectedHighlight;

    private StageManager stageManager;

    public StageNodeData StageData => stageData;

    [SerializeField] private Image iconImage;

    public void Initialize(StageManager manager)
    {
        stageManager = manager;
        SetSelected(false);
    }

    // ボタンのインタラクティブ状態を設定するメソッド
    //　例えば、クエストが選択可能かどうかに応じてボタンの状態を変更することができます。
    public void SetInteractable(bool value)
    {
        Button button = GetComponent<Button>();
        button.interactable = value;

        if (iconImage != null)
        {
            iconImage.color = value ? Color.white : Color.gray;
        }
    }

    public void SetSelected(bool value)
    {
        if (selectedHighlight != null)
        {
            selectedHighlight.SetActive(value);
        }
    }

    public void OnClick()
    {
        stageManager.SelectStage(stageData);
    }
}