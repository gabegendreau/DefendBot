using UnityEngine;

public class Recharge : MonoBehaviour
{
    StationBehavior station;
    SpriteRenderer spriteRenderer;
    bool isBeingDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        isBeingDestroyed = false;
    }

    void Awake()
    {
        station = FindObjectOfType<StationBehavior>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
