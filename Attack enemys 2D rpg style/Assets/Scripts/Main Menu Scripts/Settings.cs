using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    public CreateSettings createSettings;
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public TMP_Dropdown resolutionDropdown;
   // public Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
  

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, createSettings.fullscreen);
        audioMixer.SetFloat("Volume", createSettings.volume);
        audioSource.volume = createSettings.volume;
        volumeSlider.value = createSettings.volume;
        Screen.fullScreen = createSettings.fullscreen;
        fullscreenToggle.enabled = createSettings.fullscreen;
        QualitySettings.SetQualityLevel(createSettings.qualityIndex);
    }
    private void Start()
    {
       // Screen.SetResolution(1920, 1080, false);
        // resolutions = Screen.resolutions;
        /*
         resolutionDropdown.ClearOptions();

         List<string> options = new List<string>();
         int currentResolutionIndex = 0;

         for (int i = 0; i < resolutions.Length; i++)
         {
             string option = resolutions[i].width + " x " + resolutions[i].height;
             options.Add(option);

             if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
             {
                 currentResolutionIndex = i;
             }
         }
         */
        //resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        //  resolutionDropdown.RefreshShownValue();

    }
    public void SetResolution (int resolutionIndex)
    {
       // Resolution resolution = resolutions[resolutionIndex];
     //   Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        audioSource.volume = volume;
        createSettings.volume = volume;
    }


     public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        createSettings.qualityIndex = qualityIndex;
    }
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        createSettings.fullscreen = isFullscreen;

    }
}
