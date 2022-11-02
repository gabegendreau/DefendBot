using UnityEngine;

public class ManageGame : MonoBehaviour
{
    SoundManager soundManager;
    BotBehavior player;
    public GameObject[] spawnLocations;
    int previousSpawnLocation;
    public GameObject enemyPrefab;
    public GameObject ghoulPrefab;
    public int numEnemiesKilledToGhoul;
    int numEnemiesKilled;
    public float minSpawnInterval;
    public float maxSpawnInterval;
    float spawnInterval;
    TransferBot[] platforms;
    public GameObject powerBarScaler;
    public GameObject gameOverText;
    bool gameOver;
    public GameObject playButtonLocation;
    public GameObject quitButtonLocation;
    public GameObject playButtonPrefab;
    public GameObject quitButtonPrefab;
    int totalBaddiesSpawned;
    float totalKilled;
    
    void Start()
    {
        previousSpawnLocation = 99;
        totalBaddiesSpawned = 0;
        totalKilled = 0.0f;
        gameOver = false;
        SpawnEnemy();
    }

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        player = FindObjectOfType<BotBehavior>();
        platforms = FindObjectsOfType<TransferBot>();
    }

    void SpawnEnemy()
    {
        if (!gameOver)
        {
            int randomIndex = Random.Range(0, spawnLocations.Length - 1);
            if (randomIndex != previousSpawnLocation)
            {
                previousSpawnLocation = randomIndex;
                Instantiate(enemyPrefab, spawnLocations[randomIndex].transform.position, Quaternion.identity);
                spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
                totalBaddiesSpawned++;
                Debug.Log("Baddies spawned: " + totalBaddiesSpawned);
                Invoke("SpawnEnemy", spawnInterval);
            } else {
                SpawnEnemy();
            }

        }
    }

    public void UpdatePowerbar(float power)
    {
        Vector3 newPower = new Vector3(power/100.0f, 1.0f, 1.0f);
        powerBarScaler.transform.localScale = newPower;
    }

    public void EndGame()
    {
        gameOver = true;
        gameOverText.SetActive(true);
        soundManager.playGameOver();
        GameObject[] victoriousEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in victoriousEnemies)
        {
            Destroy(enemy.gameObject);
        }
        Invoke("SpawnButtons", 1.0f);
    }

    void SpawnButtons()
    {
        Instantiate(playButtonPrefab, playButtonLocation.transform.position, Quaternion.identity);
        Instantiate(quitButtonPrefab, quitButtonLocation.transform.position, Quaternion.identity);
    }

    public void IncEnemiesKilled()
    {
        numEnemiesKilled++;
        IncTotalKilled();
        if (numEnemiesKilled == numEnemiesKilledToGhoul)
        {
            numEnemiesKilled = 0;
            SpawnGhoul();
        }
    }

    public void SpawnGhoul()
    {
        Vector3 ghoulLocation = player.transform.position;
        foreach(TransferBot platform in platforms)
        {
            if (platform.GetPlatformNumber() == player.GetPlatformMode())
            {
                ghoulLocation = platform.GetFartherGhoulSpawnLocation();
            }
        }
        Quaternion ghoulRotation = player.transform.rotation;
        Instantiate(ghoulPrefab, ghoulLocation, ghoulRotation);
        totalBaddiesSpawned++;
        Debug.Log(totalBaddiesSpawned);
    }

    public void IncTotalKilled()
    {
        totalKilled += 1.0f;;
    }

    public float GetTotalKilled()
    {
        return totalKilled;
    }
}
