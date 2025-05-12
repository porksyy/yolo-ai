using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;


    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioClip sfxButtonClick;

    private float lastPlayTime;
    private float playCooldown = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 1);
        }

        Load();

        bgmSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { ChangeSFXVolume(); });

    }

    public void ChangeVolume()
    {
        float volume = bgmSlider.value;
        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        Save();
    }

    public void ChangeSFXVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);

        // Update the SFX AudioSource volume
        sfxAudioSource.volume = sfxSlider.value;

        // Optional: Limit how often the click plays to avoid spam
        if (Time.time - lastPlayTime > playCooldown)
        {
            sfxAudioSource.PlayOneShot(sfxButtonClick, sfxSlider.value);
            lastPlayTime = Time.time;
        }
        Save();
    }

    private void Load()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = sfxSlider.value;
        }
            
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", bgmSlider.value);
    }

}
