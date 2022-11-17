using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    TextMeshProUGUI scoreboardDisplay;
    int score;
    StoreScore storedScore;
    
    void Start()
    {
        score = storedScore.GetScore();
        scoreboardDisplay.text = score.ToString();
    }

    void Awake()
    {
        scoreboardDisplay = GetComponent<TextMeshProUGUI>();
        storedScore = FindObjectOfType<StoreScore>();
    }

    public void UpdateScore(int newValue)
    {
        score += newValue;
        storedScore.AddToScore(newValue);
        scoreboardDisplay.text = score.ToString();
    }
}
