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

    private StageManager stageManager;

    public StageNodeData StageData => stageData;

    [SerializeField] private Image iconImage;

    [Header("鍵アイコン")]
    [SerializeField] private GameObject lockIcon;

    // Lock状態であれば、ボタンを押せないようにするためのフラグ
    private bool canSelect = false;

    public void Initialize(StageManager manager)
    {
        stageManager = manager;
    }

    // ボタンのインタラクティブ状態を設定するメソッド
    //　例えば、クエストが選択可能かどうかに応じてボタンの状態を変更することができます。
    public void SetInteractable(bool value)
    {
        canSelect = value;

        Button button = GetComponent<Button>();
        button.interactable = value;

        if (iconImage != null)
        {
            iconImage.color = value ? Color.white : Color.gray;
        }

        // 選択不可なら鍵表示
        if (lockIcon != null)
        {
            lockIcon.SetActive(!value);
        }
    }

    public void OnClick()
    {
        // ロック中は何もしない
        if (!canSelect)
        {
            Debug.Log("このステージはまだ開放されていません");
            return;
        }

        stageManager.SelectStage(stageData);
    }
}