using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Quest")]
public class QuestData : ScriptableObject
{
    [Header("クエスト名")]
    public string questName;

    [Header("出現する敵")]
    public List<GameObject> enemyPrefabs = new();
    //public List<EnemyBase> enemies = new List<EnemyBase>();
}
