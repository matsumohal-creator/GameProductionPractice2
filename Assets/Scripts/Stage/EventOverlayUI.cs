using TMPro;
using UnityEngine;

public class EventOverlayUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;

    private StageManager stageManager;
    private StageNodeData currentStage;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(StageManager manager)
    {
        stageManager = manager;
    }

    public void ShowEvent(StageNodeData stage)
    {
        currentStage = stage;

        gameObject.SetActive(true);

        titleText.text = stage.stageName;
        bodyText.text = string.IsNullOrEmpty(stage.eventText)
            ? "âΩÇ‡ãNÇ±ÇÁÇ»Ç©Ç¡ÇΩÅcÅc"
            : stage.eventText;
    }

    public void OnClickConfirm()
    {
        gameObject.SetActive(false);

        if (currentStage != null)
        {
            stageManager.CompleteEventStage(currentStage);
        }
    }
}
