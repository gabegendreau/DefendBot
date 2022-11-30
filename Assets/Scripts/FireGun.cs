using UnityEngine;

public class FireGun : MonoBehaviour
{
    SoundManager soundManager;
    ManageGame gameManager;
    public GameObject spawnPoint;
    public GameObject projectile;
    Vector3 spawnlocation;
    Quaternion spawnRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<ManageGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameManager.GetIsPaused())
        {
            spawnlocation = spawnPoint.transform.position;
            spawnRotation = gameObject.transform.rotation;
            Instantiate(projectile, spawnlocation, spawnRotation);
            soundManager.playShotFired();
            gameManager.IncShotsFired();
        }
        
    }
}
