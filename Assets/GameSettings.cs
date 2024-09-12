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
        resDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

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
        UpdateScrollPosition(currentResolutionIndex);
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }

    public void SkipTutorials(bool value)
    {
        skipTutorials = value;
    }

    public void SetFullscreen (int fullScreenMode)
    {
        //Screen.fullScreenMode = (FullScreenMode)fullScreenMode;

        if (fullScreenMode == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (fullScreenMode == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    public void UpdateScrollPosition(int index)
    {
        float normalizedPosition = (float)index / resDropdown.options.Count; //Calculate the normalized position based on the index
        dropdownScrollRect.verticalNormalizedPosition = 1 - normalizedPosition;
    }

    private void OnDropdownValueChanged(int index)
    {
        UpdateScrollPosition(index); //Call UpdateScrollPosition when the dropdown value changes
    }

    private void OnDestroy()
    {
        resDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}
