using UnityEngine;
using UnityEngine.SceneManagement;

public class BotBehavior : MonoBehaviour
{
    Rigidbody2D botBody;
    PlayAnimations animationScript;
    public float speed;  // Default was 1.6
    Vector3 moveDirection;
    Vector3 platformGravity;
    bool isWalking;
    bool leapWalk;
    int platformMode;
    public float staggerSize;  // Default was 3
    public float movementBufferForAnim;


    // Start is called before the first frame update
    void Start()
    {
        platformMode = 0;
        moveDirection = Vector3.zero;
        leapWalk = true;
    }

    void Awake()
    {
        botBody = gameObject.GetComponent<Rigidbody2D>();
        animationScript = gameObject.GetComponent<PlayAnimations>();
        platformGravity = new Vector3(0.0f, -1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        Vector3 previousLocation = gameObject.transform.position;
        moveDirection = Vector3.zero;
        float xAxisVal = Input.GetAxis("Horizontal");
        float yAxisVal = Input.GetAxis("Vertical");
        // Horizontal movement modes
        if (Mathf.Abs(xAxisVal) >= 0.15f && platformMode == 0)
        {
            Vector3 movementVector = new Vector3(xAxisVal, 0.0f, 0.0f);
            moveDirection += movementVector;
        } else if (Mathf.Abs(xAxisVal) >= 0.15f && platformMode == 2) {
            Vector3 movementVector = new Vector3(xAxisVal, 0.0f, 0.0f);
            moveDirection += movementVector;
        }
        // Vertical movement modes
        if (Mathf.Abs(yAxisVal) >= 0.15f && platformMode == 1)
        {
            Vector3 movementVector = new Vector3(0.0f, yAxisVal, 0.0f);
            moveDirection += movementVector;
        } else if (Mathf.Abs(yAxisVal) >= 0.15f && platformMode == 3) {
            Vector3 movementVector = new Vector3(0.0f, yAxisVal, 0.0f);
            moveDirection += movementVector;
        }
        if (leapWalk && staggerSize != 0)
        {
            moveDirection.x /= staggerSize;
        }
        MoveBot(moveDirection);
        if (moveDirection.x < 0.0f || moveDirection.y > 0.0f)
        {
            if (platformMode == 0 || platformMode == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            } else if (platformMode == 2 || platformMode == 3) {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        } else if (moveDirection.x > 0.0f || moveDirection.y < 0.0f) {
            if (platformMode == 0 || platformMode == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            } else if (platformMode == 2 || platformMode == 3) {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (Mathf.Abs(previousLocation.x - gameObject.transform.position.x) >= movementBufferForAnim || Mathf.Abs(previousLocation.y - gameObject.transform.position.y) >= movementBufferForAnim)
        {
            isWalking = true;
        } else {
            isWalking = false;
        }
        animationScript.SetIsWalking(isWalking);
        flipLeapWalk();
    }

    void MoveBot(Vector3 moveVector)
    {
        moveVector *= speed;
        moveVector += platformGravity;
        botBody.transform.position += moveVector * Time.deltaTime;
    }

    public void SetGravity(Vector3 newGravity)
    {
        platformGravity = newGravity;
    }

    public Vector3 GetGravity()
    {
        return platformGravity;
    }

    public void flipLeapWalk()
    {
        if (leapWalk)
        {
            leapWalk = false;
        } else if (!leapWalk) {
            leapWalk = true;
        }
    }

    public void setPlatformMode(int mode)
    {
        platformMode = mode;
    }

    public int GetPlatformMode()
    {
        return platformMode;
    }
}
