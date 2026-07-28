using UnityEngine;
using UnityEngine.SceneManagement;

// かなり適当ですので、必要に応じて修正してください。

public class StageManager : MonoBehaviour
{
    [SerializeField]// QuestDataBaseをインスペクターで設定するための変数
    private QuestDataBase questDatabase;

    [SerializeField]// StageButtonをインスペクターで設定するための配列
    private StageButton[] stageButtons;

    [SerializeField]// QuestInfoUIをインスペクターで設定するための変数
    private QuestInfoUI questInfoUI;

    [SerializeField]// StageMarkerをインスペクターで設定するための変数
    private StageMarker stageMarker;

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
        Debug.Log("現在地点 : " + saveData.currentStageIndex);

        if (saveData.currentQuestName != "")
        {
            Debug.Log("現在クエスト : " + saveData.currentQuestName);
        }
        // 選択可能なクエストを表示する
        ShowAvailableQuests();
        stageMarker.MoveToStage(saveData.currentStageIndex);
    }

    // 選択可能なクエストを表示するメソッド
    private void ShowAvailableQuests()
    {
        // クエストデータベースから選択可能なクエストを取得して表示する
        foreach (QuestData quest in questDatabase.quests)
        {
            Debug.Log("選択可能クエスト : " + quest.questName);
        }
        // ステージボタンのインタラクティブ状態を更新する
        for (int i = 0; i < stageButtons.Length; i++)
        {
            bool canSelect = i < questDatabase.quests.Length;

            stageButtons[i].SetInteractable(canSelect);
        }
    }

    // クエストを選択するメソッド
    public void SelectQuest(int questIndex)
    {
        // クエスト情報UIが開いている場合は閉じる
        QuestData quest = questDatabase.quests[questIndex];
        // クエスト情報UIを開く
        saveData.selectedQuestIndex = questIndex;
        saveData.currentQuestName = quest.questName;
        // クエスト情報UIを表示する
        questInfoUI.ShowQuest(quest);

        Debug.Log("選択クエスト : " + quest.questName);
    }

    // クエスト情報UIを閉じるメソッド
    public void CloseQuestInfo()
    {
        questInfoUI.Clear();

        saveData.selectedQuestIndex = -1;
        saveData.currentQuestName = "";

        isQuestInfoOpen = false;
    }

    // クエストを開始するメソッド
    public void StartQuest()
    {
        if (saveData.selectedQuestIndex < 0)
        {
            Debug.LogWarning("クエストが選択されていません");
            return;
        }

        saveData.currentBattleQuestIndex = saveData.selectedQuestIndex;

        QuestData quest =
            questDatabase.quests[saveData.currentBattleQuestIndex];

        Debug.Log("戦闘開始 : " + quest.questName);
        SceneLoader.NextSceneName = "BattleScene";
        SceneManager.LoadScene("LoadingScene");
    }
}