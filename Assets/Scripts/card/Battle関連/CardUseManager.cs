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
        // プレイヤーターン以外は使えない
        if (!TurnManager.Instance.IsPlayerTurn) return;

        // プレイヤー入力待ちでなければ使えない
        if (!TurnManager.Instance.IsWaitingPlayerInput) return;

        // 現在のプレイヤー取得
        PlayerBase player = TurnManager.Instance.CurrentPlayer;
        if (player == null) return;

        //選択中カード取得
        CardView card = CardSelectionManager.Instance.SelectedCard;
        if (card == null) return;

        //エネルギー不足
        if (player.CurrentEnergy < card.SkillData.cost)
        {
            Debug.Log("エネルギー不足");
            return;
        }

        //敵1体を対象にする
        EnemyBase enemy = BattleManager.Instance.Enemies[0];

        //スキル使用
        BattleManager.Instance.UseSkill(
            player,
            card.SkillData,
            enemy);

        //手札の取得
        HandManager hand =
            player.GetComponent<HandManager>();

        //手札から削除
        hand.RemoveCard(card.SkillData);

        //選択解除
        CardSelectionManager.Instance.Clear();

      

    }
}