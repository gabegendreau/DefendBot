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
            string crystalName = crystal.gameObject.name.Replace("Crystal", "");
            int crystalNum = int.Parse(crystalName);
            if (crystalNum == numCrystals)
            {
                crystal.sprite = crystalFilled;
            }
        }
    }
}
