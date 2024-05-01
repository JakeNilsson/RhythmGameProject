using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text comboText;
    public Text scoreText;

    public EndScreen endScreen;

    //public AudioClip[] audioClips;

    public GameObject[] spawners;

    private int combo;
    private int score;

    private int totalNotesHit;

    private AudioSource audioSource;

    private float songLength;

    private bool started = false;
    
    private UnityWebRequest GetAudioFromFile(string path)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG);
        request.SendWebRequest();

        // wait for the request to finish
        while (!request.isDone) { }

        return request;
    }

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
        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null) {
            Debug.LogError("Audio Source is not found in the GameManager");
        }

        String path = Path.Combine(Application.persistentDataPath, "Levels");

        String[] directories = Directory.GetDirectories(path);

        if (directories.Length < GridManager.currentLevel) {
            Debug.LogError("No levels found in the Levels folder");
        }
        
        String levelPath = directories[GridManager.currentLevel - 1];

        String[] files = Directory.GetFiles(levelPath);

        if (files.Length == 0) {
            Debug.LogError("No files found in the level folder");
        }

        // get the mp3 file
        String mp3File = Array.Find(files, file => file.EndsWith(".mp3"));

        if (mp3File == null) {
            Debug.LogError("No mp3 file found in the level folder");
        }


        UnityWebRequest request = GetAudioFromFile(mp3File);

        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);

        audioClip.name = Path.GetFileNameWithoutExtension(mp3File);

        audioSource.clip = audioClip;
        
        // AudioClip audioClip = audioClips[GridManager.currentLevel - 1];
        // audioSource.clip = audioClips[GridManager.currentLevel - 1];
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        songLength = audioClip.length;

        if (comboText != null && audioSource != null && spawners.Length > 0) {
            NewGame();
        }
    }

    private void NewGame() {
        Time.timeScale = 1;
        combo = 0;
        score = 0;
        totalNotesHit = 0;
        comboText.text = "Combo\n" + combo.ToString();
        scoreText.text = "Score\n" + score.ToString();
        audioSource.Play();
        // wait 3 seconds and then enable the spawners
        // Invoke("EnableSpawners", 3f);
        started = true;

        // set the audio source for the spawner
        Spawner.audioSource = audioSource;
        // activate the first spawner
        spawners[0].GetComponent<Spawner>().enabled = true;
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
        //spawners[0].GetComponent<Spawner>().enabled = true;
    }

    public void endGame() {
        audioSource.Stop();
        // DisableSpawners();
        started = false;

        if (endScreen != null)
        {
            endScreen.Show(totalNotesHit, combo, score);
        }

        StartCoroutine(LoadMainMenuAfterDelay(5)); // wait for 5 seconds
    }

    private IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Main Menu");
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

    public void NoteHit()
    {
        totalNotesHit++;
    }

    public int GetTotalNotesHit()
    {
        return totalNotesHit;
    }
}
