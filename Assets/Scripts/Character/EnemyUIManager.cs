using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    [Header("UIプレハブ")]
    [SerializeField]
    private EnemyUIController enemyUIPrefab;

    [Header("UI生成先")]
    [SerializeField]
    private RectTransform uiRoot;

    [Header("配置設定")]
    [SerializeField]
    private Vector2 startPosition = new Vector2(300, 0);

    [SerializeField]
    private float spacing = 180f;

    private readonly List<EnemyUIController> enemyUIs = new();

    public void CreateUI(List<EnemyBase> enemies)
    {
        // 既存UIを削除
        foreach (EnemyUIController ui in enemyUIs)
        {
            if (ui != null)
            {
                Destroy(ui.gameObject);
            }
        }

        enemyUIs.Clear();

        if (uiRoot == null)
        {
            Debug.LogError(
                "EnemyUIManager: uiRootが設定されていません"
            );

            return;
        }

        if (enemyUIPrefab == null)
        {
            Debug.LogError(
                "EnemyUIManager: enemyUIPrefabが設定されていません"
            );

            return;
        }

        // Enemy UI生成
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBase enemy = enemies[i];

            if (enemy == null)
            {
                continue;
            }

            EnemyUIController ui =
                Instantiate(
                    enemyUIPrefab,
                    uiRoot
                );

            // 初期化
            ui.Initialize(enemy);

            // 左から順番に配置
            RectTransform rect =
                ui.GetComponent<RectTransform>();

            rect.anchoredPosition =
                startPosition +
                new Vector2(i * spacing, 0);

            enemyUIs.Add(ui);
        }
    }

    public void RefreshAll()
    {
        foreach (EnemyUIController ui in enemyUIs)
        {
            if (ui != null)
            {
                ui.UIRefresh();
            }
        }
    }
}