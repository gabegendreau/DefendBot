using UnityEngine;

public class TransferBot : MonoBehaviour
{
    public int platformNumber;
    public float gravityScale;
    ManageColors colorManager;
    SpriteRenderer spriteRenderer;
    public GameObject[] ghoulSpawnLocations;
    Vector3 forceVector;
    Vector3 gravityDown;
    Vector3 gravityUp;
    Vector3 gravityLeft;
    Vector3 gravityRight;
    BotBehavior player;
    Rigidbody2D botBody;
    RotateGun gunScript;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.color = colorManager.GetLevelColor();
        forceVector = Vector3.zero;
        gravityDown = new Vector3(0.0f, -1.0f, 0.0f);
        gravityUp = new Vector3(0.0f, 1.0f, 0.0f);
        gravityLeft = new Vector3(-1.0f, 0.0f, 0.0f);
        gravityRight = new Vector3(1.0f, 0.0f, 0.0f);
    }

    void Awake()
    {
        gunScript = FindObjectOfType<RotateGun>();
        player = FindObjectOfType<BotBehavior>();
        botBody = player.GetComponent<Rigidbody2D>();
        colorManager = FindObjectOfType<ManageColors>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RotateAndSetGravity(platformNumber);
            player.setPlatformMode(platformNumber);
        }
        if (other.name.Contains("Ghoul"))
        {
            Vector3 newRotation;
            other.GetComponent<GhoulBehavior>().SetPlaformMode(platformNumber);
            switch(platformNumber)
            {
                case 0:
                    newRotation = new Vector3(0.0f, 0.0f, 0.0f);
                    other.GetComponent<GhoulBehavior>().SetGravity(gravityDown);
                    break;
                case 1:
                    newRotation = new Vector3(0.0f, 0.0f, -90.0f);
                    other.GetComponent<GhoulBehavior>().SetGravity(gravityLeft);
                    break;
                case 2:
                    newRotation = new Vector3(0.0f, 0.0f, -180.0f);
                    other.GetComponent<GhoulBehavior>().SetGravity(gravityUp);
                    break;
                case 3:
                    newRotation = new Vector3(0.0f, 0.0f, 90.0f);
                    other.GetComponent<GhoulBehavior>().SetGravity(gravityRight);
                    break;
                default:
                    newRotation = new Vector3(0.0f, 0.0f, 0.0f);
                    other.GetComponent<GhoulBehavior>().SetGravity(gravityDown);
                    Debug.Log("ghoul did not get an appropriate mode from the platform");
                    break;
            }
            other.GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    void RotateAndSetGravity(int mode)
    {
        Vector3 newRotation;
        switch(mode)
        {
            case 0:
                newRotation = new Vector3(0.0f, 0.0f, 0.0f);
                forceVector = gravityDown;
                break;
            case 1:
                newRotation = new Vector3(0.0f, 0.0f, -90.0f);
                forceVector = gravityLeft;
                break;
            case 2:
                newRotation = new Vector3(0.0f, 0.0f, -180.0f);
                forceVector = gravityUp;
                break;
            case 3:
                newRotation = new Vector3(0.0f, 0.0f, 90.0f);
                forceVector = gravityRight;
                break;
            default:
                newRotation = new Vector3(0.0f, 0.0f, 0.0f);
                forceVector = gravityDown;
                Debug.Log("Rotate bot function did not get an appropriate mode from the platform");
                break;
        }
        botBody.transform.rotation = Quaternion.Euler(newRotation);
        forceVector *= gravityScale;
        player.SetGravity(forceVector);
    }

    public int GetPlatformNumber()
    {
        return platformNumber;
    }

    public Vector3 GetFartherGhoulSpawnLocation()
    {
        Vector3 firstLocation = ghoulSpawnLocations[0].transform.position;
        Vector3 secondLocation = ghoulSpawnLocations[1].transform.position;
        float firstDistance = (player.transform.position - firstLocation).magnitude;
        float secondDistance = (player.transform.position - secondLocation).magnitude;
        if (firstDistance > secondDistance)
        {
            return firstLocation;
        } else {
            return secondLocation;
        }
    }

    public Vector3 GetFartherSpectreSpawnLocation()
    {
        Vector3 firstLocation = ghoulSpawnLocations[0].transform.position;
        Vector3 secondLocation = ghoulSpawnLocations[1].transform.position;
        float firstDistance = (player.transform.position - firstLocation).magnitude;
        float secondDistance = (player.transform.position - secondLocation).magnitude;
        if (firstDistance > secondDistance)
        {
            return firstLocation;
        } else {
            return secondLocation;
        }
    }
}
