using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IStatusEffectTarget
{
    [Header("Base Status")]
    [SerializeField] private string characterName;
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int currentHp = 100;
    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private int currentEnergy = 3;
    [SerializeField] private int shield = 0;
    [SerializeField] private int speed = 0; //�ǉ�������



    [Header("Status Effect Master")]
    [SerializeField] private List<StatusEffectData> statusEffectMaster = new List<StatusEffectData>();

    [Header("Skills")]
    [SerializeField] private List<SkillData> skills = new List<SkillData>();

    private readonly Dictionary<StatusEffectType, StatusEffectData> statusEffectLookup = new Dictionary<StatusEffectType, StatusEffectData>();
    private readonly Dictionary<StatusEffectType, StatusEffectInstance> activeStatusEffects = new Dictionary<StatusEffectType, StatusEffectInstance>();

    // �ȉ���ǉ����܂���
    [Header("Character Class")]
    [SerializeField]
    private CharacterClass characterClass;// �e�v���C���[�̃N���X��w�肷��t�B�[���h
    public CharacterClass CharacterClass => characterClass;// �L�����N�^�[�N���X�́A�v���C���[�̖����X�L���Z�b�g���`���邽�߂̂�̂ł��B

    [Header("Default Deck")]
    [SerializeField]
    private CharacterDeckData defaultDeck;// �e�v���C���[�̃f�t�H���g�f�b�L��w�肷��t�B�[���h
    public CharacterDeckData DefaultDeck => defaultDeck;// �f�t�H���g�f�b�L�́A�v���C���[�̃N���X�ɉ����������J�[�h�Z�b�g��񋟂��邽�߂̂�̂ł��B

    [Header("Icon")]
    [SerializeField]
    private Sprite icon;

    public Sprite Icon => icon;

    [Header("UI")]
    [SerializeField]
    private Vector3 uiOffset = new Vector3(0, 2.0f, 0);

    [Header("Animation")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string idleStateName = "Idle";

    [SerializeField]
    private string attackTriggerName = "Attack";

    [SerializeField]
    private string hitTriggerName = "Hit";

    [SerializeField]
    private string deadTriggerName = "Dead";

    [SerializeField]
    private string deadStateName = "Dead";

    private bool isDead;
    private bool hasFrozenDeadAnimation;

    public Vector3 UIOffset => uiOffset;


    public string CharacterName => characterName;
    public int Speed => speed; // �ǉ�������
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public int CurrentEnergy => currentEnergy;
    public int MaxEnergy => maxEnergy;
    public int Shield => shield;
    public IReadOnlyCollection<StatusEffectInstance> ActiveStatusEffects => activeStatusEffects.Values;
    public IReadOnlyList<SkillData> Skills => skills;

    public event Action<StatusEffectInstance> StatusEffectApplied;
    public event Action<StatusEffectInstance> StatusEffectRemoved;

    // �����l��␳���A��Ԉُ�}�X�^�[��Q�ƃe�[�u��������
    protected virtual void Awake()
    {
        maxHp = Mathf.Max(1, maxHp);
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        maxEnergy = Mathf.Max(0, maxEnergy);
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator != null)
        {
            animator.speed = 1f;
        }

        hasFrozenDeadAnimation = false;
        isDead = currentHp <= 0;

        if (!isDead)
        {
            PlayIdleAnimation();
        }

        BuildStatusEffectLookup();
    }

    private void Update()
    {
        FreezeDeadAnimationIfFinished();
    }

    // Enum�Ə�Ԉُ�f�[�^�̑Ή��\��쐬����
    public void BuildStatusEffectLookup()
    {
        statusEffectLookup.Clear();

        foreach (StatusEffectData data in statusEffectMaster)
        {
            if (data == null)
            {
                continue;
            }

            statusEffectLookup[data.effectType] = data;
        }
    }

    // Enum�����Ԉُ�f�[�^��擾����
    public bool TryGetStatusEffectData(StatusEffectType type, out StatusEffectData data)
    {
        return statusEffectLookup.TryGetValue(type, out data);
    }

    // Enum�w��ŏ�Ԉُ��t�^����
    public void ApplyStatusEffect(StatusEffectType type, int duration, int stack = 1)
    {
        if (!TryGetStatusEffectData(type, out StatusEffectData data))
        {
            return;
        }

        ApplyStatusEffect(data, duration, stack);
    }

    // ��Ԉُ�f�[�^�𒼐ڎw�肵�ĕt�^����
    public void ApplyStatusEffect(StatusEffectData statusData, int duration, int stack = 1)
    {
        if (statusData == null)
        {
            return;
        }

        int clampedStack = Mathf.Max(1, stack);
        int maxStackValue = Mathf.Max(1, statusData.maxStack);
        int clampedDuration = Mathf.Max(1, duration);

        if (activeStatusEffects.TryGetValue(statusData.effectType, out StatusEffectInstance instance))
        {
            instance.stack = Mathf.Clamp(instance.stack + clampedStack, 1, maxStackValue);
            instance.remainingTurns = Mathf.Max(instance.remainingTurns, clampedDuration);
            OnStatusEffectApplied(instance);
            return;
        }

        var newInstance = new StatusEffectInstance
        {
            data = statusData,
            remainingTurns = clampedDuration,
            stack = Mathf.Clamp(clampedStack, 1, maxStackValue)
        };

        activeStatusEffects[statusData.effectType] = newInstance;
        OnStatusEffectApplied(newInstance);
    }

    // �w���Ԉُ��ێ����Ă��邩�m�F����
    public bool HasStatusEffect(StatusEffectType type)
    {
        return activeStatusEffects.ContainsKey(type);
    }

    // �w���Ԉُ�̃X�^�b�N����Ԃ�
    public int GetStatusStack(StatusEffectType type)
    {
        if (!activeStatusEffects.TryGetValue(type, out StatusEffectInstance instance))
        {
            return 0;
        }

        return instance.stack;
    }

    // �w���Ԉُ��擾����B���݂��Ȃ��ꍇ��false��Ԃ��B
    public bool TryGetStatusEffect(
    StatusEffectType type,
    out StatusEffectInstance instance)
    {
        return activeStatusEffects.TryGetValue(type, out instance);
    }

    // �w���Ԉُ��������
    public void RemoveStatusEffect(StatusEffectType type)
    {
        if (!activeStatusEffects.TryGetValue(type, out StatusEffectInstance instance))
        {
            return;
        }

        activeStatusEffects.Remove(type);
        OnStatusEffectRemoved(instance);
    }

    // �S��Ԉُ��������
    public void ClearStatusEffects()
    {
        List<StatusEffectType> keys = new List<StatusEffectType>(activeStatusEffects.Keys);
        foreach (StatusEffectType key in keys)
        {
            RemoveStatusEffect(key);
        }
    }

    // HP�𒼐ڐݒ肷��
    public void SetHp(int value)
    {
        currentHp = Mathf.Clamp(value, 0, maxHp);
    }

    // �G�i�W�[�𒼐ڐݒ肷��
    public void SetEnergy(int value)
    {
        currentEnergy = Mathf.Clamp(value, 0, maxEnergy);
    }

    // �G�i�W�[��ő�܂ŉ񕜂���
    public void RefillEnergy()
    {
        currentEnergy = maxEnergy;
    }

    // �G�i�W�[�������݁A�����ۂ�Ԃ�
    public bool TryUseEnergy(int cost)
    {
        int clampedCost = Mathf.Max(0, cost);
        if (currentEnergy < clampedCost)
        {
            return false;
        }

        currentEnergy -= clampedCost;
        return true;
    }

    // �G�i�W�[����Z����
    public void GainEnergy(int amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + Mathf.Max(0, amount), 0, maxEnergy);
    }

    // �V�[���h����Z����
    public void GainShield(int amount)
    {
        shield += Mathf.Max(0, amount);
    }

    // �_���[�W��󂯂�
    public void TakeDamage(int amount, IStatusEffectTarget attacker = null)
    {
        amount = Mathf.Max(0, amount);

        // 1) �V�[���h�Ő�Ƀ_���[�W��z��
        if (shield > 0)
        {
            int blocked = Mathf.Min(shield, amount);

            shield -= blocked;
            amount -= blocked;
        }

        // 2) HP�����O����r���āA���_���[�W�����������𔻒�
        int beforeHp = currentHp;
        currentHp = Mathf.Max(0, currentHp - amount);

        // 3) �v���C���[��HP�����������̂݃_���[�WSE��Đ�
        if (currentHp < beforeHp)
        {
            GameManager.Sound?.PlayDamageSE();

            if (currentHp <= 0)
            {
                PlayDeadAnimation();
            }
            else
            {
                PlayHitAnimation();
            }
        }

        // ��e���̏�Ԉُ폈��
        // 4) ��e���g���K�[�i���˂Ȃǁj����s
        OnDamaged(attacker);
    }

    // ��Ԉُ�ɂ��_���[�W�̂��炵��l�����ă_���[�W��󂯂�
    public void ReceiveDamage(int amount, IStatusEffectTarget attacker = null)
    {
        PlayerBase redirect =
            StatusEffectManager.RedirectDamageTarget(this);

        if (redirect != this)
        {
            redirect.TakeDamage(amount, attacker);
            return;
        }

        TakeDamage(amount, attacker);
    }

    public void OnDamaged(IStatusEffectTarget attacker)
    {
        StatusEffectManager.OnDamaged(this, attacker);
    }

    // �V�[���h�����_���[�W
    public void TakeDirectDamage(int amount)
    {
        int beforeHp = currentHp;
        currentHp = Mathf.Max(0, currentHp - Mathf.Max(0, amount));

        if (currentHp < beforeHp)
        {
            if (currentHp <= 0)
            {
                PlayDeadAnimation();
            }
            else
            {
                PlayHitAnimation();
            }
        }
    }

    // ���g��HP������i�Œ�1HP�͎c���j
    public int ConsumeHpPercent(int percent)
    {
        percent = Mathf.Clamp(percent, 0, 100);

        // �����
        int consume = maxHp * percent / 100;

        // ����HP-1�܂ł������点�Ȃ�
        consume = Mathf.Min(consume, currentHp - 1);

        // HP1�Ȃ����ł��Ȃ�
        if (consume <= 0)
        {
            return 0;
        }

        currentHp -= consume;

        // ���ۂɌ������l��Ԃ�
        return consume;
    }

    // HP��񕜂���
    public void Heal(int amount)
    {
        // �񕜗ʂ�0�ȏ�ɕ␳
        amount = Mathf.Max(0, amount);
        // HealBoost��Ԉُ킪����ꍇ�A�񕜗ʂ�1.5�{�ɂ���
        int healBoost = GetStatusStack(StatusEffectType.HealBoost);

        if (healBoost > 0)
        {
            amount = Mathf.RoundToInt(amount * 1.5f);
        }
        // �񕜗ʂ�ő�HP�𒴂��Ȃ��悤�ɕ␳
        currentHp = Mathf.Min(maxHp, currentHp + amount);

        if (!isDead)
        {
            PlayIdleAnimation();
        }
    }

    // �^�[���I��������
    public void PlayAttackAnimation()
    {
        if (isDead)
        {
            return;
        }

        SetTrigger(attackTriggerName);
    }

    public void PlayHitAnimation()
    {
        if (isDead)
        {
            return;
        }

        SetTrigger(hitTriggerName);
    }

    public void PlayDeadAnimation()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (animator != null)
        {
            animator.speed = 1f;
        }

        SetTrigger(deadTriggerName);
    }

    public void PlayIdleAnimation()
    {
        if (animator == null || isDead)
        {
            return;
        }

        animator.speed = 1f;

        int stateHash = Animator.StringToHash(idleStateName);
        if (animator.HasState(0, stateHash))
        {
            animator.Play(stateHash, 0, 0f);
        }
    }

    private void FreezeDeadAnimationIfFinished()
    {
        if (animator == null || !isDead || hasFrozenDeadAnimation || string.IsNullOrEmpty(deadStateName))
        {
            return;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        int deadStateHash = Animator.StringToHash(deadStateName);
        bool isDeadState = stateInfo.shortNameHash == deadStateHash || stateInfo.IsName(deadStateName);

        if (!isDeadState)
        {
            return;
        }

        // アニメーターを完全に停止
        animator.speed = 0f;

        // 全てのトリガーをリセット
        ResetAnimatorParameters();

        // アニメーション状態を最後のフレームに確実に固定
        animator.Play(stateInfo.shortNameHash, 0, 1f);

        hasFrozenDeadAnimation = true;
    }

    private void ResetAnimatorParameters()
    {
        if (animator == null)
        {
            return;
        }

        // 全てのパラメータをリセット
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            switch (param.type)
            {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(param.name, false);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(param.name, 0);
                    break;
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(param.name, 0f);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.ResetTrigger(param.name);
                    break;
            }
        }
    }

    private void SetTrigger(string triggerName)
    {
        if (animator == null || string.IsNullOrEmpty(triggerName))
        {
            return;
        }

        animator.SetTrigger(triggerName);
    }

    public void OnTurnEnd()
    {
        TickStatusEffects();

        // �^�[���I�����ɃV�[���h����Z�b�g
        shield = 0;
    }

    // ��Ԉُ�̃^�[���o�ߏ�����s��
    public void TickStatusEffects()
    {
        List<StatusEffectType> expired = new List<StatusEffectType>();

        foreach (KeyValuePair<StatusEffectType, StatusEffectInstance> pair in activeStatusEffects)
        {
            StatusEffectManager.ApplyTurnTick(this, pair.Value);
            pair.Value.remainingTurns--;

            if (pair.Value.remainingTurns <= 0 || pair.Value.stack <= 0)
            {
                expired.Add(pair.Key);
            }
        }

        foreach (StatusEffectType type in expired)
        {
            RemoveStatusEffect(type);
        }
    }

    // ��Ԉُ�t�^���̒ʒm�t�b�N
    protected virtual void OnStatusEffectApplied(StatusEffectInstance instance)
    {
        StatusEffectApplied?.Invoke(instance);
    }

    // ��Ԉُ������̒ʒm�t�b�N
    protected virtual void OnStatusEffectRemoved(StatusEffectInstance instance)
    {
        StatusEffectRemoved?.Invoke(instance);
    }
}