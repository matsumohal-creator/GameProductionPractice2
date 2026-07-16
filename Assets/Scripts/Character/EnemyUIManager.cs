using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    [SerializeField]
    private EnemyUIController enemyUIPrefab;

    [SerializeField]
    private Transform uiRoot;

    private List<EnemyUIController> uiList = new();

    public void CreateUI(List<EnemyBase> enemies)
    {
        foreach (Transform child in uiRoot)
        {
            Destroy(child.gameObject);
        }

        uiList.Clear();

        foreach (EnemyBase enemy in enemies)
        {
            EnemyUIController ui =
                Instantiate(enemyUIPrefab, uiRoot);

            ui.Initialize(enemy);

            uiList.Add(ui);
        }
    }

    public void RefreshAll()
    {
        foreach (EnemyUIController ui in uiList)
        {
            ui.UIRefresh();
        }
    }
}