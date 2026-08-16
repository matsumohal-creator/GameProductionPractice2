using System.Collections.Generic;
using UnityEngine;

public class Boom : EnemyBase
{
    [SerializeField]
    private int attackDamage = 7;

    [SerializeField]
    private int explosionDamage = 14;

    [SerializeField]
    private int reviveCountdownTurns = 2;

    private int turnCount = 1;
    private bool isCountdownActive;
    private int remainingCountdownTurns;

    public override void ExecuteTurn()
    {
        // 通常行動中は1～3ターンループ、カウントダウン中は爆発処理を優先
        List<PlayerBase> players = BattleManager.Instance.Players;
        List<PlayerBase> alivePlayers = new List<PlayerBase>();

        foreach (PlayerBase player in players)
        {
            if (player.CurrentHp > 0)
            {
                alivePlayers.Add(player);
            }
        }

        if (alivePlayers.Count == 0)
        {
            return;
        }

        if (isCountdownActive)
        {
            remainingCountdownTurns--;

            if (remainingCountdownTurns <= 0)
            {
                ExplodeAndDie(alivePlayers);
            }
            else
            {
                Debug.Log(name + " は爆発準備中... 残り " + remainingCountdownTurns + " ターン");
            }

            return;
        }

        switch (turnCount)
        {
            case 1:
                ExecuteAttack(alivePlayers);
                break;
            case 2:
                Debug.Log(name + " は様子をうかがっている...");
                break;
            case 3:
                ExecuteAttack(alivePlayers);
                break;
        }

        turnCount++;
        if (turnCount > 3)
        {
            turnCount = 1;
        }
    }

    public override void TakeDamage(int amount, IStatusEffectTarget attacker = null)
    {
        if (isCountdownActive)
        {
            return;
        }

        base.TakeDamage(amount);

        if (CurrentHp <= 0)
        {
            StartExplosionCountdown();
        }
    }

    public override void TakeDirectDamage(int amount)
    {
        if (isCountdownActive)
        {
            return;
        }

        base.TakeDirectDamage(amount);

        if (CurrentHp <= 0)
        {
            StartExplosionCountdown();
        }
    }

    private void StartExplosionCountdown()
    {
        isCountdownActive = true;
        remainingCountdownTurns = Mathf.Max(1, reviveCountdownTurns);

        SetHp(1);
        ClearStatusEffects();

        Debug.Log(name + " は倒れた... しかし " + remainingCountdownTurns + " ターン後に爆発する");
    }

    private void ExecuteAttack(List<PlayerBase> targets)
    {
        PlayerBase target = targets[Random.Range(0, targets.Count)];
        int finalDamage = DamageCalculator.CalculateDamage(attackDamage, this, target);

        target.TakeDamage(finalDamage);

        Debug.Log(name + " が " + target.name + " に " + finalDamage + " のダメージを与えた");
    }

    private void ExplodeAndDie(List<PlayerBase> alivePlayers)
    {
        Debug.Log(name + " が爆発した！");

        foreach (PlayerBase player in alivePlayers)
        {
            player.TakeDamage(explosionDamage);
            Debug.Log(player.name + " は " + explosionDamage + " のダメージを受けた");
        }

        isCountdownActive = false;
        SetHp(0);
    }
}
