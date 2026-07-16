using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    //アイコン
    [SerializeField]
    private Image icon;

    //名前
    [SerializeField]
    private TMP_Text nameText;

    //HPバー
    [SerializeField]
    private Slider hpBar;

    //HPテキスト
    [SerializeField]
    private TMP_Text hpText;

    //エネルギーバー
    [SerializeField]
    private Slider energyBar;

    //シールドテキスト
    [SerializeField]
    private TMP_Text shieldText;

    //状態異常
    [SerializeField]
    private Transform statusIconRoot;

    // Target Enemy
    private EnemyBase enemy;
    private EnemyBase Target => enemy;

    public void Initialize(EnemyBase target)
    {
        enemy = target;

        nameText.text = enemy.CharacterName;
        icon.sprite = enemy.Icon;

        UIRefresh();
    }

    public void UIRefresh()
    {
        hpBar.maxValue = enemy.MaxHp;
        hpBar.value = enemy.CurrentHp;

        hpText.text =
            $"{enemy.CurrentHp}/{enemy.MaxHp}";

        shieldText.text =
            enemy.Shield.ToString();
    }
}