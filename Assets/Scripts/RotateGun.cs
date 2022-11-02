using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public float rotationOffset;
    float rotationToMatchBot;
    Vector3 mousePos;
    Vector3 objectPos;
    
    // Start is called before the first frame update
    void Start()
    {
        rotationToMatchBot = 0.0f;
    }

    // Update is called once per frame
    void Update () {
        mousePos = Input.mousePosition;
        mousePos.z = 0.0f;

        objectPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        angle += rotationOffset;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void MatchBotRotation(float newDegrees)
    {
        rotationToMatchBot = newDegrees;
    }
}
