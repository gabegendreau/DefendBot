using UnityEngine;

public class StationBehavior : MonoBehaviour
{
    SoundManager soundManager;
    public Sprite[] stationSprites;
    public int framesPerSecond;
    SpriteRenderer spriteRenderer;
    BotBehavior player;
    bool spriteChange;
    int spriteIndex;
    public float health;
    float defaultHealth;
    public float damageFromEnemy;
    ManageGame gameManager;
    bool gameOver;
    public GameObject batteryPrefab;
    public GameObject[] batterySpawnPoints;
    bool batteryCooldown;
    public float batteryCooldownTime;

    // Start is called before the first frame update
    void Start()
    {
        spriteIndex = 0;
        spriteChange = true;
        batteryCooldown = false;
        defaultHealth = health;
    }

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<ManageGame>();
        player = FindObjectOfType<BotBehavior>();
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (spriteChange)
            {
                spriteChange = false;
                spriteRenderer.sprite = stationSprites[spriteIndex];
                spriteIndex++;
                if (spriteIndex == stationSprites.Length)
                {
                    spriteIndex = 0;
                }
                Invoke("ResetAnimChange", 1.0f/framesPerSecond);
            }
            if (health <= 35.0f && !batteryCooldown)
            {
                batteryCooldown = true;
                SpawnBattery();
                Invoke("ResetBatteryCooldown", batteryCooldownTime);
            }
        }          
    }

    void ResetAnimChange()
    {
        spriteChange = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health > 0.0f)
        {
            gameManager.UpdatePowerbar(health);
        } else {
            gameManager.UpdatePowerbar(0.0f);
            gameOver = true;
            gameManager.EndGame();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Enemy"))
        {
            soundManager.playStationDamaged();
            other.gameObject.GetComponent<EnemyBehavior>().Explode();
            TakeDamage(damageFromEnemy);
        }
    }

    void SpawnBattery()
    {
        int platNum = player.GetPlatformMode();
        Vector3 rotation = Vector3.zero;
        switch (platNum)
        {
            case 0:
                platNum = 2;
                rotation = new Vector3(0.0f, 0.0f, -180.0f);
                break;
            case 1:
                platNum = 3;
                rotation = new Vector3(0.0f, 0.0f, 90.0f);
                break;
            case 2:
                platNum = 0;
                rotation = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case 3:
                platNum = 1;
                rotation = new Vector3(0.0f, 0.0f, -90.0f);
                break;
            default:
                platNum = 2;
                rotation = new Vector3(0.0f, 0.0f, -180.0f);
                Debug.Log("SpawnBattery did not get a valid platform number");
                break;
        }
        Vector3 batterySpawnLocation = batterySpawnPoints[platNum].transform.position;
        Instantiate(batteryPrefab, batterySpawnLocation, Quaternion.Euler(rotation));
    }

    void ResetBatteryCooldown()
    {
        batteryCooldown = false;
    }

    public void RechargePower()
    {
        health = defaultHealth;
        gameManager.UpdatePowerbar(health);
    }

    public float GetEnemyDamage()
    {
        return damageFromEnemy;
    }
}
