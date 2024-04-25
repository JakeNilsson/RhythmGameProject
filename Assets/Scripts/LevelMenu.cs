using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    void Start()
    {

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
