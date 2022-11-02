using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    ManageGame gameManager;
    SoundManager soundManager;
    StationBehavior station;
    Vector3 stationLocation;
    Vector3 moveDirection;
    public float speed;
    Vector3 movementVector;
    Rigidbody2D enemyBody;
    BoxCollider2D enemyCollider;
    public Sprite[] enemySprites;
    int spriteIndex;
    bool spriteChange;
    SpriteRenderer spriteRenderer;
    public int framesPerSecond;
    bool isDead;
    public Sprite deadSprite;
    public Sprite[] rotSprites;
    public int rotFramesPerSecond;
    int rotSpriteIndex;
    public float rotDelay;
    public Sprite[] explodeSprites;
    public int explodeFramesPerSecond;
    int explodeSpriteIndex;
    public Sprite[] spawnSprites;
    public int spawnFramesPerSecond;
    int spawnSpriteIndex;
    bool hasSpawned;
    bool isBeingDestroyed;

    // Start is called before the first frame update
    void Start()
    { 
        isDead = false;
        spriteIndex = 0;
        spriteChange = true;
        rotSpriteIndex = 0;
        explodeSpriteIndex = 0;
        spawnSpriteIndex = 0;
        isBeingDestroyed = false;
        hasSpawned = false;
        SpawnAnim();
    }

    void Awake()
    {
        gameManager = FindObjectOfType<ManageGame>();
        soundManager = FindObjectOfType<SoundManager>();
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        station = FindObjectOfType<StationBehavior>();
        stationLocation = station.transform.position;
        moveDirection = stationLocation - gameObject.transform.position;
        moveDirection = moveDirection.normalized;
        movementVector = moveDirection * speed;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Update is called once per frame
    void Update()
    {
        if (hasSpawned)
        {
            if (!isDead)
            {
                enemyBody.transform.position += movementVector * Time.deltaTime;
                if (spriteChange)
                {
                    spriteChange = false;
                    spriteRenderer.sprite = enemySprites[spriteIndex];
                    spriteIndex++;
                    if (spriteIndex == enemySprites.Length)
                    {
                        spriteIndex = 0;
                    }
                    Invoke("ResetAnimChange", 1.0f/framesPerSecond);
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
        isDead = true;
        enemyCollider.enabled = false;
        spriteRenderer.sprite = deadSprite;
        gameManager.IncEnemiesKilled();
        Invoke("RotAway", rotDelay);
    }

    void RotAway()
    {
        if (!isBeingDestroyed)
        {
            spriteRenderer.sprite = rotSprites[rotSpriteIndex];
            rotSpriteIndex++;
            if (rotSpriteIndex == rotSprites.Length)
            {
                isBeingDestroyed = true;
                Destroy(gameObject);
            } else {
                Invoke("RotAway", 1.0f/rotFramesPerSecond);
            }
        }
    }

    public void Explode()
    {
        if (!isBeingDestroyed)
        {
            isDead = true;
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
                enemyCollider.enabled = true;
            } else {
                Invoke("SpawnAnim", 1.0f/spawnFramesPerSecond);
            }
        }
    }
}
