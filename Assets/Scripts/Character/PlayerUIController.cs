using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    private RectTransform rectTransform;
    private Camera mainCamera;

    [Header("UI Elements")]
    [SerializeField]
    private Vector3 uiOffset = new Vector3(0, 2, 0);

    // 選択フレーム
    [SerializeField]
    private GameObject selectionFrame;

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

    // UIを置くCanvas
    private Canvas canvas;

    public void Initialize(PlayerBase target)
    {
        player = target;

        rectTransform = GetComponent<RectTransform>();

        mainCamera = Camera.main;

        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError(
                "PlayerUIController: 親Canvasが見つかりません"
            );

            return;
        }

        nameText.text = player.CharacterName;

        UIRefresh();

        FollowTarget();
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

        if (hpText != null)
        {
            hpText.text =
                $"{player.CurrentHp}/{player.MaxHp}";
        }

        RefreshEnergy();
        RefreshShield();
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        FollowTarget();
    }

    // キャラクターの位置にUIを追従
    private void FollowTarget()
    {
        if (player == null ||
            mainCamera == null ||
            canvas == null)
        {
            return;
        }

        Vector3 worldPos =
            player.transform.position +
            player.UIOffset;

        Vector3 screenPos =
            mainCamera.WorldToScreenPoint(worldPos);

        RectTransform canvasRect =
            canvas.GetComponent<RectTransform>();

        Camera canvasCamera =
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvasCamera,
            out Vector2 localPos))
        {
            rectTransform.localPosition = localPos;
        }
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
}