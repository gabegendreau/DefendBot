using UnityEngine;
using TMPro;

public class AdjustBigTextColor : MonoBehaviour
{
    TextMeshProUGUI textObject;
    ManageColors colorManager;

    // Start is called before the first frame update
    void Start()
    {
        textObject.faceColor = colorManager.GetLevelColor();
    }

    void Awake()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        colorManager = FindObjectOfType<ManageColors>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
