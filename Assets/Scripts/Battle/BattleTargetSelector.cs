using UnityEngine;

public class BattleTargetSelector : MonoBehaviour
{
    public static BattleTargetSelector Instance;

    // 現在選択されているプレイヤー
    public PlayerBase SelectedPlayer { get; private set; }

    // 現在選択されている敵
    public EnemyBase SelectedEnemy { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // プレイヤーを選択
    public void SelectPlayer(PlayerBase player)
    {
        if (player == null)
        {
            return;
        }

        SelectedPlayer = player;
        SelectedEnemy = null;

        Debug.Log(
            $"[Target] Player選択: {player.CharacterName}"
        );
    }

    // 敵を選択
    public void SelectEnemy(EnemyBase enemy)
    {
        if (enemy == null)
        {
            return;
        }

        SelectedEnemy = enemy;
        SelectedPlayer = null;

        Debug.Log(
            $"[Target] Enemy選択: {enemy.CharacterName}"
        );
    }

    // 全選択解除
    public void ClearTarget()
    {
        SelectedPlayer = null;
        SelectedEnemy = null;

        Debug.Log("[Target] 選択解除");
    }
}