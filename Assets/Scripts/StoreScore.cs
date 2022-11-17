
using UnityEngine;

public class StoreScore : MonoBehaviour
{
    int score;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        score = 0;
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int value)
    {
        score += value;
    }
}
