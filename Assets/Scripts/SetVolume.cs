using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    // make this a singleton class
    public static SetVolume instance;

    public AudioMixer songMixer;

    public bool master = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetSongVolume(float sliderValue)
    {
        float volume = sliderValue != 0 ? Mathf.Log10(sliderValue) * 20 : -80;

        songMixer.SetFloat("Volume", volume);

        if (master) {
            PlayerPrefs.SetFloat("Volume", sliderValue);
        }
    }

    
}
