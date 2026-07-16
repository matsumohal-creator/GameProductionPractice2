using UnityEngine;

[CreateAssetMenu(menuName = "Game/QuestDatabase")]
public class QuestDataBase : ScriptableObject
{
    public QuestData[] quests;

    /// <summary>
    /// BattleSceneから現在のクエストを取得する際に使用します。
    /// SaveManager.CurrentSave.currentBattleQuestIndex を渡してください。
    /// </summary>
    public QuestData GetQuest(int index)
    {
        if (index < 0 || index >= quests.Length)
        {
            Debug.LogWarning($"Quest Indexが不正です : {index}");
            return null;
        }

        return quests[index];
    }
}

/*
 * 開始時に
QuestData quest =
    questDatabase.GetQuest(
        SaveManager.CurrentSave.currentBattleQuestIndex);

と

foreach(GameObject enemy in quest.enemyPrefabs)
{
    Instantiate(enemy);
}
で使えるようにしたよ
*/