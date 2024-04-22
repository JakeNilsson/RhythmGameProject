using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text comboText;
    public Text scoreText;

    public AudioClip[] audioClips;

    public GameObject[] spawners;

    private int combo;
    private int score;

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

        if (scoreText == null) {
            Debug.LogError("Score Text is not set in the GameManager");
        }
        else if (audioClips.Length == 0) {
            Debug.LogError("Audio Clips are not set in the GameManager");
        }

        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null) {
            Debug.LogError("Audio Source is not found in the GameManager");
        }
        else {
            AudioClip audioClip = audioClips[GridManager.currentLevel - 1];
            audioSource.clip = audioClips[GridManager.currentLevel - 1];
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            songLength = audioClip.length;
        }

        if (comboText != null && audioClips.Length > 0 && audioSource != null && spawners.Length > 0) {
            NewGame();
        }
    }

    private void NewGame() {
        Time.timeScale = 1;
        combo = 0;
        score = 0;
        comboText.text = "Combo\n" + combo.ToString();
        scoreText.text = "Score\n" + score.ToString();
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

    public void IncreaseScore(int score) {
        this.score += score;
        scoreText.text = "Score\n" + this.score.ToString();
    }
}
