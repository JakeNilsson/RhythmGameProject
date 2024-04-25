using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;

    void Start()
    {
        //SetVolume.instance.songMixer = audioMixer;
        //SetVolume.instance.SetSongVolume(PlayerPrefs.GetFloat("Volume", 1.0f));
    }

    public void OpenLevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }

    public void OpenMapping()
    {
        SceneManager.LoadScene("Mapping");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Successfully.");
        Application.Quit();
    }
}
