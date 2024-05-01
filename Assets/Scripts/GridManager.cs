using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static int currentLevel = 1;

    [NonSerialized] public int num_levels = 1;

    public Button buttonPrefab;

    private void Start()
    {
        // check how many folders are in the persistent data path/levels folder
        string path = Path.Combine(Application.persistentDataPath, "Levels");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //string[] directories = Directory.GetDirectories(path);

        string[] directories = Directory.GetDirectories(path).Select(Path.GetFileName).ToArray();
        

        Debug.Log("Directories: " + string.Join(", ", directories));

        num_levels = directories.Length;

        for (int i = 0; i < num_levels; i++)
        {
            int level = i + 1;
            Button button = Instantiate(buttonPrefab, transform);
            string truncatedName = directories[i].Length > 8 ? directories[i].Substring(0, 8) : directories[i];
            button.name = truncatedName;
            button.GetComponentInChildren<Text>().text = truncatedName;
            button.onClick.AddListener(() => LoadLevel(level));
        }
    }

    private void LoadLevel(int level)
    {
        currentLevel = level;
        // log current level
        Debug.Log($"Current Level: {currentLevel}");
        SceneManager.LoadScene("Game");
    }
}
