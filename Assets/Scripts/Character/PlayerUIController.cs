using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    // UI Elements

    //アイコン
   // [SerializeField]
   // private Image icon;

    //選択フレーム
    [SerializeField]
    private GameObject selectionFrame;

    //名前
    [SerializeField]
    private TMP_Text nameText;

    //HPバー
    [SerializeField]
    private Image hpBar;

    //HPテキスト
    [SerializeField]
    private TMP_Text hpText;

    //エネルギー
    [SerializeField]
    private TMP_Text energyText;

    //シールドテキスト
    [SerializeField]
    private TMP_Text shieldText;

    //状態異常
    //[SerializeField]
    //private Transform statusIconRoot;


    // Target Player
    private PlayerBase player;
    public PlayerBase Target => player;

    //
    public void Initialize(PlayerBase target)
    {
        player = target;

        nameText.text = player.CharacterName;

        //icon.sprite = player.Icon;

       energyBar.maxValue = player.MaxEnergy;

        UIRefresh();
    }

    //UIの更新
    public void UIRefresh()
    {
        //HPバーとテキストの更新
        hpBar.fillAmount =
      (float)player.CurrentHp / player.MaxHp;

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

