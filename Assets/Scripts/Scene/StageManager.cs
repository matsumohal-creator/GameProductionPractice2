using UnityEngine;

// かなり適当ですので、必要に応じて修正してください。

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private QuestDataBase questDatabase;

    private SaveData saveData;

    private void Start()
    {
        saveData = SaveManager.CurrentSave;

        LoadCurrentStage();
    }

    private void LoadCurrentStage()
    {
        Debug.Log("現在地点 : " + saveData.currentStageIndex);

        if (saveData.currentQuestName != "")
        {
            Debug.Log("現在クエスト : " + saveData.currentQuestName);
        }
    }

    public void SelectQuest(QuestData quest)
    {
        SaveManager.CurrentSave.currentQuestName = quest.questName;

        Debug.Log("選択クエスト : " + quest.questName);
    }
}