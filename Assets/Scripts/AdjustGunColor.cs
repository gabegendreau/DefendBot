using UnityEngine;

public class AdjustGunColor : MonoBehaviour
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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        colorManager = FindObjectOfType<ManageColors>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
