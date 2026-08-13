using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

// ゲームマネージャー経由で簡単に呼び出せるサウンドマネージャー
// 使い方: GameManager.Sound.PlayBGM(BGMType.Title);
// 音量調整スライダーの設定も対応
public class SoundManager : MonoBehaviour
{
    [Header("BGM Audio Source")]
    [SerializeField]
    private AudioSource bgmSource;

    [Header("SE Audio Source")]
    [SerializeField]
    private AudioSource seSource;

    [Header("BGM Clips")]
    [SerializeField]
    private AudioClip titleBGM;

    [SerializeField]
    private AudioClip battleBGM;

    [SerializeField]
    private AudioClip bossBGM;

    [SerializeField]
    private AudioClip exploreBGM;

    [Header("SE Clips")]
    [SerializeField]
    private AudioClip damageSE;

    [Header("Volume Sliders (Optional)")]
    [SerializeField]
    private Slider masterSlider;

    [SerializeField]
    private Slider bgmSlider;

    [SerializeField]
    private Slider seSlider;

    // 現在の音量を外部から取得できるようにする
    public float BGMVolume => bgmSource != null ? bgmSource.volume : 1f;
    public float SEVolume => seSource != null ? seSource.volume : 1f;

    private void Awake()
    {
        // AudioSourceが未設定の場合は自動で作成
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        // BGMは常にループ再生
        bgmSource.loop = true;

        if (seSource == null)
        {
            seSource = gameObject.AddComponent<AudioSource>();
        }

        // SEはループしない
        seSource.loop = false;
    }

    private void Start()
    {
        // スライダーの初期化とリスナー登録
        InitializeSliders();
    }

    private void OnEnable()
    {
        // シーン読み込み時に自動でBGMを切り替える
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;

        // スライダーのリスナー解除
        RemoveSliderListeners();
    }

    // スライダーの初期化
    private void InitializeSliders()
    {
        if (masterSlider != null)
        {
            masterSlider.value = AudioListener.volume;
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (bgmSlider != null)
        {
            bgmSlider.value = BGMVolume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (seSlider != null)
        {
            seSlider.value = SEVolume;
            seSlider.onValueChanged.AddListener(SetSEVolume);
        }
    }

    // スライダーのリスナー解除
    private void RemoveSliderListeners()
    {
        if (masterSlider != null)
        {
            masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        }

        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(SetBGMVolume);
        }

        if (seSlider != null)
        {
            seSlider.onValueChanged.RemoveListener(SetSEVolume);
        }
    }

    // シーン読み込み時に呼ばれる
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // シーン名に応じてBGMを自動切り替え
        string sceneName = scene.name;

        if (sceneName.Contains("Title"))
        {
            PlayBGM(BGMType.Title);
        }
        else if (sceneName.Contains("Battle"))
        {
            // ボス戦かどうかは別の条件で判定が必要
            PlayBGM(BGMType.Battle);
        }
        else if (sceneName.Contains("Home") || sceneName.Contains("Stage"))
        {
            PlayBGM(BGMType.Explore);
        }

        // シーン切り替え後にスライダーを再検索して接続
        FindAndConnectSliders();
    }

    // シーン内のスライダーを検索して接続
    private void FindAndConnectSliders()
    {
        // 既存のリスナーを解除
        RemoveSliderListeners();

        // シーン内のAudioManagerやスライダーを検索
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();

        if (audioManager != null)
        {
            // AudioManagerが持っているスライダーを取得
            var masterField = audioManager.GetType().GetField("masterSlider", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var bgmField = audioManager.GetType().GetField("bgmSlider", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var seField = audioManager.GetType().GetField("seSlider", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (masterField != null)
                masterSlider = masterField.GetValue(audioManager) as Slider;
            if (bgmField != null)
                bgmSlider = bgmField.GetValue(audioManager) as Slider;
            if (seField != null)
                seSlider = seField.GetValue(audioManager) as Slider;

            // スライダーを再初期化
            InitializeSliders();
        }
    }

    // BGMを再生
    public void PlayBGM(BGMType type)
    {
        AudioClip clip = GetBGMClip(type);

        if (clip == null)
        {
            Debug.LogWarning("BGM clip not found: " + type);
            return;
        }

        // 同じBGMが再生中なら何もしない
        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // BGMを停止
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // BGMのボリュームを設定
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    // SEを再生
    public void PlaySE(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        seSource.PlayOneShot(clip);
    }

    public void PlayDamageSE()
    {
        PlaySE(damageSE);
    }

    // SEのボリュームを設定
    public void SetSEVolume(float volume)
    {
        seSource.volume = Mathf.Clamp01(volume);
    }

    // マスターボリュームを設定
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp01(volume);
    }

    // BGMタイプから対応するクリップを取得
    private AudioClip GetBGMClip(BGMType type)
    {
        switch (type)
        {
            case BGMType.Title:
                return titleBGM;
            case BGMType.Battle:
                return battleBGM;
            case BGMType.Boss:
                return bossBGM;
            case BGMType.Explore:
                return exploreBGM;
            default:
                return null;
        }
    }
}

// BGMの種類を定義
public enum BGMType
{
    Title,   // タイトル
    Battle,  // 戦闘
    Boss,    // ボス戦
    Explore  // 探索
}
