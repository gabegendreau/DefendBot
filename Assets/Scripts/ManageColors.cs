using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageColors : MonoBehaviour
{
    public float levelRed;
    public float levelGreen;
    public float levelBlue;
    public float levelAlpha;
    Color levelColor;

    void Start()
    {
        AdjustUIColors(GetLevelColor());
    }

    void Awake()
    {
        levelColor = new Color(levelRed, levelGreen, levelBlue, levelAlpha);
    }

    public Color GetLevelColor()
    {
        return levelColor;
    }

    void AdjustUIColors(Color newColor)
    {
        TextMeshProUGUI[] textObjects = FindObjectsOfType<TextMeshProUGUI>();
        CrystalScoreboard crystals = FindObjectOfType<CrystalScoreboard>();
        SpriteRenderer[] crystalRenderers = crystals.GetComponentsInChildren<SpriteRenderer>();
        foreach(TextMeshProUGUI textObject in textObjects)
        {
            textObject.faceColor = GetLevelColor();
        }
        foreach(SpriteRenderer spriteRenderer in crystalRenderers)
        {
            spriteRenderer.color = newColor;
        }
    }
}
