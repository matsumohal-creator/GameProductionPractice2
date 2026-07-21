using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    // UI Elements

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


    // Target Player
    private PlayerBase player;
    public PlayerBase Target => player;

    //
    public void Initialize(PlayerBase target)
    {
        player = target;

        nameText.text = player.CharacterName;

        icon.sprite = player.Icon;

        hpBar.maxValue = player.MaxHp;
       energyBar.maxValue = player.MaxEnergy;

        UIRefresh();
    }

    //更新
    public void UIRefresh()
    {
        hpBar.value = player.CurrentHp;
        hpText.text = $"{player.CurrentHp}/{player.MaxHp}";

        energyBar.value = player.CurrentEnergy;

        shieldText.text = player.Shield.ToString();
    }
}

