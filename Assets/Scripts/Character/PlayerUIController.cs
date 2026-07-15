using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private TMP_Text hpText;

    [SerializeField]
    private Slider energyBar;

    [SerializeField]
    private TMP_Text shieldText;

    private PlayerBase player;

    public void Initialize(PlayerBase target)
    {
        player = target;

        nameText.text = player.CharacterName;

        icon.sprite = player.Icon;

        UIRefresh();
    }

    public void UIRefresh()
    {
        hpBar.maxValue = player.MaxHp;
        hpBar.value = player.CurrentHp;

        hpText.text =
            $"{player.CurrentHp}/{player.MaxHp}";

        energyBar.maxValue = player.MaxEnergy;
        energyBar.value = player.CurrentEnergy;

        shieldText.text =
            player.Shield.ToString();
    }
}
