using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    private void Start()
    {
        // 初期値をロード
        if (masterSlider != null)
        {
            masterSlider.value = AudioListener.volume;
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (bgmSlider != null)
        {
            bgmSlider.value = GameManager.Sound != null ? GameManager.Sound.BGMVolume : 1f;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (seSlider != null)
        {
            seSlider.value = GameManager.Sound != null ? GameManager.Sound.SEVolume : 1f;
            seSlider.onValueChanged.AddListener(SetSEVolume);
        }
    }

    private void OnDestroy()
    {
        // リスナー解除
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

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetBGMVolume(float value)
    {
        if (GameManager.Sound != null)
        {
            GameManager.Sound.SetBGMVolume(value);
        }
    }

    public void SetSEVolume(float value)
    {
        if (GameManager.Sound != null)
        {
            GameManager.Sound.SetSEVolume(value);
        }
    }
}
