using TMPro;
using UnityEngine;

public class EventOverlayUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;

    [Header("選択肢")]
    [SerializeField] private Transform choiceRoot;
    [SerializeField] private EventChoiceButtonUI choiceButtonPrefab;

    [Header("結果")]
    [SerializeField] private GameObject resultRoot;
    [SerializeField] private TMP_Text resultText;

    [SerializeField] private EventEffectManager eventEffectManager;

    private StageManager stageManager;
    private StageNodeData currentStage;
    private EventData currentEvent;
    private EventChoiceData currentChoice;

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

        // イベント一覧が存在しない
        if (stage.eventTable == null)
        {
            Debug.LogWarning(
                $"イベントステージ {stage.stageName} にEventTableが設定されていません");

            return;
        }

        // イベント一覧が空
        if (stage.eventTable.events == null ||
            stage.eventTable.events.Count == 0)
        {
            Debug.LogWarning(
                $"イベントステージ {stage.stageName} のEventTableにイベントがありません");

            return;
        }

        // ランダムでイベントを1つ選択
        int randomIndex = Random.Range(
            0,
            stage.eventTable.events.Count);

        currentEvent = stage.eventTable.events[randomIndex];

        if (currentEvent == null)
        {
            Debug.LogWarning("選択されたEventDataがnullです");
            return;
        }

        gameObject.SetActive(true);

        titleText.text = currentEvent.eventTitle;

        bodyText.text = string.IsNullOrEmpty(currentEvent.eventText)
            ? "何も起こらなかった……"
            : currentEvent.eventText;

        ShowChoices();

        Debug.Log(
            $"イベント発生: {currentEvent.eventTitle}");
    }

    private void ShowChoices()
    {
        ClearChoices();

        resultRoot.SetActive(false);
        choiceRoot.gameObject.SetActive(true);

        if (currentEvent.choices == null ||
            currentEvent.choices.Count == 0)
        {
            Debug.LogWarning(
                $"イベント {currentEvent.eventTitle} に選択肢がありません");

            return;
        }

        foreach (EventChoiceData choice in currentEvent.choices)
        {
            if (choice == null)
            {
                continue;
            }

            EventChoiceButtonUI button =
                Instantiate(choiceButtonPrefab, choiceRoot);

            button.Initialize(this, choice);
        }
    }

    private void ClearChoices()
    {
        for (int i = choiceRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(choiceRoot.GetChild(i).gameObject);
        }
    }

    // 選択肢を選んだとき
    public void SelectChoice(EventChoiceData choice)
    {
        if (choice == null)
        {
            return;
        }

        currentChoice = choice;

        Debug.Log(
            $"選択肢を選択: {choice.choiceText}");

        choiceRoot.gameObject.SetActive(false);

        resultRoot.SetActive(true);

        resultText.text = string.IsNullOrEmpty(choice.resultText)
            ? "何も起こらなかった……"
            : choice.resultText;
    }

    public void OnClickConfirm()
    {
        // 選択肢の効果を適用
        if (currentChoice != null)
        {
            eventEffectManager.ApplyEffects(
                currentChoice.effects,
                currentStage);
        }

        gameObject.SetActive(false);

        if (currentStage != null)
        {
            stageManager.CompleteEventStage(currentStage);
        }

        ClearChoices();

        currentStage = null;
        currentEvent = null;
        currentChoice = null;
    }
}
