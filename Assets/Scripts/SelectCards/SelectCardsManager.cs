using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCardsManager : MonoBehaviour
{
    [Header("カードDB")]
    [SerializeField] private SkillDatabase skillDatabase;

    [Header("職業ごとの3枠（上から順に割り当て）")]
    [SerializeField] private List<SelectCards> fighterSlots = new();
    [SerializeField] private List<SelectCards> healerSlots = new();
    [SerializeField] private List<SelectCards> tankSlots = new();
    [SerializeField] private List<SelectCards> mageSlots = new();

    [Header("完了ボタン")]
    [SerializeField] private Button completeButton;

    [Header("未選択でも進める")]
    [SerializeField] private bool allowProceedWithoutSelection = true;

    [Header("タンクに対応するクラス（このプロジェクトではKnight）")]
    [SerializeField] private CharacterClass tankClass = CharacterClass.Knight;

    private readonly Dictionary<CharacterClass, List<SelectCards>> slotsByClass = new();
    private readonly Dictionary<CharacterClass, SelectCards> selectedByClass = new();

    // 報酬対象の職業順
    private readonly List<CharacterClass> rewardClasses = new()
    {
        CharacterClass.Fighter,
        CharacterClass.Healer,
        CharacterClass.Knight,
        CharacterClass.Mage
    };

    private void Awake()
    {
        // Tankの扱いをInspector値に同期
        rewardClasses[2] = tankClass;

        slotsByClass.Clear();
        slotsByClass[CharacterClass.Fighter] = fighterSlots;
        slotsByClass[CharacterClass.Healer] = healerSlots;
        slotsByClass[tankClass] = tankSlots;
        slotsByClass[CharacterClass.Mage] = mageSlots;
    }

    private void OnEnable()
    {
        // シーン再表示時にも確実に再生成する
        if (Application.isPlaying)
        {
            GenerateRewardCards();
        }
    }

    private void Start()
    {
        // 画面表示時にランダム候補を生成
        GenerateRewardCards();
    }

    // 各職業3枚ずつの候補を作成してUIへ表示
    public void GenerateRewardCards()
    {
        selectedByClass.Clear();

        if (skillDatabase == null)
        {
            Debug.LogError("[SelectCards] SkillDatabaseが未設定です。");
            UpdateCompleteButtonState();
            return;
        }

        if (skillDatabase.skills == null || skillDatabase.skills.Length == 0)
        {
            Debug.LogError("[SelectCards] SkillDatabase.skills が空です。");
            UpdateCompleteButtonState();
            return;
        }

        foreach (CharacterClass characterClass in rewardClasses)
        {
            List<SelectCards> slots = GetSlots(characterClass);

            if (slots == null || slots.Count == 0)
            {
                Debug.LogWarning($"[SelectCards] {characterClass} のスロットが未設定です。");
                continue;
            }

            List<SkillData> candidates = GenerateCandidates(characterClass, slots.Count);
            Debug.Log($"[SelectCards] {characterClass} 候補数: {candidates.Count}");

            for (int i = 0; i < slots.Count; i++)
            {
                SelectCards slot = slots[i];

                if (slot == null)
                {
                    Debug.LogWarning($"[SelectCards] {characterClass} Slots の Element {i} が未設定です。");
                    continue;
                }

                SkillData card = i < candidates.Count ? candidates[i] : null;
                slot.Initialize(this, characterClass, card);
            }
        }

        UpdateCompleteButtonState();
    }

    // カードクリック時：同じ職業枠では常に1枚だけ選択状態にする
    public void OnCardClicked(SelectCards clickedSlot)
    {
        if (clickedSlot == null || !clickedSlot.HasCard)
        {
            return;
        }

        CharacterClass characterClass = clickedSlot.TargetClass;
        List<SelectCards> slots = GetSlots(characterClass);

        if (slots == null)
        {
            return;
        }

        foreach (SelectCards slot in slots)
        {
            if (slot != null)
            {
                slot.SetSelected(false);
            }
        }

        clickedSlot.SetSelected(true);
        selectedByClass[characterClass] = clickedSlot;

        UpdateCompleteButtonState();
    }

    // 完了ボタン押下：選択カードのみをデッキへ追加し、未選択は追加せず進行
    public void OnClickComplete()
    {
        if (SaveManager.CurrentSave == null)
        {
            Debug.LogError("SaveDataが存在しません。");
            return;
        }

        int addedCount = 0;

        foreach (CharacterClass characterClass in rewardClasses)
        {
            if (!selectedByClass.TryGetValue(characterClass, out SelectCards selectedSlot) ||
                selectedSlot == null ||
                selectedSlot.CardData == null)
            {
                // 取得しない選択：この職業は追加せずそのまま進む
                continue;
            }

            PartyMemberData member = FindPartyMemberByClass(characterClass);

            if (member == null)
            {
                Debug.LogWarning($"{characterClass} のメンバーがパーティにいないため、カード追加をスキップしました。");
                continue;
            }

            if (member.deck == null)
            {
                member.deck = new List<SkillData>();
            }

            member.deck.Add(selectedSlot.CardData);
            addedCount++;

            Debug.Log($"[SelectCards] {characterClass} に {selectedSlot.CardData.skillName} を追加");
        }

        Debug.Log($"[SelectCards] 完了: 追加枚数={addedCount}（未選択は追加なし）");

        // LoadingSceneを経由してステージシーンへ遷移
        SceneLoader.NextSceneName = "StageScene";
        SceneManager.LoadScene("LoadingScene");
    }

    // 指定職業向けの候補カードを抽選
    private List<SkillData> GenerateCandidates(CharacterClass characterClass, int count)
    {
        List<SkillData> result = new();

        List<SkillData> classPool = new();
        List<SkillData> allPool = new();

        foreach (SkillData skill in skillDatabase.skills)
        {
            if (skill == null)
            {
                continue;
            }

            allPool.Add(skill);

            // 共通カード(None) か、その職業専用カードのみ候補にする
            if (skill.exclusiveClass == CharacterClass.None ||
                skill.exclusiveClass == characterClass)
            {
                classPool.Add(skill);
            }
        }

        // 職業一致カードが0件なら、全カードから出す（表示0件を回避）
        List<SkillData> pool = classPool;
        if (pool.Count == 0)
        {
            Debug.LogWarning($"[SelectCards] {characterClass} の職業一致カードが0件のため、全カードから抽選します。");
            pool = allPool;
        }

        if (pool.Count == 0)
        {
            return result;
        }

        // まずは重複なしで候補を取る
        List<SkillData> available = new(pool);
        int noDupCount = Mathf.Min(count, available.Count);

        for (int i = 0; i < noDupCount; i++)
        {
            int index = Random.Range(0, available.Count);
            result.Add(available[index]);
            available.RemoveAt(index);
        }

        // 3枚に足りない場合は重複ありで補完（空表示回避）
        while (result.Count < count)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
        }

        return result;
    }

    private List<SelectCards> GetSlots(CharacterClass characterClass)
    {
        if (!slotsByClass.TryGetValue(characterClass, out List<SelectCards> slots))
        {
            return null;
        }

        return slots;
    }

    // 4職業すべて選択済みか
    private bool IsAllClassSelected()
    {
        foreach (CharacterClass characterClass in rewardClasses)
        {
            if (!selectedByClass.TryGetValue(characterClass, out SelectCards selectedSlot) ||
                selectedSlot == null ||
                selectedSlot.CardData == null)
            {
                return false;
            }
        }

        return true;
    }

    // 完了ボタンは常に押せる（取得しない選択を許可）
    private void UpdateCompleteButtonState()
    {
        if (completeButton != null)
        {
            completeButton.interactable = true;
        }
    }

    // SaveData上から該当クラスのパーティメンバーを探す
    private PartyMemberData FindPartyMemberByClass(CharacterClass characterClass)
    {
        List<PartyMemberData> partyMembers = SaveManager.CurrentSave.partyMembers;

        if (partyMembers == null)
        {
            return null;
        }

        foreach (PartyMemberData member in partyMembers)
        {
            if (member == null)
            {
                continue;
            }

            if (member.characterClass == characterClass)
            {
                return member;
            }
        }

        return null;
    }
}
