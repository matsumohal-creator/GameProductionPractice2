using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    // UI Elements

    //アイコン
    [SerializeField]
    private Image icon;

    //選択フレーム
    [SerializeField]
    private GameObject selectionFrame;

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

    //UIの更新
    public void UIRefresh()
    {
        hpBar.value = player.CurrentHp;
        hpText.text = $"{player.CurrentHp}/{player.MaxHp}";

        energyBar.value = player.CurrentEnergy;

        RefreshShield();
    }

    //キャラクターの選択状態
    public void SetSelected(bool value)
    {
        selectionFrame.SetActive(value);
    }

    //シールドの更新
    private void RefreshShield()
    {
        bool hasShield = player.Shield > 0;

        shieldText.gameObject.SetActive(hasShield);

        if (hasShield)
        {
            shieldText.text = player.Shield.ToString();
        }
    }
}

