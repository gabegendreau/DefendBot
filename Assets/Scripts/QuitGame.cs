using UnityEngine;

public class QuitGame : MonoBehaviour
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
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
