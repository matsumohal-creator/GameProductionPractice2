using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject selectionFrame;

    [SerializeField]
    private Button targetButton;

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private Image hpFill;

    [SerializeField]
    private TMP_Text hpText;

    [SerializeField]
    private TMP_Text shieldText;

    [SerializeField]
    private Transform statusIconRoot;

    // 対象Enemy
    private EnemyBase enemy;

    public EnemyBase Target => enemy;

    public void Initialize(EnemyBase target)
    {
        enemy = target;

        if (enemy == null)
        {
            Debug.LogError("EnemyUIController: Enemyがnullです");
            return;
        }

        // 名前
        if (nameText != null)
        {
            nameText.text = enemy.CharacterName;
        }

        UIRefresh();

        if (targetButton != null)
        {
            targetButton.onClick.RemoveAllListeners();

            targetButton.onClick.AddListener(
                OnClickEnemy
            );
        }
    }

    // UI更新
    public void UIRefresh()
    {
        if (enemy == null)
        {
            return;
        }

        // HP
        if (hpFill != null)
        {
            hpFill.fillAmount =
                (float)enemy.CurrentHp / enemy.MaxHp;
        }

        // HPテキスト
        if (hpText != null)
        {
            hpText.text =
                $"{enemy.CurrentHp}/{enemy.MaxHp}";
        }

        RefreshShield();
        RefreshDeadState();
    }

    // 選択状態
    public void SetSelected(bool value)
    {
        if (selectionFrame != null)
        {
            selectionFrame.SetActive(value);
        }
    }

    // シールド
    private void RefreshShield()
    {
        if (shieldText == null)
        {
            return;
        }

        bool hasShield = enemy.Shield > 0;

        shieldText.gameObject.SetActive(hasShield);

        if (hasShield)
        {
            shieldText.text =
                enemy.Shield.ToString();
        }
    }

    // 戦闘不能表示
    private void RefreshDeadState()
    {
        CanvasGroup canvasGroup =
            GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            return;
        }

        bool dead = enemy.CurrentHp <= 0;

        canvasGroup.alpha =
            dead ? 0.4f : 1f;
    }

    private void OnClickEnemy()
    {
        if (enemy == null)
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

        BattleTargetSelector.Instance.SelectEnemy(enemy);

        Debug.Log(
            $"Enemy UIクリック: {enemy.CharacterName}"
        );
    }
}