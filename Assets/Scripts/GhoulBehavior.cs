using UnityEngine;

public class GhoulBehavior : MonoBehaviour
{
    SoundManager soundManager;
    ManageGame gameManager;
    StationBehavior station;
    Vector3 moveDirection;
    Vector3 platformGravity;
    BotBehavior player;
    public float speed;
    Vector3 movementVector;
    Rigidbody2D ghoulBody;
    BoxCollider2D[] ghoulColliders;
    SpriteRenderer spriteRenderer;
    bool hasSpawned;
    bool isBeingDestroyed;
    bool isShot;
    bool isExploding;
    public Sprite[] spawnSprites;
    public Sprite[] chaseSprites;
    public Sprite[] explodeSprites;
    public Sprite[] burrowSprites;
    int spawnSpriteIndex;
    int chaseSpriteIndex;
    int explodeSpriteIndex;
    int burrowSpriteIndex;
    public int spawnFramesPerSecond;
    public int chaseFramesPerSecond;
    public int explodeFramesPerSecond;
    public int burrowFramesPerSecond;
    bool spriteChange;
    int platformMode;
    public float gravityScale;
    public float ghoulDamage;

    // Start is called before the first frame update
    void Start()
    {
        hasSpawned = false;
        isBeingDestroyed = false;
        isShot = false;
        isExploding = false;
        spawnSpriteIndex = 0;
        chaseSpriteIndex = 0;
        explodeSpriteIndex = 0;
        burrowSpriteIndex = 0;
        spriteChange = true;
        soundManager.playGhoulSpawn();
        SpawnAnim();
        SetGravity(player.GetGravity());
    }

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<ManageGame>();
        station = FindObjectOfType<StationBehavior>();
        player = FindObjectOfType<BotBehavior>();
        ghoulBody = GetComponent<Rigidbody2D>();
        ghoulColliders = GetComponents<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformMode = player.GetPlatformMode();
    }
    
    void Update()
    {
        if (hasSpawned && !isExploding)
        {
            if (!isShot)
            {
                moveDirection = player.transform.position - gameObject.transform.position;
                moveDirection = moveDirection.normalized;
                movementVector = moveDirection * speed;
                movementVector += platformGravity;
                ghoulBody.transform.position += movementVector * Time.deltaTime;  // SOMEWHERE IN HERE USE PLATFORM MODE TO FLIP SPRITE *********
                if (spriteChange)
                {
                    spriteChange = false;
                    spriteRenderer.sprite = chaseSprites[chaseSpriteIndex];
                    chaseSpriteIndex++;
                    if (chaseSpriteIndex == chaseSprites.Length)
                    {
                        chaseSpriteIndex = 0;
                    }
                    Invoke("ResetAnimChange", 1.0f/chaseFramesPerSecond);
                }     
            }
        }
    }

    void ResetAnimChange()
    {
        spriteChange = true;
    }

    public void GetShot()
    {
        soundManager.playEnemyHit();
        isShot = true;
        gameObject.tag = "Unshootable";
        gameManager.IncTotalKilled();
        gameManager.CalcScoreInc(ghoulDamage);
        foreach (BoxCollider2D collider in ghoulColliders)
        {
            enabled = false;
        }
        soundManager.playGhoulSpawn();
        Burrow();
    }

    void Burrow()
    {
        if (!isBeingDestroyed)
        {
            spriteRenderer.sprite = burrowSprites[burrowSpriteIndex];
            burrowSpriteIndex++;
            if (burrowSpriteIndex == burrowSprites.Length)
            {
                isBeingDestroyed = true;
                Destroy(gameObject);
            } else {
                Invoke("Burrow", 1.0f/burrowFramesPerSecond);
            }
        }
    }

    public void Explode()
    {
        if (!isBeingDestroyed)
        {
            foreach (BoxCollider2D collider in ghoulColliders)
            {
                enabled = false;
            }
            spriteRenderer.sprite = explodeSprites[explodeSpriteIndex];
            explodeSpriteIndex++;
            if (explodeSpriteIndex == explodeSprites.Length)
            {
                isBeingDestroyed = true;
                Destroy(gameObject);
            } else {
                Invoke("Explode", 1.0f/explodeFramesPerSecond);
            }
        }
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
                foreach (BoxCollider2D collider in ghoulColliders)
                {
                    enabled = true;
                }
            } else {
                Invoke("SpawnAnim", 1.0f/spawnFramesPerSecond);
            }
        }
    }

    public void SetGravity(Vector3 newGravity)
    {
        platformGravity = newGravity * gravityScale;
    }

    public void SetPlaformMode(int mode)
    {
        platformMode = mode;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameObject.tag = "Unshootable";
            soundManager.playStationDamaged();
            station.TakeDamage(ghoulDamage);
            isExploding = true;
            Explode();
        }
    }
}
