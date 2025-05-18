using UnityEngine;
using UnityEngine.UI;

public class SettingsPrefab : MonoBehaviour
{
    void OnEnable()
    {
        // Only do this if SoundManager exists
        if (SoundManager.instance == null) return;

        // Navigate through the hierarchy to find the sliders
        Slider bgmSlider = transform.Find("PnlContainer/SoundContainer/SliderBgm").GetComponent<Slider>();
        Slider sfxSlider = transform.Find("PnlContainer/SfxContainer/SliderSfx").GetComponent<Slider>();

        // Assign sliders to SoundManager
        SoundManager.instance.SetSliders(bgmSlider, sfxSlider);
    }
}
