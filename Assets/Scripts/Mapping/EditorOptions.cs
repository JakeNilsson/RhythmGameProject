using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class EditorOptions : MonoBehaviour
{
    public GameObject volumeSlider;

    public GameObject optionsCanvas;

    public GameObject editorCanvas;

    public AudioSource audioSource;

    // private bool isPlaying = false;

    public void Awake()
    {
        // set volume slider value to the value stored in PlayerPrefs
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.GetComponent<UnityEngine.UI.Slider>().value = volume;
        SetVolume.instance.SetSongVolume(volume);
    }

    public void BackToEditor()
    {
        optionsCanvas.SetActive(false);
        //editorCanvas.SetActive(true);
    }

    public void OpenOptions()
    {
        // isPlaying = audioSource.isPlaying;

        // if (isPlaying)
        // {
        //     audioSource.Pause();
        // }
        
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        optionsCanvas.SetActive(true);
        //editorCanvas.SetActive(false);
    }

}
