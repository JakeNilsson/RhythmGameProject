using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject volumeSlider;

    public void Awake()
    {
        // set volume slider value to the value stored in PlayerPrefs
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.GetComponent<UnityEngine.UI.Slider>().value = volume;
        //SetVolume.instance.SetSongVolume(volume);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
