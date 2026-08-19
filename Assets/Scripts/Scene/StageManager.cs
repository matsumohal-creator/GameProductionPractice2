using System.Collections;
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

    [SerializeField]// StageLineDrawerをインスペクターで設定するための変数
    private StageLineDrawer stageLineDrawer;

    [SerializeField]// EventOverlayUIをインスペクターで設定するための変数
    private EventOverlayUI eventOverlayUI;

    [SerializeField]
    private EventEffectManager eventEffectManager;

    [SerializeField]
    private DeckListUI deckListUI;

    [SerializeField]
    private PartyHpUI partyHpUI;

    // セーブデータを保持する変数
    private SaveData saveData;
    // クエスト情報UIが開いているかどうかを管理するフラグ
    private bool isQuestInfoOpen = false;

    // ゲームクリア時のテキスト表示用のGameObject
    [SerializeField]
    private GameObject demoClearPanel;

    [SerializeField]
    private float gameClearWaitTime = 3.0f;

    private void Start()
    {
        // CurrentSaveをそのまま参照して、ステージ進行データを共有します。
        saveData = SaveManager.CurrentSave;

        if (demoClearPanel != null)
        {
            demoClearPanel.SetActive(false);
        }

        // 開始地点をクリア済み扱い
        if (!saveData.clearedStageIds.Contains(stageMap.startStage.stageId))
        {
            saveData.clearedStageIds.Add(stageMap.startStage.stageId);
        }

        foreach (StageButton button in stageButtons)
        {
            button.Initialize(this);
        }
        // クエスト情報UIを初期化する
        questInfoUI.Initialize(this);

        // イベントUIを初期化
        eventOverlayUI.Initialize(this, eventEffectManager);

        if (partyHpUI != null)
        {
            partyHpUI.Refresh();
        }

        // 現在のステージ情報をロードする
        LoadCurrentStage();
    }

    private void Update()
    {
        // F1で現在選択中のステージをクリア扱いにする
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugUnlockSelectedStage();
        }
    }

    public void ShowDeckList()
    {
        if (deckListUI == null)
        {
            Debug.LogWarning("DeckListUIが設定されていません");
            return;
        }

        deckListUI.ShowDeck();
    }

    // デバッグ用：選択中ステージをクリア扱いにする
    private void DebugUnlockSelectedStage()
    {
        if (saveData.selectedStageId < 0)
        {
            return;
        }

        // 選択中ステージを「戦闘したステージ」として扱う
        saveData.currentBattleStageId = saveData.selectedStageId;

        // 本来は BattleScene 勝利後に呼ばれる処理
        CompleteCurrentBattleStage();

        // UIを更新
        ShowAvailableStages();

        // ステージ間の線を再描画
        if (stageLineDrawer != null)
        {
            stageLineDrawer.DrawLines();
        }

        // 現在地マーカーを更新
        stageMarker.SetPositionImmediate(saveData.currentStageId);
    }

    // 現在のステージ情報をロードするメソッド
    private void LoadCurrentStage()
    {
        StageNodeData current = GetCurrentStage();

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

            // クリア済みかどうかを確認
            bool cleared = IsStageCleared(button.StageData.stageId);
            button.SetCleared(cleared);
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
            return;
        }

        saveData.currentBattleStageId = saveData.selectedStageId;

        StageNodeData stage = GetStageById(saveData.currentBattleStageId);

        switch (stage.stageType)
        {
            case StageType.Battle:
            case StageType.Boss:

                // BattleSceneへ移動する準備
                GameManager.SetupBattle(GameManager.selectedFlgs);

                // LoadingSceneを経由してBattleSceneへ
                SceneLoader.NextSceneName = "BattleScene";
                SceneManager.LoadScene("LoadingScene");

                break;

            case StageType.Start:
            case StageType.Event:
                // クエスト情報を閉じる
                CloseQuestInfo();

                // オーバーレイ表示
                eventOverlayUI.ShowEvent(stage);
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

        StageNodeData clearedStage = GetStageById(clearedId);

        if (clearedStage == null)
        {
            Debug.LogWarning($"クリア対象のステージが見つかりません : {clearedId}");
            return;
        }

        // ステージをクリア済みに登録
        MarkStageCleared(clearedId);

        // 現在位置を更新
        saveData.currentStageId = clearedId;

        // ゲームクリア判定
        if (clearedStage.isGameClearStage)
        {
            GameClear();
            return;
        }

        ShowAvailableStages();
        stageMarker.SetPositionImmediate(saveData.currentStageId);

        // ステージ間の線を再描画
        if (stageLineDrawer != null)
        {
            stageLineDrawer.DrawLines();
        }

        // SaveManager.Save(); ← 将来追加
    }

    public void CompleteEventStage(StageNodeData stage)
    {
        saveData.currentBattleStageId = stage.stageId;

        CompleteCurrentBattleStage();
    }

    private void GameClear()
    {
        StartCoroutine(GameClearSequence());
    }

    private IEnumerator GameClearSequence()
    {
        // 体験版終了メッセージを表示
        if (demoClearPanel != null)
        {
            demoClearPanel.SetActive(true);
        }

        // 指定時間待つ
        yield return new WaitForSeconds(gameClearWaitTime);

        // タイトルシーンへ戻る
        SceneLoader.NextSceneName = "TitleScene";
        SceneManager.LoadScene("LoadingScene");
    }
}