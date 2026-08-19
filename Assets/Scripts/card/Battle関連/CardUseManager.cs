using UnityEngine;

public class CardUseManager : MonoBehaviour
{
    public static CardUseManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UseSelectedCard()
    {
        // プレイヤターン以外は使えない
        if (!TurnManager.Instance.IsPlayerTurn) return;

        // プレイヤー入力待ちでなければ使えない
        if (!TurnManager.Instance.IsWaitingPlayerInput) return;

        // 現在のプレイヤー取得
        PlayerBase player = TurnManager.Instance.CurrentPlayer;
        if (player == null) return;

        // 選択中カード取得
        CardView card = CardSelectionManager.Instance.SelectedCard;
        if (card == null) return;

        // エネルギー不足
        if (player.CurrentEnergy < card.SkillData.cost)
        {
            Debug.Log("エネルギー不足");
            return;
        }

        // ★修正箇所：生存している最初の敵を取得する
        // (将来的にクリックでターゲット選択した敵を入れる場合はここを差し替えます)
        EnemyBase enemy = BattleManager.Instance.GetFirstLivingEnemy();

        // 攻撃対象となる敵が1体も生存していない場合は処理中断
        if (enemy == null)
        {
            Debug.LogWarning("対象となる生存中の敵がいません");
            return;
        }

        // スキル使用
        BattleManager.Instance.UseSkill(
            player,
            card.SkillData,
            enemy);

        // 手札の取得
        HandManager hand = player.GetComponent<HandManager>();

        // 手札から削除
        hand.RemoveCard(card.SkillData);

        // 選択解除
        CardSelectionManager.Instance.Clear();
    }
}