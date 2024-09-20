using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance { get; private set; }

    [Header("References")]
    public AudioMixer mixer;

    [Header("UI")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider SFXVolSlider;
    public Toggle skipTutToggle;
    public TMP_Dropdown fsDropdown;
    public TMP_Dropdown resDropdown;
    public ScrollRect dropdownScrollRect;

    [Header("Settings")]
    public bool skipTutorials = false;
    Resolution[] resolutions;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height/* + " @ " + resolutions[i].refreshRateRatio + "hz"*/;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height/* && resolutions[i].refreshRateRatio == Screen.currentResolution.refreshRateRatio*/)
            {
                currentResolutionIndex = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();

        #region LoadPlayerPrefs
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            SetMasterVolume(PlayerPrefs.GetFloat("MasterVol"));
            masterVolSlider.value = PlayerPrefs.GetFloat("MasterVol");
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVol"));
            musicVolSlider.value = PlayerPrefs.GetFloat("MusicVol");
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVol"));
            SFXVolSlider.value = PlayerPrefs.GetFloat("SFXVol");
        }

        if (PlayerPrefs.HasKey("SkipTut"))
        {
            if (PlayerPrefs.GetInt("SkipTut") == 1)
            {
                SkipTutorials(true);
                skipTutToggle.isOn = true;
            }
            else if (PlayerPrefs.GetInt("SkipTut") == 0)
            {
                SkipTutorials(false);
                skipTutToggle.isOn = false;
            }
        }

        if (PlayerPrefs.HasKey("FullScreenMode"))
        {
            SetFullscreen(PlayerPrefs.GetInt("FullScreenMode"));
            fsDropdown.RefreshShownValue();
        }

        if (PlayerPrefs.HasKey("ResIndex"))
        {
            SetResolution(PlayerPrefs.GetInt("ResIndex"));
            resDropdown.RefreshShownValue();
        }

        #endregion
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
        AkSoundEngine.SetRTPCValue("Master Audio Bus", volume);
        PlayerPrefs.SetFloat("MasterVol", volume);
        Debug.Log(PlayerPrefs.GetFloat("MasterVol"));
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", volume);
        AkSoundEngine.SetRTPCValue("Master Music Bus", volume);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", volume);
        AkSoundEngine.SetRTPCValue("SFX Bus", volume);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void SkipTutorials(bool value)
    {
        skipTutorials = value;

        if (value)
        {
            PlayerPrefs.SetInt("SkipTut", 1);
            Debug.Log(PlayerPrefs.GetInt("SkipTut"));
        }
        else if (!value)
        {
            PlayerPrefs.SetInt("SkipTut", 0);
        }
    }

    public void SetFullscreen(int fullScreenMode)
    {
        //Screen.fullScreenMode = (FullScreenMode)fullScreenMode;

        if (fullScreenMode == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            PlayerPrefs.SetInt("FullScreenMode", 0);
            Debug.Log(PlayerPrefs.GetInt("FullScreenMode"));
        }
        else if (fullScreenMode == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed; 
            PlayerPrefs.SetInt("FullScreenMode", 1);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("ResIndex", resolutionIndex);
    }
}
