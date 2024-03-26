using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetLevel(float sliderValue)
    {
        if (sliderValue == 0)
        {
            audioMixer.SetFloat("Volume", -80);
        }
        else
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        }
    }
}
