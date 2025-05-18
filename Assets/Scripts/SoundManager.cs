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
    
    public static SoundManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", 1);

        if (!PlayerPrefs.HasKey("sfxVolume"))
            PlayerPrefs.SetFloat("sfxVolume", 1);

        if (bgmSlider != null && sfxSlider != null)
        {
            Load();
            bgmSlider.onValueChanged.AddListener(ChangeVolume);
            sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        }
    }


    public void SetSliders(Slider bgm, Slider sfx)
    {
        // Remove previous listeners first
        if (bgmSlider != null)
            bgmSlider.onValueChanged.RemoveListener(ChangeVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(ChangeSFXVolume);

        // Assign new sliders
        bgmSlider = bgm;
        sfxSlider = sfx;

        // Set current volume values
        Load();

        // Add listeners (non-delegate, cleaner)
        bgmSlider.onValueChanged.AddListener(ChangeVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }



    public void ChangeVolume(float value)
    {
        musicAudioSource.volume = value;
        PlayerPrefs.SetFloat("musicVolume", value);
        Save();
    }

    public void ChangeSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("sfxVolume", value);
        sfxAudioSource.volume = value;

        if (Time.time - lastPlayTime > playCooldown)
        {
            sfxAudioSource.PlayOneShot(sfxButtonClick, value);
            lastPlayTime = Time.time;
        }

        Save();
    }


    private void Load()
    {
        if (bgmSlider != null)
            bgmSlider.value = PlayerPrefs.GetFloat("musicVolume");

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            if (sfxAudioSource != null)
                sfxAudioSource.volume = sfxSlider.value;
        }
    }

    private void Save()
    {
        if (bgmSlider != null)
            PlayerPrefs.SetFloat("musicVolume", bgmSlider.value);

        if (sfxSlider != null)
            PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }


}
