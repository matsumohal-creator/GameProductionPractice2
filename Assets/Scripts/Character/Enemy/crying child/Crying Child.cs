using System.Collections.Generic;
using UnityEngine;

public class CryingChild : EnemyBase
{
    [SerializeField]
    private int baseAttackDamage = 12;

    [SerializeField]
    private int weaknessStack = 2;

    [SerializeField]
    private int weaknessDuration = 1;

    [SerializeField]
    private StatusEffectData weaknessStatusEffect;

    private int turnCount = 1;

    protected override void Awake()
    {
        base.Awake();

        if (weaknessStatusEffect == null)
        {
            TryGetStatusEffectData(StatusEffectType.Weakness, out weaknessStatusEffect);
        }
    }

    public override void ExecuteTurn()
    {
        List<PlayerBase> alivePlayers = GetAlivePlayers();

        if (alivePlayers.Count == 0)
        {
            return;
        }

        switch (turnCount)
        {
            case 1:
                ApplyWeaknessToAllPlayers(alivePlayers);
                break;

            case 2:
                ExecuteAttack(alivePlayers);
                break;

            case 3:
                ApplyWeaknessToAllPlayers(alivePlayers);
                break;
        }

        turnCount++;
        if (turnCount > 3)
        {
            turnCount = 1;
        }
    }

    private List<PlayerBase> GetAlivePlayers()
    {
        List<PlayerBase> alivePlayers = new List<PlayerBase>();

        foreach (PlayerBase player in BattleManager.Instance.Players)
        {
            if (player.CurrentHp > 0)
            {
                alivePlayers.Add(player);
            }
        }

        return alivePlayers;
    }

    private void ApplyWeaknessToAllPlayers(List<PlayerBase> targets)
    {
        if (weaknessStatusEffect == null)
        {
            return;
        }

        foreach (PlayerBase target in targets)
        {
            target.ApplyStatusEffect(weaknessStatusEffect, weaknessDuration, weaknessStack);
        }

        Debug.Log(name + " がプレイヤー全体に脱力" + weaknessStack + "を付与した");
    }

    private void ExecuteAttack(List<PlayerBase> targets)
    {
        PlayerBase target = targets[Random.Range(0, targets.Count)];
        int finalDamage = DamageCalculator.CalculateDamage(baseAttackDamage, this, target);

        target.TakeDamage(finalDamage);

        Debug.Log(name + " が " + target.name + " に " + finalDamage + " のダメージを与えた");
    }
}
