using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    // References to UI sliders and input fields
    public Slider musicVolumeSlider;
    public InputField musicVolumeInputField;
    public Slider sfxVolumeSlider;
    public InputField sfxVolumeInputField;
    public Slider mouseSensitivitySlider;
    public InputField mouseSensitivityInputField;

    void Start()
    {
        // Set initial values from GameSettings
        musicVolumeSlider.value = GameSettings.instance.musicVolume;
        musicVolumeInputField.text = GameSettings.instance.musicVolume.ToString();
        sfxVolumeSlider.value = GameSettings.instance.sfxVolume;
        sfxVolumeInputField.text = GameSettings.instance.sfxVolume.ToString();
        mouseSensitivitySlider.value = GameSettings.instance.mouseSensitivity;
        mouseSensitivityInputField.text = GameSettings.instance.mouseSensitivity.ToString();
    }

    // Function to set music volume from UI
    public void SetMusicVolume(float value)
    {
        GameSettings.instance.SetMusicVolume(value); // Call GameSettings function
        musicVolumeInputField.text = value.ToString(); // Update input field text
    }

    // Function to set SFX volume from UI
    public void SetSfxVolume(float value)
    {
        GameSettings.instance.SetSfxVolume(value);
        sfxVolumeInputField.text = value.ToString();
    }

    // Function to set mouse sensitivity from UI
    public void SetMouseSensitivity(float value)
    {
        GameSettings.instance.SetMouseSensitivity(value);
        mouseSensitivityInputField.text = value.ToString();
    }
}