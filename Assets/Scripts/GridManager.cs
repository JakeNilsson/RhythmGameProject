using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static int currentLevel = 1;

    public int num_levels = 1;

    public Button buttonPrefab;

    private void Start()
    {
        int level;
        for (int i = 0; i < num_levels; i++)
        {
            level = i + 1;
            Button button = Instantiate(buttonPrefab, transform);
            button.name = $"Level {level}";
            button.GetComponentInChildren<Text>().text = $"Level {level}";
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
