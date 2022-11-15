using UnityEngine;
using UnityEngine.UI;

public class CrystalScoreboard : MonoBehaviour
{
    public Sprite crystalFilled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCrystals(int numCrystals)
    {
        Image[] allCrystals = GetComponentsInChildren<Image>();
        foreach (Image crystal in allCrystals)
        {
            Debug.Log("***********************");
            Debug.Log(crystal.gameObject.name);
            string crystalName = crystal.gameObject.name.Replace("Crystal", "");
            Debug.Log(crystalName);
            int crystalNum = int.Parse(crystalName);
            Debug.Log(crystalNum);
            if (crystalNum == numCrystals)
            {
                Debug.Log("inside the if statement");
                crystal.sprite = crystalFilled;
            }
        }
    }
}
