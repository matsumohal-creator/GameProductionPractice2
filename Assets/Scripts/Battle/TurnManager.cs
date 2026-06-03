using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<PlayerBase> players;
    public List<EnemyBase> enemies;

    private Queue<TurnUnit> turnQueue;

    private TurnUnit currentUnit;

    private void Start()
    {
        StartRound();
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

        currentUnit = turnQueue.Dequeue();

        if (currentUnit.isPlayer)
        {
            Debug.Log(
                currentUnit.player.name +
                " のターン"
            );
        }
        else
        {
            Debug.Log(
                currentUnit.enemy.name +
                " のターン"
            );
        }
    }
}