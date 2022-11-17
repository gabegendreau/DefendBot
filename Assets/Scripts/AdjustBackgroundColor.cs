using UnityEngine;

public class AdjustBackgroundColor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    ManageColors colorManager;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.color = colorManager.GetLevelColor();
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorManager = FindObjectOfType<ManageColors>();
    }
}
