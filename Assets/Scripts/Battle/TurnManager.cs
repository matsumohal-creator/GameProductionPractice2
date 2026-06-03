using System.Collections.Generic; // List
using System.Linq; // LINQ
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<PlayerBase> players;
    public List<EnemyBase> enemies;

    private Queue<Unit> turnQueue;

    public Unit currentUnit;

    public void StartRound()
    {
        var sortedUnits =
            units
            .Where(x => x.currentHP > 0)
            .OrderByDescending(x => x.speed)
            .ToList();

        turnQueue = new Queue<Unit>(sortedUnits);

        NextTurn();
    }

    public void NextTurn()
    {
        if (turnQueue.Count == 0)
        {
            StartRound();
            return;
        }

        currentUnit = turnQueue.Dequeue();

        Debug.Log(currentUnit.unitName + "ÇÃÉ^Å[Éì");
    }
}