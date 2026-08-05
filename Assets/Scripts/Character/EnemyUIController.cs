using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    private RectTransform rectTransform;
    private Camera mainCamera;
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Vector3 uiOffset = new Vector3(0, 0, 0);

    [Header("UI")]
    //[SerializeField] private Image icon;
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

        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();

        //icon.sprite = enemy.Icon;
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

    private void LateUpdate()
    {
        if (enemy == null) return;

        Vector3 screenPos =
            mainCamera.WorldToScreenPoint(
                enemy.transform.position + uiOffset);

        gameObject.SetActive(screenPos.z > 0);

        if (screenPos.z > 0)
        {
            rectTransform.position = screenPos;
        }
    }
}