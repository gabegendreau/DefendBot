using UnityEngine;

public class CrystalBehavior : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool spriteChange;
    public Sprite[] crystalPulseSprites;
    public int pulseFramesPerSecond;
    int pulseSpriteIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteChange = true;
        pulseSpriteIndex = 0;
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteChange)
                {
                    spriteChange = false;
                    spriteRenderer.sprite = crystalPulseSprites[pulseSpriteIndex];
                    pulseSpriteIndex++;
                    if (pulseSpriteIndex == crystalPulseSprites.Length)
                    {
                        pulseSpriteIndex = 0;
                    }
                    Invoke("ResetAnimChange", 1.0f/pulseFramesPerSecond);
                }        
    }

    void ResetAnimChange()
    {
        spriteChange = true;
    }
}
