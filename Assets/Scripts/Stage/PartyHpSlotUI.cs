using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyHpSlotUI : MonoBehaviour
{
    [Header("キャラクターアイコン")]
    [SerializeField]
    private Image characterIcon;

    [Header("HPバー")]
    [SerializeField]
    private Slider hpSlider;

    [Header("HPテキスト")]
    [SerializeField]
    private TMP_Text hpText;

    // キャラクター情報を表示
    public void Setup(PartyMemberData member, Sprite icon)
    {
        if (member == null)
        {
            return;
        }

        // キャラクターアイコン
        if (characterIcon != null)
        {
            characterIcon.sprite = icon;
        }

        // HPバー
        if (hpSlider != null)
        {
            hpSlider.maxValue = member.maxHp;
            hpSlider.value = member.currentHp;
        }

        // HPテキスト
        if (hpText != null)
        {
            hpText.text = $"{member.currentHp} / {member.maxHp}";
        }
    }
}