using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text comboText;

    private int combo;

    private void Start() {
        NewGame();
    }

    private void NewGame() {
        combo = 0;
        comboText.text = "Combo\n" + combo.ToString();
    }

    public void pauseGame() {
        Time.timeScale = 0;
    }

    public void resumeGame() {
        Time.timeScale = 1;
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
