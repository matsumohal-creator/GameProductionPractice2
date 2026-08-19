using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [Header("UIプレハブ")]
    [SerializeField]
    private PlayerUIController playerUIPrefab;

    [Header("UI生成先")]
    [SerializeField]
    private RectTransform uiRoot;

    [Header("配置設定")]
    [SerializeField]
    private Vector2 startPosition = new Vector2(-600f, 200f);

    [SerializeField]
    private float spacing = 180f;

    // UIリスト
    private readonly List<PlayerUIController> playerUIs = new();

    // UI生成
    public void CreateUI(List<PlayerBase> players)
    {
        // 既存UIを削除
        foreach (PlayerUIController ui in playerUIs)
        {
            if (ui != null)
            {
                Destroy(ui.gameObject);
            }
        }

        playerUIs.Clear();

        if (uiRoot == null)
        {
            Debug.LogError(
                "PlayerUIManager: uiRootが設定されていません"
            );

            return;
        }

        if (playerUIPrefab == null)
        {
            Debug.LogError(
                "PlayerUIManager: playerUIPrefabが設定されていません"
            );

            return;
        }

        // プレイヤーUIを生成
        for (int i = 0; i < players.Count; i++)
        {
            PlayerBase player = players[i];

            if (player == null)
            {
                continue;
            }

            PlayerUIController ui =
                Instantiate(
                    playerUIPrefab,
                    uiRoot
                );

            // 初期化
            ui.Initialize(player);

            // 左から順番に配置
            RectTransform rect =
                ui.GetComponent<RectTransform>();

            rect.anchoredPosition =
                startPosition +
                new Vector2(i * spacing, 0);

            playerUIs.Add(ui);
        }
    }

    // UI更新
    public void RefreshAll()
    {
        foreach (PlayerUIController ui in playerUIs)
        {
            if (ui != null)
            {
                ui.UIRefresh();
            }
        }
    }
}