using UnityEngine;

public class Recharge : MonoBehaviour
{
    StationBehavior station;
    SpriteRenderer spriteRenderer;
    bool isBeingDestroyed;
    ManageColors colorManager;

    // Start is called before the first frame update
    void Start()
    {
        isBeingDestroyed = false;
        spriteRenderer.color = colorManager.GetLevelColor();
    }

    void Awake()
    {
        station = FindObjectOfType<StationBehavior>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorManager = FindObjectOfType<ManageColors>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!isBeingDestroyed)
                {
                    isBeingDestroyed = true;  // CHECK THIS WHEN YOU'RE NOT FATIGUED
                    station.RechargePower();
                    Destroy(gameObject);
                }
        }
    }
}
