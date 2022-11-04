using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    TextMeshProUGUI scoreboardDisplay;
    int score;
    
    void Start()
    {
        score = 0;
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
