using UnityEngine;
using UnityEngine.SceneManagement;

// かなり適当ですので、必要に応じて修正してください。

public class StageManager : MonoBehaviour
{
    [SerializeField]// StageButtonをインスペクターで設定するための配列
    private StageButton[] stageButtons;

    [SerializeField]// QuestInfoUIをインスペクターで設定するための変数
    private QuestInfoUI questInfoUI;

    [SerializeField]// StageMarkerをインスペクターで設定するための変数
    private StageMarker stageMarker;

    [SerializeField]// StageMapDataをインスペクターで設定するための変数
    private StageMapData stageMap;

    // セーブデータを保持する変数
    private SaveData saveData;
    // クエスト情報UIが開いているかどうかを管理するフラグ
    private bool isQuestInfoOpen = false;

    private void Start()
    {
        if (SaveManager.CurrentSave == null)
        {
            SaveManager.CurrentSave = new SaveData();
        }

        saveData = SaveManager.CurrentSave;

        foreach (StageButton button in stageButtons)
        {
            button.Initialize(this);
        }
        // クエスト情報UIを初期化する
        questInfoUI.Initialize(this);
        // 現在のステージ情報をロードする
        LoadCurrentStage();
    }

    // 現在のステージ情報をロードするメソッド
    private void LoadCurrentStage()
    {
        StageNodeData current = GetCurrentStage();

        Debug.Log("現在地点 : " + current.stageName);

        ShowAvailableStages();

        // ロード時は瞬間移動
        stageMarker.SetPositionImmediate(current.stageId);
    }

    // 現在のステージ情報を取得するメソッド
    private StageNodeData GetCurrentStage()
    {
        foreach (StageNodeData stage in stageMap.allStages)
        {
            if (stage.stageId == saveData.currentStageId)
            {
                return stage;
            }
        }

        return stageMap.startStage;
    }

    // 選択可能なクエストを表示するメソッド
    private void ShowAvailableStages()
    {
        StageNodeData current = GetCurrentStage();

        foreach (StageButton button in stageButtons)
        {
            bool canSelect = current.nextStages.Contains(button.StageData);

            button.SetInteractable(canSelect);
        }
    }

    // クエストを選択するメソッド
    public void SelectStage(StageNodeData stage)
    {
        saveData.selectedStageId = stage.stageId;
        saveData.currentStageName = stage.stageName;

        foreach (StageButton button in stageButtons)
        {
            bool selected =
                button.StageData != null &&
                button.StageData.stageId == stage.stageId;
        }

        // マーカーを移動
        stageMarker.MoveToStage(stage.stageId);

        questInfoUI.ShowStage(stage);

        Debug.Log("選択ステージ : " + stage.stageName);
    }

    // クエスト情報UIを閉じるメソッド
    public void CloseQuestInfo()
    {
        questInfoUI.Clear();

        saveData.selectedStageId = -1;
        saveData.currentStageName = "";

        // 現在地へ戻す
        stageMarker.MoveToStage(saveData.currentStageId);

        isQuestInfoOpen = false;
    }

    // クエストを開始するメソッド
    public void StartQuest()
    {
        if (saveData.selectedStageId < 0)
        {
            Debug.LogWarning("クエストが選択されていません");
            return;
        }

        saveData.currentBattleStageId = saveData.selectedStageId;

        StageNodeData stage = GetStageById(saveData.currentBattleStageId);

        switch (stage.stageType)
        {
            case StageType.Battle:
            case StageType.Boss:
                Debug.Log("戦闘開始 : " + stage.stageName);
                SceneLoader.NextSceneName = "BattleScene";
                SceneManager.LoadScene("LoadingScene");
                break;

            case StageType.Start:
            case StageType.Event:
                Debug.Log("イベント開始 : " + stage.stageName);
                // 将来 EventScene に遷移
                // SceneManager.LoadScene("EventScene");
                break;
        }
    }

    // IDからステージ取得
    private StageNodeData GetStageById(int stageId)
    {
        foreach (StageNodeData stage in stageMap.allStages)
        {
            if (stage.stageId == stageId)
            {
                return stage;
            }
        }

        return null;
    }

    // クリア済みか
    public bool IsStageCleared(int stageId)
    {
        return saveData.clearedStageIds.Contains(stageId);
    }

    // クリア登録
    public void MarkStageCleared(int stageId)
    {
        if (!saveData.clearedStageIds.Contains(stageId))
        {
            saveData.clearedStageIds.Add(stageId);
        }
    }

    // 将来 BattleScene から呼ばれる
    public void CompleteCurrentBattleStage()
    {
        int clearedId = saveData.currentBattleStageId;

        MarkStageCleared(clearedId);

        saveData.currentStageId = clearedId;

        Debug.Log($"ステージ {clearedId} をクリアしました");

        // SaveManager.Save(); ← 将来追加
    }
}