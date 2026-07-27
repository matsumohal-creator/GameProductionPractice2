using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selectionFrame;

    [SerializeField] private TMP_Text nameText;

    [SerializeField] private Image hpFill;
    [SerializeField] private TMP_Text hpText;

    [SerializeField] private TMP_Text shieldText;

    [SerializeField] private Transform statusIconRoot;

    private EnemyBase enemy;
    public EnemyBase Target => enemy;

    public void Initialize(EnemyBase target)
    {
        enemy = target;

        icon.sprite = enemy.Icon;
        nameText.text = enemy.CharacterName;

        UIRefresh();
    }

    public void UIRefresh()
    {
        hpFill.fillAmount =
            (float)enemy.CurrentHp / enemy.MaxHp;

        hpText.text =
            $"{enemy.CurrentHp}/{enemy.MaxHp}";

        RefreshShield();

        RefreshDeadState();
    }

    public void SetSelected(bool value)
    {
        selectionFrame.SetActive(value);
    }

    private void RefreshShield()
    {
        bool hasShield = enemy.Shield > 0;

        shieldText.gameObject.SetActive(hasShield);

        if (hasShield)
            shieldText.text = enemy.Shield.ToString();
    }

    private void RefreshDeadState()
    {
        bool dead = enemy.CurrentHp <= 0;

        GetComponent<CanvasGroup>().alpha =
            dead ? 0.4f : 1f;
    }
}