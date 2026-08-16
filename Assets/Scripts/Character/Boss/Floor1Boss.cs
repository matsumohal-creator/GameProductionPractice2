using System.Collections.Generic;
using UnityEngine;

public class Floor1Boss : BossBase
{
    [SerializeField]
    private int attackDamageAll = 15;

    [SerializeField]
    private int fatigueStack = 1;

    [SerializeField]
    private int fatigueDuration = 1;

    [SerializeField]
    private int burnOnHitStack = 3;

    [SerializeField]
    private int burnOnHitDuration = 1;

    [SerializeField]
    private StatusEffectData fatigueStatusEffect;

    [SerializeField]
    private StatusEffectData burnStatusEffect;

    private int turnCount = 1;
    private bool burnOnHitActive;

    protected override void Awake()
    {
        base.Awake();

        if (fatigueStatusEffect == null)
        {
            TryGetStatusEffectData(StatusEffectType.Fatigue, out fatigueStatusEffect);
        }

        if (burnStatusEffect == null)
        {
            TryGetStatusEffectData(StatusEffectType.Burn, out burnStatusEffect);
        }
    }

    public override void ExecuteTurn()
    {
        // 行動対象は生存中プレイヤーのみ
        List<PlayerBase> alivePlayers = GetAlivePlayers();

        if (alivePlayers.Count == 0)
        {
            return;
        }

        switch (turnCount)
        {
            case 1:
                // 1ターン目: 攻撃せず、疲労デバフを全体付与
                ApplyFatigueToAllPlayers(alivePlayers);
                break;

            case 2:
                // 2ターン目: 全体15ダメージ攻撃
                ExecuteAttackAll(alivePlayers);
                break;

            case 3:
                // 3ターン目: 疲労をかけ直し、次の攻撃に「やけど3付与」を準備
                ApplyFatigueToAllPlayers(alivePlayers);
                burnOnHitActive = true;
                break;
        }

        // 1→2→3→1... の3ターンループ
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
            if (player != null && player.CurrentHp > 0)
            {
                alivePlayers.Add(player);
            }
        }

        return alivePlayers;
    }

    private void ApplyFatigueToAllPlayers(List<PlayerBase> targets)
    {
        if (fatigueStatusEffect == null)
        {
            return;
        }

        foreach (PlayerBase target in targets)
        {
            target.ApplyStatusEffect(fatigueStatusEffect, fatigueDuration, fatigueStack);
        }
    }

    private void ExecuteAttackAll(List<PlayerBase> targets)
    {
        PlayAttackAnimation();

        foreach (PlayerBase target in targets)
        {
            int finalDamage = DamageCalculator.CalculateDamage(attackDamageAll, this, target);
            target.TakeDamage(finalDamage, this);

            // 3ターン目で準備した効果が有効な間は、ボスの攻撃被弾時にやけど3を付与
            if (burnOnHitActive && burnStatusEffect != null)
            {
                target.ApplyStatusEffect(burnStatusEffect, burnOnHitDuration, burnOnHitStack);
            }
        }

        PlayIdleAnimation();

        // やけど付与は次の攻撃1回で消費
        burnOnHitActive = false;
    }
}
