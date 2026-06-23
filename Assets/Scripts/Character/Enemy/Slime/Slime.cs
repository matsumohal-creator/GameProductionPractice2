using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

// スライムの行動パターン
// 1ターン目: 10ダメージの攻撃
// 2ターン目: 何もせず、ターン終了時に筋力バフ5を付与
// 3ターン目: 15ダメージの攻撃（筋力バフで強化される）
// 以降ループ
public class Slime : EnemyBase
{
    // 現在のターンカウント（1?3でループ）
    private int turnCount = 1;

    // 基本攻撃力（1ターン目、3ターン目共に10）
    [SerializeField]
    private int baseAttackDamage = 10;

    // 2ターン目に付与する筋力バフのスタック数
    [SerializeField]
    private int strengthStack = 5;

    // 筋力バフは永続（持続ターン数を999など大きな値に設定）
    [SerializeField]
    private int strengthDuration = 999;

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
                // 2ターン目: 何もしない（ターン終了時に筋力バフを付与）
                Debug.Log(name + " は力を溜めている...");
                break;

            case 3:
                // 3ターン目: 10ダメージ攻撃（筋力バフ5で15ダメージになる）
                ExecuteAttack(alivePlayers, baseAttackDamage);
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

    // ターン終了時の処理をオーバーライド
    public override void OnTurnEnd()
    {
        // 2ターン目のターン終了時に筋力バフを付与
        if (turnCount == 3) // turnCountは次のターンを指しているので3になっている
        {
            ApplyStatusEffect(StatusEffectType.Strength, strengthDuration, strengthStack);
            Debug.Log(name + " は筋力バフを得た（+" + strengthStack + "）");
        }

        // 基底クラスのターン終了処理（状態異常の進行など）
        base.OnTurnEnd();
    }
}
