using UnityEngine;

public class PlayAnimations : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    ManageColors colorManager;
    public Sprite[] walkSprites;
    public Sprite[] idleSprites;
    public float framesPerSecond;
    bool isWalking;
    


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

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkSprites.Length;
            spriteRenderer.sprite = walkSprites[index];  
        } else if (!isWalking) {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % idleSprites.Length;
            spriteRenderer.sprite = idleSprites[index];
        }
    }

    public void SetIsWalking(bool walking)
    {
        isWalking = walking;
    }
}
