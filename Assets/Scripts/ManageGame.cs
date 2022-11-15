using UnityEngine;

public class ManageGame : MonoBehaviour
{
    SoundManager soundManager;
    BotBehavior player;
    Scoreboard scoreboard;
    CrystalScoreboard crystalScoreboard;
    public GameObject[] spawnLocations;
    int previousSpawnLocation;
    public GameObject enemyPrefab;
    public GameObject ghoulPrefab;
    public GameObject spectrePrefab;
    public GameObject crystalPrefab;
    public int numEnemiesKilledToGhoul;
    public int numBaddiesKilledToSpectre;
    public float crystalSpawnInterval;
    public float crystalSpawnRadius;
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
    int shotsFired;
    float totalKilled;
    public float accuracyBonusMultiplier;
    int crystalsCollected;
    
    void Start()
    {
        previousSpawnLocation = 99;
        totalBaddiesSpawned = 0;
        totalKilled = 0.0f;
        gameOver = false;
        crystalsCollected = 0;
        SpawnEnemy();
        InvokeRepeating("SpawnCrystal", crystalSpawnInterval, crystalSpawnInterval);
    }

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        player = FindObjectOfType<BotBehavior>();
        platforms = FindObjectsOfType<TransferBot>();
        scoreboard = FindObjectOfType<Scoreboard>();
        crystalScoreboard = FindObjectOfType<CrystalScoreboard>();
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
    }

    public void SpawnSpectre()
    {
        Vector3 spectreLocation = player.transform.position;
        TransferBot spectrePlatform;
        int playerPlatform = player.GetPlatformMode();
        playerPlatform += 2;
        if (playerPlatform > 3)
        {
            playerPlatform -= 4;
        }
        spectrePlatform = GetPlatformToSpawnSpectre(playerPlatform);
        spectreLocation = spectrePlatform.GetFartherSpectreSpawnLocation();
        Instantiate(spectrePrefab, spectreLocation, Quaternion.identity);
        totalBaddiesSpawned++;
    }

    public void IncTotalKilled()
    {
        totalKilled += 1.0f;
        if ((totalKilled % numBaddiesKilledToSpectre) == 0.0f)
        {
            SpawnSpectre();
        }
    }

    public void SpawnCrystal()
    {
        float xValue = Random.Range(-crystalSpawnRadius, crystalSpawnRadius);
        float yValue = Mathf.Sqrt((crystalSpawnRadius * crystalSpawnRadius) - (xValue * xValue));
        int isNegative = Random.Range(0, 2);
        if (isNegative == 1)
        {
            yValue = -yValue;
        }
        Vector3 crystalSpawnLocation = new Vector3(xValue, yValue, 0.0f);
        Instantiate(crystalPrefab, crystalSpawnLocation, Quaternion.identity);
    }

    public float GetTotalKilled()
    {
        return totalKilled;
    }

    public void IncShotsFired()
    {
        shotsFired++;
    }

    public void CalcScoreInc(float baddiePoints)
    {
        float newValue = (totalKilled / shotsFired) * accuracyBonusMultiplier;
        newValue += baddiePoints;
        int result = Mathf.CeilToInt(newValue);
        scoreboard.UpdateScore(result);
    }

    TransferBot GetPlatformToSpawnSpectre(int spawnPlatform)
    {
        TransferBot resultPlatform = gameObject.AddComponent<TransferBot>();
        foreach(TransferBot platform in platforms)
        {
            if (platform.GetPlatformNumber() == spawnPlatform)
            {
                resultPlatform = platform;
            }
        }
        return resultPlatform;
    }

    public void CollectCrystal()
    {
        crystalsCollected++;
        crystalScoreboard.UpdateCrystals(crystalsCollected);
        if (crystalsCollected >= 10)
        {
            // NEXT LEVEL **********************************************************************
            Debug.Log("NEXT LEVEL");
        }
    }
}
