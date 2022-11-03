using UnityEngine;

public class SpectreBehavior : MonoBehaviour
{
    BotBehavior player;
    StationBehavior station;
    SoundManager soundManager;
    ManageGame gameManager;
    Rigidbody2D spectreBody;
    SpriteRenderer spriteRenderer;
    BoxCollider2D spectreCollider;
    Vector3 moveDirection;
    Vector3 movementVector;
    public float speed; // 4.2
    public float spectreDamage;
    bool hasSpawned;
    bool isBeingDestroyed;
    bool isDead;
    bool isExploding;
    bool spriteChange;
    public Sprite[] spectreSpawnSprites;
    int spectreSpawnIndex;
    public int spawnFramesPerSecond;
    public Sprite[] spectreFlySprites;
    int spectreFlyIndex;
    public int flyFramesPerSecond;
    public float zigZagAngle; // 16
    public float zigZagRotSpeed; // 1.53
    float zigZagT;
    bool zigZagPaused;
    public float zigZagPauseTime;
    public Sprite[] spectreExplodeSprites;
    int spectreExplodeIndex;
    public int explodeFramesPerSecond;
    public Sprite[] spectrePhaseOutSprites;
    public int phaseOutFramesPerSecond;
    int spectrePhaseOutIndex;

    // Start is called before the first frame update
    void Start()
    {
        zigZagPaused = false;
        zigZagT = 0.0f;
        hasSpawned = false;
        isBeingDestroyed = false;
        spriteChange = true;
        isExploding = false;
        isDead = false;
        spectreSpawnIndex = 0;
        spectreFlyIndex = 0;
        spectreExplodeIndex = 0;
        spectrePhaseOutIndex = 0;
        soundManager.playSpectreSpawn();
        SpawnAnim();
    }

    void Awake()
    {
        player = FindObjectOfType<BotBehavior>();
        station = FindObjectOfType<StationBehavior>();
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<ManageGame>();
        spectreBody = GetComponent<Rigidbody2D>();
        spectreCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        moveDirection = player.transform.position - gameObject.transform.position;
        moveDirection = moveDirection.normalized;
        movementVector = moveDirection * speed;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 180.0f));

    }

    // Update is called once per frame
    void Update()
    {
        if (hasSpawned)
        {
            if (!isDead && !isExploding)
            {
                if (spriteChange)
                {
                    spriteChange = false;
                    spriteRenderer.sprite = spectreFlySprites[spectreFlyIndex];
                    spectreFlyIndex++;
                    if (spectreFlyIndex == spectreFlySprites.Length)
                    {
                        spectreFlyIndex = 0;
                    }
                    Invoke("ResetAnimChange", 1.0f/flyFramesPerSecond);
                }  
                if (!zigZagPaused)
                { 
                    moveDirection = player.transform.position - gameObject.transform.position;
                    moveDirection = moveDirection.normalized;
                    movementVector = moveDirection * speed;
                    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                    float angleToAdd = Mathf.Lerp(-zigZagAngle, zigZagAngle, zigZagT);
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + angleToAdd));
                    spectreBody.transform.position += movementVector * Time.deltaTime;     
                    zigZagT += zigZagRotSpeed * Time.deltaTime;
                    if (zigZagT > 1.0f)
                    {
                        zigZagAngle *= -1.0f;
                        zigZagT = 0.0f;
                        zigZagPaused = true;
                        Invoke("UnpauseZigZag", zigZagPauseTime);
                    }
                }    
            }
        }
    }

    void SpawnAnim()
    {
        if (!hasSpawned)
        {
            spriteRenderer.sprite = spectreSpawnSprites[spectreSpawnIndex];
            spectreSpawnIndex++;
            if (spectreSpawnIndex == spectreSpawnSprites.Length)
            {
                hasSpawned = true;
                spectreCollider.enabled = true;
                soundManager.playSpectreMove();
            } else {
                Invoke("SpawnAnim", 1.0f/spawnFramesPerSecond);
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
        gameObject.tag = "Unshootable";
        gameManager.IncTotalKilled();
        spectreCollider.enabled = false;
        soundManager.playSpectreSpawn();
        PhaseOut();
    }

    void PhaseOut()
    {
        if (!isBeingDestroyed)
        {
            spriteRenderer.sprite = spectrePhaseOutSprites[spectrePhaseOutIndex];
            spectrePhaseOutIndex++;
            if (spectrePhaseOutIndex == spectrePhaseOutSprites.Length)
            {
                isBeingDestroyed = true;
                Destroy(gameObject);
            } else {
                Invoke("PhaseOut", 1.0f/phaseOutFramesPerSecond);
            }
        }
    }

    public void Explode()
    {
        if (!isBeingDestroyed)
        {
            isDead = true;
            spriteRenderer.sprite = spectreExplodeSprites[spectreExplodeIndex];
            spectreExplodeIndex++;
            if (spectreExplodeIndex == spectreExplodeSprites.Length)
            {
                isBeingDestroyed = true;
                Destroy(gameObject);
            } else {
                Invoke("Explode", 1.0f/explodeFramesPerSecond);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameObject.tag = "Unshootable";
            soundManager.playStationDamaged();
            station.TakeDamage(spectreDamage);
            isExploding = true;
            Explode();
        }
    }

    void UnpauseZigZag()
    {
        soundManager.playSpectreMove();
        zigZagPaused = false;
    }
}
