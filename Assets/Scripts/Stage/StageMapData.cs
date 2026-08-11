using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage Map")]
public class StageMapData : ScriptableObject
{
    public StageNodeData startStage;
    public StageNodeData[] allStages;
}