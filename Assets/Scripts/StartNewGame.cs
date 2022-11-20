using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNewGame : MonoBehaviour
{
    public GameObject projectile;
    SpriteRenderer spriteRenderer;

    void Start()
    {

    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            SceneManager.LoadScene(0);
        }
    }
}
