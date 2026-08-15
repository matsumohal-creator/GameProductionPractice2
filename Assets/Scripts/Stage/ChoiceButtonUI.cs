using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventChoiceButtonUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text choiceText;

    [SerializeField]
    private Button button;

    private EventOverlayUI eventOverlayUI;
    private EventChoiceData choiceData;

    public void Initialize(
        EventOverlayUI overlayUI,
        EventChoiceData data)
    {
        eventOverlayUI = overlayUI;
        choiceData = data;

        choiceText.text = data.choiceText;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        eventOverlayUI.SelectChoice(choiceData);
    }
}