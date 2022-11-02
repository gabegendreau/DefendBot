using UnityEngine;

public class FireGun : MonoBehaviour
{
    SoundManager soundManager;
    ManageGame gameManager;
    public GameObject spawnPoint;
    public GameObject projectile;
    Vector3 spawnlocation;
    Quaternion spawnRotation;
    float totalShotsFired;

    // Start is called before the first frame update
    void Start()
    {
        totalShotsFired = 0.0f;
    }

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<ManageGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spawnlocation = spawnPoint.transform.position;
            spawnRotation = gameObject.transform.rotation;
            Instantiate(projectile, spawnlocation, spawnRotation);
            soundManager.playShotFired();
            totalShotsFired += 1.0f;
            float shotsThatHit = gameManager.GetTotalKilled();
            if (shotsThatHit != 0.0f)
            {
                Debug.Log(shotsThatHit + "/" + totalShotsFired + " = " + shotsThatHit/totalShotsFired);
            }
        }
        
    }
}
