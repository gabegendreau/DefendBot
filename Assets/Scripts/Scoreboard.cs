using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    TextMeshProUGUI scoreboardDisplay;
    int score;
    
    void Start()
    {
        scoreboardDisplay.text = score.ToString();
    }

    void Awake()
    {
        scoreboardDisplay = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int newValue)
    {
        score += newValue;
        scoreboardDisplay.text = score.ToString();
    }
}
