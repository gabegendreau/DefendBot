using UnityEngine;

public class CrystalBehavior : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SoundManager soundManager;
    ManageGame gameManager;
    ManageColors colorManager;
    BotBehavior player;
    CircleCollider2D crystalCollider;
    Rigidbody2D crystalBody;
    bool spriteChange;
    public Sprite[] crystalPulseSprites;
    public int pulseFramesPerSecond;
    public int collectedFramesPerSecond;
    public float rotateSpeed;
    public float collectedRotateSpeed;
    public float selfDestructDelay;
    public float fadeOutAmount;
    int pulseSpriteIndex;
    bool hasSpawned;
    public Sprite[] spawnSprites;
    int spawnSpriteIndex;
    public int spawnFramesPerSecond;
    bool isCollected;
    Vector3 moveDirection;
    Vector3 movementVector;
    public float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.color = colorManager.GetLevelColor();
        isCollected = false;
        spriteChange = true;
        hasSpawned = false;
        pulseSpriteIndex = 0;
        spawnSpriteIndex = 0;
        soundManager.playCrystalSpawn();
        SpawnAnim();
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<ManageGame>();
        crystalCollider = GetComponent<CircleCollider2D>();
        player = FindObjectOfType<BotBehavior>();
        crystalBody = GetComponent<Rigidbody2D>();
        colorManager = FindObjectOfType<ManageColors>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasSpawned)
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
            Vector3 rotVector = new Vector3(0.0f, 0.0f, rotateSpeed);
            gameObject.transform.Rotate(rotVector, Space.Self);
        }

        if (isCollected)
        {
                moveDirection = player.transform.position - gameObject.transform.position;
                moveDirection = moveDirection.normalized;
                movementVector = moveDirection * moveSpeed;
                crystalBody.transform.position += movementVector * Time.deltaTime;
        }
    }

    void ResetAnimChange()
    {
        spriteChange = true;
    }

    void SpawnAnim()
    {
        if (!hasSpawned)
        {
            spriteRenderer.sprite = spawnSprites[spawnSpriteIndex];
            spawnSpriteIndex++;
            if (spawnSpriteIndex == spawnSprites.Length)
            {
                hasSpawned = true;
                crystalCollider.enabled = true;
                Invoke("SelfDestruct", selfDestructDelay);
            } else {
                Invoke("SpawnAnim", 1.0f/spawnFramesPerSecond);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            isCollected = true;
            gameManager.IncTotalKilled();
            rotateSpeed = collectedRotateSpeed;
            pulseFramesPerSecond = collectedFramesPerSecond;
        }
        if (other.tag == "Player")
        {
            crystalCollider.enabled = false;
            moveSpeed *= 0.5f;
            shrinkAndDestroy();
            soundManager.playCrystalConsume();
        }
    }

    void shrinkAndDestroy()
    {
        Vector3 tempScale = gameObject.transform.localScale;
        tempScale *= 0.75f;
        gameObject.transform.localScale = tempScale;
        if (tempScale.x >= 0.1f)
        {
            Invoke("shrinkAndDestroy", 0.125f);
        } else {
            gameManager.CollectCrystal();
            Destroy(gameObject);
        }

    }
    public bool GetCollected()
    {
        return isCollected;
    }

    void SelfDestruct()
    {
        if (crystalCollider.enabled)
        {
            crystalCollider.enabled = false;
        }
        Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - fadeOutAmount);
        spriteRenderer.color = newColor;
        if (newColor.a <= 0.05f)
        {
            Destroy(gameObject);
        } else {
            Invoke("SelfDestruct", 0.125f);
        }
    }
}
