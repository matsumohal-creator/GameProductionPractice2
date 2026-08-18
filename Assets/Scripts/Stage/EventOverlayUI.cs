using System.Collections.Generic;
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

    [Header("イベント効果")]
    [SerializeField]
    private EventEffectManager eventEffectManager;

    [Header("カード削除UI")]
    [SerializeField]
    private GameObject cardRemoveRoot;

    [SerializeField]
    private EventCardRemoveUI cardRemoveUI;

    [Header("カード獲得UI")]
    [SerializeField]
    private GameObject cardAddRoot;

    [SerializeField]
    private EventCardAddUI cardAddUI;

    // 今回のイベントでカード削除が必要か
    private bool pendingCardRemoval;

    // 今回のイベントで獲得したカード
    private List<EventCardAddData> pendingAcquiredCards;

    private StageManager stageManager;
    private StageNodeData currentStage;
    private EventData currentEvent;
    private EventChoiceData currentChoice;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (cardRemoveRoot != null)
        {
            cardRemoveRoot.SetActive(false);
        }

        if (resultRoot != null)
        {
            resultRoot.SetActive(false);
        }

        if (cardAddRoot != null)
        {
            cardAddRoot.SetActive(false);
        }
    }

    public void Initialize(
        StageManager manager,
        EventEffectManager effectManager)
    {
        stageManager = manager;
        eventEffectManager = effectManager;

        if (cardRemoveUI != null)
        {
            cardRemoveUI.Initialize(
                eventEffectManager,
                OnCardRemoveCompleted);
        }

        if (cardAddUI != null)
        {
            cardAddUI.Initialize(
                OnCardAddCompleted);
        }
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

        // -----------------------------------------------------
        // 固定イベントを使用
        // -----------------------------------------------------

        if (stage.useFixedEvent)
        {
            if (stage.fixedEvent == null)
            {
                Debug.LogWarning(
                    $"ステージ {stage.stageName} は固定イベント設定ですが、" +
                    "Fixed Eventが設定されていません");

                return;
            }

            currentEvent = stage.fixedEvent;

            Debug.Log(
                $"固定イベント発生: {currentEvent.eventTitle}");
        }
        // -----------------------------------------------------
        // 通常のランダムイベント
        // -----------------------------------------------------
        else
        {
            // イベント一覧が存在しない
            if (stage.eventTable == null)
            {
                Debug.LogWarning(
                    $"イベントステージ {stage.stageName} に" +
                    "EventTableが設定されていません");

                return;
            }

            // イベント一覧が空
            if (stage.eventTable.events == null ||
                stage.eventTable.events.Count == 0)
            {
                Debug.LogWarning(
                    $"イベントステージ {stage.stageName} の" +
                    "EventTableにイベントがありません");

                return;
            }

            // ランダムでイベントを1つ選択
            int randomIndex = Random.Range(
                0,
                stage.eventTable.events.Count);

            currentEvent =
                stage.eventTable.events[randomIndex];

            if (currentEvent == null)
            {
                Debug.LogWarning(
                    "選択されたEventDataがnullです");

                return;
            }

            Debug.Log(
                $"ランダムイベント発生: {currentEvent.eventTitle}");
        }


        gameObject.SetActive(true);

        if (cardRemoveRoot != null)
        {
            cardRemoveRoot.SetActive(false);
        }

        if (cardAddRoot != null)
        {
            cardAddRoot.SetActive(false);
        }

        if (resultRoot != null)
        {
            resultRoot.SetActive(false);
        }

        pendingCardRemoval = false;
        pendingAcquiredCards = null;

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

        if (resultRoot != null)
        {
            resultRoot.SetActive(false);
        }
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
        if (choiceRoot == null)
        {
            return;
        }

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

        // -----------------------------------------
        // 今回のイベント効果情報を初期化
        // -----------------------------------------

        pendingCardRemoval = false;
        pendingAcquiredCards = null;

        // -----------------------------------------
        // イベント効果を実行
        // -----------------------------------------

        if (eventEffectManager != null)
        {
            pendingCardRemoval =
                eventEffectManager.ApplyEffects(
                    choice.effects,
                    currentStage,
                    out pendingAcquiredCards);
        }

        // -----------------------------------------
        // カード獲得が必要
        // -----------------------------------------

        if (pendingAcquiredCards != null &&
              pendingAcquiredCards.Count > 0)
        {
            if (cardAddRoot != null &&
                cardAddUI != null)
            {
                cardAddRoot.SetActive(true);

                cardAddUI.Show(
                    pendingAcquiredCards);

                return;
            }

            Debug.LogWarning(
                "AddCard効果がありますが、" +
                "CardAddRoot または EventCardAddUI が設定されていません");
        }

        // -----------------------------------------
        // カード削除が必要
        // -----------------------------------------

        if (pendingCardRemoval)
        {
            ShowCardRemoveUI();
            return;
        }

        // -----------------------------------------
        // 通常の結果表示
        // -----------------------------------------

        ShowResult(choice);
    }

    // 結果を表示
    private void ShowResult(EventChoiceData choice)
    {
        resultRoot.SetActive(true);

        resultText.text =
            string.IsNullOrEmpty(choice.resultText)
                ? "何も起こらなかった……"
                : choice.resultText;
    }

    // カード獲得UIが完了したとき
    private void OnCardAddCompleted()
    {
        Debug.Log(
            "[EventEffect] カード獲得UIが完了しました");

        if (cardAddRoot != null)
        {
            cardAddRoot.SetActive(false);
        }

        // -----------------------------------------
        // 次にカード削除がある場合
        // -----------------------------------------

        if (pendingCardRemoval)
        {
            ShowCardRemoveUI();
            return;
        }

        // -----------------------------------------
        // カード削除がない場合
        // -----------------------------------------

        if (currentChoice == null)
        {
            return;
        }

        ShowResult(currentChoice);
    }

    private void OnCardRemoveCompleted()
    {
        Debug.Log(
            "[EventEffect] カード削除処理が完了しました");

        if (cardRemoveRoot != null)
        {
            cardRemoveRoot.SetActive(false);
        }

        pendingCardRemoval = false;

        if (currentChoice == null)
        {
            return;
        }

        ShowResult(currentChoice);
    }

    // カード削除UIを表示
    private void ShowCardRemoveUI()
    {
        if (cardRemoveRoot != null &&
            cardRemoveUI != null)
        {
            cardRemoveRoot.SetActive(true);

            cardRemoveUI.Show();

            return;
        }

        Debug.LogWarning(
            "RemoveCard効果がありますが、" +
            "CardRemoveRoot または EventCardRemoveUI が設定されていません");

        ShowResult(currentChoice);
    }

    public void OnClickConfirm()
    {
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
