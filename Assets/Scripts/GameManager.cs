using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text comboText;

    public AudioClip audioClip;

    public GameObject[] spawners;

    private int combo;

    private AudioSource audioSource;

    private float songLength;

    private bool started = false;

    

    private void Start() {
        if (spawners.Length == 0) {
            Debug.LogError("Spawners are not set in the GameManager");
        }

        if (comboText == null) {
            Debug.LogError("Combo Text is not set in the GameManager");
        }
        else if (audioClip == null) {
            Debug.LogError("Audio Clip is not set in the GameManager");
        }

        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null) {
            Debug.LogError("Audio Source is not found in the GameManager");
        }
        else {
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            songLength = audioClip.length;
        }

        if (comboText != null && audioClip != null && audioSource != null && spawners.Length > 0) {
            NewGame();
        }
    }

    private void NewGame() {
        Time.timeScale = 1;
        combo = 0;
        comboText.text = "Combo\n" + combo.ToString();
        audioSource.Play();
        // wait 3 seconds and then enable the spawners
        // Invoke("EnableSpawners", 3f);
        started = true;
    }

    // private void EnableSpawners() {
    //     foreach (GameObject spawner in spawners) {
    //         spawner.SetActive(true);
    //     }
    // }

    // private void DisableSpawners() {
    //     foreach (GameObject spawner in spawners) {
    //         spawner.SetActive(false);
    //     }
    // }

    private void Update() {
        if (audioSource.time >= songLength && started) {
            endGame();
        }
    }

    public void pauseGame() {
        Time.timeScale = 0;
        audioSource.Pause();
    }

    public void resumeGame() {
        Time.timeScale = 1;
        audioSource.Play();
    }

    public void endGame() {
        audioSource.Stop();
        // DisableSpawners();
        started = false;
        SceneManager.LoadScene("Main Menu"); // TODO: Change to Game Over Scene
    }

    public void IncreaseCombo(int combo) {
        this.combo += combo;
        comboText.text = "Combo\n" + this.combo.ToString();
    }

    public void ResetCombo() {
        combo = 0;
        comboText.text = "Combo\n" + combo.ToString();
    }
}
