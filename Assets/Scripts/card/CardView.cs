using TMPro;
using UnityEngine;
using UnityEngine.UI;

// カードのUIを管理するクラス
// カードのUIは、カードの名前、コスト、説明、アイコンなどを表示するためのクラスです。
// カードのUIは、CardViewクラスとして実装されており、カードのデータを受け取ってUIを更新するためのSetup関数を持っています。

public class CardView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text explanationText;
    [SerializeField] private Image cardImage;

    private SkillData skillData;

    [Header("セレクト中")]
    [SerializeField]
    private GameObject selectFrame;

    public SkillData SkillData => skillData;

    public void Setup(SkillData skill)
    {
        if (skill == null)
        {
            return;
        }

        skillData = skill;

        nameText.text = skill.skillName;
        costText.text = skill.cost.ToString();
        explanationText.text = skill.description;
        cardImage.sprite = skill.icon;

        // カードの選択状態を初期化する
        SetSelected(false);
    }

    // カードがクリックされたときに呼ばれる関数
    public void OnClick()
    {
        CardSelectionManager.Instance.Select(this);
    }

    //カードの選択状態を設定する関数
    public void SetSelected(bool value)
    {
        if (selectFrame == null)
            return;

        selectFrame.SetActive(value);
    }
}