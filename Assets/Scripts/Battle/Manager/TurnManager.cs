using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //プレイヤーとエネミーのリストを保持
    private List<PlayerBase> players;
    private List<EnemyBase> enemies;

    private Queue<TurnUnit> turnQueue;

    private TurnUnit currentUnit;

    private void Start()
    {
        // BattleManagerからプレイヤーとエネミーのリストを取得
        players = BattleManager.Instance.Players;
        enemies = BattleManager.Instance.Enemies;

        StartRound(); //ターン開始
    }

    public void StartRound()
    {
        List<TurnUnit> units = new List<TurnUnit>();

        foreach (PlayerBase player in players)
        {
            if (player.CurrentHp > 0)
            {
                units.Add(new TurnUnit()
                {
                    isPlayer = true,
                    player = player
                });
            }
        }

        foreach (EnemyBase enemy in enemies)
        {
            if (enemy.CurrentHp > 0)
            {
                units.Add(new TurnUnit()
                {
                    isPlayer = false,
                    enemy = enemy
                });
            }
        }

        var sortedUnits =
            units.OrderByDescending(x => x.Speed)
                 .ToList();

        turnQueue = new Queue<TurnUnit>(sortedUnits);

        NextTurn();
    }

    public void NextTurn()
    {
        if (turnQueue.Count == 0)
        {
            Debug.Log("ラウンド終了");
            StartRound();
            return;
        }

        // 現在のターンユニットを取得
        currentUnit = turnQueue.Dequeue();

        // ターンの開始処理
        if (currentUnit.isPlayer)
        {
            StartPlayerTurn(currentUnit.player);
            Debug.Log( currentUnit.player.name +" のターン" );
        }
        else
        {
            StartEnemyTurn(currentUnit.enemy);
            Debug.Log(currentUnit.enemy.name + " のターン");
        }
    }
            
 

    // プレイヤーのターン処理
    private void StartPlayerTurn(PlayerBase player)
    {
        Debug.Log(player.name + " のターン開始");

        // エナジー回復
        player.RefillEnergy();

        // 手札取得
        HandManager hand =
            player.GetComponent<HandManager>();

        if (hand != null)
        {
            hand.DrawToFive();
        }

        // 今後
        // UI更新
        // カード操作開始
    }

    // エネミーのターン処理
    private void StartEnemyTurn(EnemyBase enemy)
    {
        Debug.Log(enemy.name + " のターン開始");

        // 後でAIを書く
    }
}