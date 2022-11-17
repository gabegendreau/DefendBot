using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public GameObject projectile;
    ManageColors colorManager;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer.color = colorManager.GetLevelColor();
    }

    void Awake()
    {
        colorManager = FindObjectOfType<ManageColors>();
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
