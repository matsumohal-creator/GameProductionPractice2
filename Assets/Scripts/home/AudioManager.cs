using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    private void Start()
    {
        masterSlider.value = AudioListener.volume;
    }

    public void SetMasterVolume()
    {
        AudioListener.volume = masterSlider.value;
    }
}