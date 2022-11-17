using UnityEngine;
using TMPro;

public class DisplayFinalScore : MonoBehaviour
{
    TextMeshProUGUI finalScore;
    StoreScore storedScore;

    // Start is called before the first frame update
    void Start()
    {
        finalScore.text = storedScore.GetScore().ToString();
    }

    void Awake()
    {
        finalScore = gameObject.GetComponent<TextMeshProUGUI>();
        storedScore = FindObjectOfType<StoreScore>();
    }
}
