using UnityEngine;

public class TurnUnit
{
    public bool isPlayer;

    public PlayerBase player;
    public EnemyBase enemy;

    public int Speed
    {
        get
        {
            if (isPlayer)
            {
                return player.Speed;
            }

            return enemy.Speed;
        }
    }
}