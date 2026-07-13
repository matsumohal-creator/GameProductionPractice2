using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    //シングルトンの初期化
    public static BattleUIManager Instance;

    [SerializeField]
    private GameObject endTurnButton;

    [SerializeField]
    private GameObject victoryPanel;

    [SerializeField]
    private GameObject defeatPanel;

    [SerializeField]
    private TMP_Text turnText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        endTurnButton.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);

        // ターンテキストを初期化
        turnText.text = "";
    }

    //UIの切り替え
    public void RefreshUI(BattleState state)
    {
        switch (state)
        {
            case BattleState.BattleStart:

                turnText.text = "Battle Start!";
                endTurnButton.SetActive(false);
                break;

            case BattleState.PlayerInput:

                turnText.text = "Player Turn";
                endTurnButton.SetActive(true);
                break;

            case BattleState.EnemyAction:

                turnText.text = "Enemy Turn";
                endTurnButton.SetActive(false);
                break;

            case BattleState.TurnEnd:

                endTurnButton.SetActive(false);
                break;

            case BattleState.Victory:

                victoryPanel.SetActive(true);
                break;

            case BattleState.Defeat:

                defeatPanel.SetActive(true);
                break;
        }
    }
}
