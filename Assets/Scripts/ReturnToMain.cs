using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D mouseClick = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (mouseClick)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
