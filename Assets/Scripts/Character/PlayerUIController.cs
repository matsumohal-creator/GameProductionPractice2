using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("UI Elements")]

    // プレイヤーUIの固定位置
    // 画面中央より左
    [SerializeField]
    private Vector2 fixedPosition = new Vector2(-350f, 250f);

    // 選択フレーム
    [SerializeField]
    private GameObject selectionFrame;

    // UIクリック用ボタン
    [SerializeField]
    private Button targetButton;

    // 名前
    [SerializeField]
    private TMP_Text nameText;

    // HPバー
    [SerializeField]
    private Image hpBar;

    // HPテキスト
    [SerializeField]
    private TMP_Text hpText;

    // エネルギー
    [SerializeField]
    private TMP_Text energyText;

    // シールド
    [SerializeField]
    private TMP_Text shieldText;

    // 対象プレイヤー
    private PlayerBase player;

    public PlayerBase Target => player;

    // UIのRectTransform
    private RectTransform rectTransform;

    public void Initialize(PlayerBase target)
    {
        player = target;

        rectTransform = GetComponent<RectTransform>();

        // 名前
        if (nameText != null)
        {
            nameText.text = player.CharacterName;
        }

        // UI更新
        UIRefresh();

        // 固定位置に配置
        SetFixedPosition();

        if (targetButton != null)
        {
            targetButton.onClick.RemoveAllListeners();

            targetButton.onClick.AddListener(
                OnClickPlayer
            );
        }
    }

    // UI更新
    public void UIRefresh()
    {
        if (player == null)
        {
            return;
        }

        // HP
        if (hpBar != null)
        {
            hpBar.fillAmount =
                (float)player.CurrentHp / player.MaxHp;
        }

        // HPテキスト
        if (hpText != null)
        {
            hpText.text =
                $"{player.CurrentHp}/{player.MaxHp}";
        }

        RefreshEnergy();
        RefreshShield();
    }

    // 固定位置
    private void SetFixedPosition()
    {
        if (rectTransform == null)
        {
            return;
        }

        rectTransform.anchoredPosition = fixedPosition;
    }

    // 選択状態
    public void SetSelected(bool value)
    {
        if (selectionFrame != null)
        {
            selectionFrame.SetActive(value);
        }
    }

    // エネルギー
    private void RefreshEnergy()
    {
        if (energyText == null)
        {
            return;
        }

        energyText.text =
            $"{player.CurrentEnergy}/{player.MaxEnergy}";
    }

    // シールド
    private void RefreshShield()
    {
        if (shieldText == null)
        {
            return;
        }

        bool hasShield =
            player.Shield > 0;

        shieldText.gameObject.SetActive(hasShield);

        if (hasShield)
        {
            shieldText.text =
                player.Shield.ToString();
        }
    }

    private void OnClickPlayer()
    {
        if (player == null)
        {
            return;
        }

        if (BattleTargetSelector.Instance == null)
        {
            Debug.LogError(
                "BattleTargetSelectorが存在しません"
            );

            return;
        }

        BattleTargetSelector.Instance.SelectPlayer(player);

        Debug.Log(
            $"Player UIクリック: {player.CharacterName}"
        );
    }
}