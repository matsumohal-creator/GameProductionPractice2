using System.Collections.Generic;
using UnityEngine;

// ステージノードのデータを保持するクラス
// 次に進めるステージにStage_1みたいなのを設定することで、ステージの分岐を表現できます
// 戦闘内容では、QuestDataを設定することで、どのクエストを戦闘するかを指定できます

[CreateAssetMenu(menuName = "Game/Stage Node")]
public class StageNodeData : ScriptableObject
{
    [Header("ID")]
    public int stageId;

    [Header("表示名")]
    public string stageName;

    [TextArea]
    public string description;

    [Header("ステージ種別")]
    public StageType stageType = StageType.Battle;

    [Header("イベント一覧(Event のみ使用)")]
    public EventTableData eventTable;

    [Header("出現する敵（Battle/Boss のみ使用）")]
    public List<GameObject> enemyPrefabs = new();

    [Header("次に進めるステージ")]
    public List<StageNodeData> nextStages = new List<StageNodeData>();
}