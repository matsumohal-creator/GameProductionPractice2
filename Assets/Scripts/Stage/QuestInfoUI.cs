using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ここでは、クエスト情報を表示するUIのスクリプトを作成します。
// このスクリプトは、クエスト名、敵のリスト、開始ボタンを表示するためのもので
// クエストが選択されたときに、StageManagerから呼び出されます。

public class QuestInfoUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text questNameText;

    [SerializeField]
    private TMP_Text enemyListText;

    [SerializeField]
    private Button startButton;

    // StageManagerの参照を保持するための変数
    private StageManager stageManager;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // StageManagerから呼び出される初期化メソッド
    public void Initialize(StageManager manager)
    {
        stageManager = manager;
    }

    public void ShowStage(StageNodeData stage)
    {
        gameObject.SetActive(true);

        questNameText.text = stage.stageName;

        enemyListText.text = "";

        foreach (GameObject enemy in stage.enemyPrefabs)
        {
            if (enemy == null) continue;

            enemyListText.text += enemy.name + "\n";
        }

        startButton.interactable = true;
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }


    public void OnClickStart()
    {
        stageManager.StartQuest();
    }

}