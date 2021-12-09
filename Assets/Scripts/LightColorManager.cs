
using System;
using UnityEngine;

[Serializable]
struct ChristmasColors
{
    public Color redLightColor;
    public Color greenLightColor;
    public Color amberLightColor;
    public Color blueLightColor;
}

public class LightColorManager : MonoBehaviour
{
    [SerializeField]
    private ChristmasColors editorChristmasColors;

    private static Color[] christmasColors;

    private void Start()
    {
        LightColorManager.christmasColors = new Color[4];
        christmasColors[0] = editorChristmasColors.redLightColor;
        christmasColors[1] = editorChristmasColors.greenLightColor;
        christmasColors[2] = editorChristmasColors.amberLightColor;
        christmasColors[3] = editorChristmasColors.blueLightColor;
    }


    public static Color GetChristmasLightColor(int index)
    {
        return christmasColors[index % 4];
    }

}