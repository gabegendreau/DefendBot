using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour
{
    SoundManager soundManager;
    BotBehavior player;
    Scoreboard scoreboard;
    StationBehavior station;
    CrystalScoreboard crystalScoreboard;
    int levelNumber;
    public GameObject levelDisplay;
    TextMeshProUGUI levelDisplayText;
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
    public GameObject nextLevelText;
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
        levelNumber = 1;
        previousSpawnLocation = 99;
        totalBaddiesSpawned = 0;
        totalKilled = 0.0f;
        gameOver = false;
        crystalsCollected = 0;
        levelDisplayText.text = levelNumber.ToString();
        SpawnEnemy();
        Invoke("SpawnCrystal", crystalSpawnInterval);
    }

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        player = FindObjectOfType<BotBehavior>();
        platforms = FindObjectsOfType<TransferBot>();
        scoreboard = FindObjectOfType<Scoreboard>();
        crystalScoreboard = FindObjectOfType<CrystalScoreboard>();
        levelDisplayText = levelDisplay.GetComponent<TextMeshProUGUI>();
        station = FindObjectOfType<StationBehavior>();
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
        GameObject[] leftoverBatteries = GameObject.FindGameObjectsWithTag("Battery");
        if (leftoverBatteries.Length != 0)
        {
            foreach(GameObject battery in leftoverBatteries)
            {
                Destroy(battery.gameObject);
            }
        }
        GameObject leftoverCrystal = GameObject.FindGameObjectWithTag("Crystal");
        if (leftoverCrystal)
        {
            Destroy(leftoverCrystal);
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
            Invoke("SpawnGhoul", 0.5f);
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
            Invoke("SpawnSpectre", 0.75f);
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
        Invoke("SpawnCrystal", crystalSpawnInterval);
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
            nextLevelText.SetActive(true);
            Invoke("LoadNextLevel", 1.0f);
        }
    }

    void LoadNextLevel()
    {
        levelNumber++;
        crystalSpawnInterval += 1.0f;
        nextLevelText.SetActive(false);
        levelDisplayText.text = levelNumber.ToString();
        station.RechargePower();
        crystalsCollected = 0;
        crystalScoreboard.ResetCrystals();
        numEnemiesKilledToGhoul--;
        numBaddiesKilledToSpectre--;
        minSpawnInterval -= 0.1f;
        if (minSpawnInterval < 0.5f)
        {
            minSpawnInterval = 0.5f;
        }
        maxSpawnInterval -= 0.12f;
        if (maxSpawnInterval < 0.75f)
        {
            maxSpawnInterval = 0.75f;
        }
        accuracyBonusMultiplier += 1.0f;
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }
}
