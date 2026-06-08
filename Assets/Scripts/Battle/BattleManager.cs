using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    //プレイヤーベース
    [Header("Players")]
    [SerializeField]
    private List<PlayerBase> players = new();

    //エネミーベース
    [Header("Enemies")]
    [SerializeField]
    private List<EnemyBase> enemies = new();

    //シングルトンの初期化
    private void Awake()
    {
        Instance = this;
    }

    public List<PlayerBase> Players => players;
    public List<EnemyBase> Enemies => enemies;

    //スキルの使用
    public void UseSkill(
    PlayerBase user,
    SkillData skill,
    IStatusEffectTarget singleTarget)
    {
        // ターゲットを指定
        List<IStatusEffectTarget> enemyTargets =
            new List<IStatusEffectTarget>();

        // ターゲットのベクトル
        List<IStatusEffectTarget> allyTargets =
            new List<IStatusEffectTarget>();

        // 敵と味方のターゲットを追加
        foreach (EnemyBase enemy in enemies)
        {
            enemyTargets.Add(enemy);
        }

        foreach (PlayerBase player in players)
        {
            allyTargets.Add(player);
        }

        // ターゲットの解決
        List<IStatusEffectTarget> targets =
            TargetResolver.Resolve(
                skill.targetType,
                user,
                singleTarget,
                enemyTargets,
                allyTargets);

        // スキルの実行
        SkillExecution.ExecuteSkill(
            user,
            targets,
            skill);

        // 戦闘結果の確認
        CheckBattleResult();
    }

    // 戦闘結果の確認
    private void CheckBattleResult()
    {
        // 全ての敵が倒されているか確認
        bool allEnemiesDead = true;

        // 敵のHPを確認
        foreach (EnemyBase enemy in enemies)
        {
            // 敵のHPが0より大きい場合、全ての敵が倒されていないと判断
            if (enemy.CurrentHp > 0)
            {
                allEnemiesDead = false;
                break;
            }
        }

        // 全ての敵が倒されている場合、勝利と判断
        if (allEnemiesDead)
        {
            Debug.Log("Victory");
            return;
        }

        // 全てのプレイヤーが倒されているか確認
        bool allPlayersDead = true;

        // プレイヤーのHPを確認
        foreach (PlayerBase player in players)
        {
            // プレイヤーのHPが0より大きい場合、全てのプレイヤーが倒されていないと判断
            if (player.CurrentHp > 0)
            {
                allPlayersDead = false;
                break;
            }
        }

        // 全てのプレイヤーが倒されている場合、敗北と判断
        if (allPlayersDead)
        {
            Debug.Log("Defeat");
        }
    }
}