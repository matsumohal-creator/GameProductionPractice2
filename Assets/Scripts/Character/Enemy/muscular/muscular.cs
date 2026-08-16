using System.Collections.Generic;
using UnityEngine;

// muscularの行動パターン
// 1ターン目: 自身に筋力+5のバフ（攻撃なし）
// 2ターン目: 5ダメージ攻撃
// 3ターン目: 筋力+5してから基本5ダメージ攻撃（筋力バフ込みで10ダメージ）
// 以降ループ（筋力は永続で累積され続ける）
public class muscular : EnemyBase
{
    // ターンカウント（1?3でループ)
    private int turnCount = 1;

    // 基本攻撃力
    [SerializeField]
    private int baseAttackDamage = 5;

    // 付与する筋力バフのスタック数
    [SerializeField]
    private int strengthStack = 5;

    // 筋力バフの持続ターン数（999で実質永続）
    [SerializeField]
    private int strengthDuration = 999;

    public override void ExecuteTurn()
    {
        // 生存プレイヤーを取得
        // 毎ターン、生存中プレイヤーを抽出してから行動を分岐
        List<PlayerBase> players = BattleManager.Instance.Players;
        List<PlayerBase> alivePlayers = new List<PlayerBase>();

        foreach (PlayerBase player in players)
        {
            if (player.CurrentHp > 0)
            {
                alivePlayers.Add(player);
            }
        }

        switch (turnCount)
        {
            case 1:
                // 1ターン目: 自身に筋力+5のバフ、攻撃なし
                ApplyStatusEffect(StatusEffectType.Strength, strengthDuration, strengthStack);
                Debug.Log(name + " は力を蓄えた！筋力合計: " + GetStatusStack(StatusEffectType.Strength));
                break;

            case 2:
                // 2ターン目: 5ダメージ攻撃（筋力バフも乗る）
                if (alivePlayers.Count > 0)
                {
                    ExecuteAttack(alivePlayers, baseAttackDamage);
                }
                break;

            case 3:
                // 3ターン目: 筋力+5してから基本5ダメージ攻撃（合計10ダメージ）
                ApplyStatusEffect(StatusEffectType.Strength, strengthDuration, strengthStack);
                Debug.Log(name + " の筋力がさらに上がった！筋力合計: " + GetStatusStack(StatusEffectType.Strength));

                if (alivePlayers.Count > 0)
                {
                    ExecuteAttack(alivePlayers, baseAttackDamage);
                }
                break;
        }

        // ターンカウントを進める（3でループ）
        turnCount++;
        if (turnCount > 3)
        {
            turnCount = 1;
        }
    }

    // 攻撃実行（ランダムなプレイヤーを対象に、筋力バフ込みでダメージ計算）
    private void ExecuteAttack(List<PlayerBase> targets, int damage)
    {
        PlayerBase target = targets[Random.Range(0, targets.Count)];

        int finalDamage = DamageCalculator.CalculateDamage(damage, this, target);

        target.TakeDamage(finalDamage);

        Debug.Log(name + " が " + target.name + " に " + finalDamage + " のダメージを与えた");
    }
}
