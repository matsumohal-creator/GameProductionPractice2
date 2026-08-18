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

    // UIリスト
    private readonly List<PlayerUIController> playerUIs = new();

    // UI生成
    public void CreateUI(List<PlayerBase> players)
    {
        foreach (PlayerUIController ui in playerUIs)
        {
            if (ui != null)
            {
                Destroy(ui.gameObject);
            }
        }

        playerUIs.Clear();

        foreach (PlayerBase player in players)
        {
            if (player == null)
            {
                continue;
            }

            PlayerUIController ui =
                Instantiate(
                    playerUIPrefab,
                    uiRoot
                );

            ui.Initialize(player);

            playerUIs.Add(ui);
        }
    }

    // 更新
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