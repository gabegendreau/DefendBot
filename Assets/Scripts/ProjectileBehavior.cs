using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed;
    public Sprite[] bombHitSprites;
    public float framesPerSecond;
    public float explosionScaleStep;
    Vector3 explosionScaleVector;
    Rigidbody2D projectileBody;
    SpriteRenderer spriteRenderer;
    Vector2 direction;
    Vector2 centerPoint;
    RotateGun gunScript;
    bool spriteChange;
    int spriteIndex;
    float frameRate;
    bool isBeingDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        explosionScaleVector = new Vector3(explosionScaleStep, explosionScaleStep, 0.0f);
        isBeingDestroyed = false;
        spriteIndex = 0;
        spriteChange = false;
        frameRate = CalcFrameRate();
        gunScript = FindObjectOfType<RotateGun>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        centerPoint = new Vector2(gunScript.gameObject.transform.position.x, gunScript.gameObject.transform.position.y);
        projectileBody = gameObject.GetComponent<Rigidbody2D>();
        Vector2 projectilePosition = new Vector2(projectileBody.transform.position.x, projectileBody.transform.position.y);
        direction = projectilePosition - centerPoint;
        projectileBody.velocity = direction * speed;
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isBeingDestroyed)
        {
            if (spriteChange)
            {
                spriteChange = false;
                gameObject.transform.localScale += explosionScaleVector;
                spriteRenderer.sprite = bombHitSprites[spriteIndex]; 
                spriteIndex++;
                if (spriteIndex == bombHitSprites.Length)
                {
                    isBeingDestroyed = true;
                    Invoke("DelayedDestroy", frameRate);
                } else {
                    Invoke("ResetAnimTimer", frameRate);
                }
            }
        }
    }

    float CalcFrameRate()
    {
        float result = 0.0f;
        result = 1.0f / framesPerSecond;
        return result;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Level" || other.tag == "Enemy")
        {
            projectileBody.velocity = Vector2.zero;
            spriteChange = true;
            if (other.name.Contains("Enemy"))
            {
                other.GetComponent<EnemyBehavior>().GetShot();
            }
            if (other.name.Contains("Ghoul"))
            {
                other.GetComponent<GhoulBehavior>().GetShot();
            }
            if (other.name.Contains("Spectre"))
            {
                other.GetComponent<SpectreBehavior>().GetShot();
            }
        }

        if (other.tag == "Crystal" && !other.GetComponent<CrystalBehavior>().GetCollected())
        {
            Destroy(gameObject);
        }
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
        
    }

    void ResetAnimTimer()
    {
        spriteChange = true;
    }
}
