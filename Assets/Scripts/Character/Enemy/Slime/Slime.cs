using System.Collections.Generic;
using UnityEngine;

// スライムの行動パターン
// 1ターン目: 10ダメージの攻撃
// 2ターン目: 何もしない
// 3ターン目: 攻撃前に筋力+5して攻撃、攻撃後に筋力をリセット
// 以降ループ
public class Slime : EnemyBase
{
    // 現在のターンカウント（1?3でループ）
    private int turnCount = 1;

    // 基本攻撃力（1ターン目、3ターン目共に10）
    [SerializeField]
    private int baseAttackDamage = 10;

    // 3ターン目の攻撃前に付与する筋力バフのスタック数
    [SerializeField]
    private int strengthStack = 5;

    public override void ExecuteTurn()
    {
        // BattleManagerから全プレイヤーを取得
        List<PlayerBase> players = BattleManager.Instance.Players;

        // 生存プレイヤーのみ取得
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

        // ターンカウントに応じた行動
        switch (turnCount)
        {
            case 1:
                // 1ターン目: 10ダメージ攻撃
                ExecuteAttack(alivePlayers, baseAttackDamage);
                break;

            case 2:
                // 2ターン目: 何もしない
                Debug.Log(name + " は力を溜めている...");
                break;

            case 3:
                // 3ターン目: 攻撃前に筋力+5してから攻撃
                ApplyStatusEffect(StatusEffectType.Strength, 1, strengthStack);
                Debug.Log(name + " は筋力が上がった（+" + strengthStack + "）");

                ExecuteAttack(alivePlayers, baseAttackDamage);

                // 攻撃後は筋力をリセット
                RemoveStatusEffect(StatusEffectType.Strength);
                Debug.Log(name + " の筋力が元に戻った");
                break;
        }

        // ターンカウントを進める（3ターンでループ）
        turnCount++;
        if (turnCount > 3)
        {
            turnCount = 1;
        }
    }

    // 攻撃実行（ランダムなプレイヤーを対象）
    private void ExecuteAttack(List<PlayerBase> targets, int baseDamage)
    {
        if (targets.Count == 0)
        {
            return;
        }

        // ランダムにターゲットを選択
        PlayerBase target = targets[Random.Range(0, targets.Count)];

        // ダメージ計算（筋力バフを考慮）
        int finalDamage = DamageCalculator.CalculateDamage(baseDamage, this, target);

        // ダメージを与える
        target.TakeDamage(finalDamage);

        Debug.Log(name + " が " + target.name + " に " + finalDamage + " のダメージを与えた");
    }
}
