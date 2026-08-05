using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [Header("UIプレハブ")]
    [SerializeField]
    private PlayerUIController playerUIPrefab;

    //UIリスト
    private readonly List<PlayerUIController> playerUIs = new();

    //UI生成
    public void CreateUI(List<PlayerBase> players)
    {
        foreach (PlayerUIController ui in playerUIs)
        {
            Destroy(ui.gameObject);
        }

        playerUIs.Clear();

        foreach (PlayerBase player in players)
        {
            PlayerUIController ui =
          Instantiate(
          playerUIPrefab,
          player.transform);

            ui.Initialize(player);

            playerUIs.Add(ui);
        }
    }

    //更新
    public void RefreshAll()
    {
        foreach (PlayerUIController ui in playerUIs)
        {
            ui.UIRefresh();
        }
    }
}

