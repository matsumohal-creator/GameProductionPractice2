using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    [SerializeField]
    private EnemyUIController enemyUIPrefab;

    private readonly List<EnemyUIController> enemyUIs = new();

    public void CreateUI(List<EnemyBase> enemies)
    {
        foreach (EnemyBase enemy in enemies)
        {
            EnemyUIController ui =
                Instantiate(enemyUIPrefab, enemy.transform);

            ui.Initialize(enemy);

            enemyUIs.Add(ui);
        }
    }

    public void RefreshAll()
    {
        foreach (EnemyUIController ui in enemyUIs)
        {
            ui.UIRefresh();
        }
    }
}