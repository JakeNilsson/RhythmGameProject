using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{

    public Text scoreText;
    public Text notesHitText;
    public Text longestStreakText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(int notesHit, int longestStreak, int score)
    {
        notesHitText.text = "Notes Hit: " + notesHit;
        longestStreakText.text = "Final Combo: " + longestStreak;
        scoreText.text = "Score: " + score;
        gameObject.SetActive(true);
    }
}