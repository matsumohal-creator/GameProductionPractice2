using UnityEngine;

// かなり適当ですので、必要に応じて修正してください。

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private QuestDataBase questDatabase;

    [SerializeField]
    private StageButton[] stageButtons;

    private SaveData saveData;

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

        LoadCurrentStage();
    }

    private void LoadCurrentStage()
    {
        Debug.Log("現在地点 : " + saveData.currentStageIndex);

        if (saveData.currentQuestName != "")
        {
            Debug.Log("現在クエスト : " + saveData.currentQuestName);
        }

        ShowAvailableQuests();

    }

    private void ShowAvailableQuests()
    {
        foreach (QuestData quest in questDatabase.quests)
        {
            Debug.Log("選択可能クエスト : " + quest.questName);
        }

        for (int i = 0; i < stageButtons.Length; i++)
        {
            bool canSelect = i < questDatabase.quests.Length;

            stageButtons[i].SetInteractable(canSelect);
        }
    }

    public void SelectQuest(int questIndex)
    {
        QuestData quest = questDatabase.quests[questIndex];

        saveData.selectedQuestIndex = questIndex;
        saveData.currentQuestName = quest.questName;

        Debug.Log("選択クエスト : " + quest.questName);
    }
}