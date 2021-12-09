
using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
struct ChristmasColors
{
    public Color redColor;
    public Color greenColor;
    public Color amberColor;
    public Color blueColor;
    public Color whiteColor;
}

public enum ChristmasColorID
{
    RED,
    GREEN,
    AMBER,
    BLUE,
    WHITE
}

public class ChristmasColorManager : MonoBehaviour
{
    [SerializeField]
    private ChristmasColors editorChristmasLightColors;
    [SerializeField]
    private ChristmasColors editorChristmasDecorationColors;

    private static Color[] christmasLightColors;
    private static Color[] christmasDecorationColors;

    private void Start()
    {
        ChristmasColorManager.christmasLightColors = new Color[5];
        christmasLightColors[0] = editorChristmasLightColors.redColor;
        christmasLightColors[1] = editorChristmasLightColors.greenColor;
        christmasLightColors[2] = editorChristmasLightColors.amberColor;
        christmasLightColors[3] = editorChristmasLightColors.blueColor;
        christmasLightColors[4] = editorChristmasLightColors.whiteColor;

        ChristmasColorManager.christmasDecorationColors = new Color[5];
        christmasDecorationColors[0] = editorChristmasDecorationColors.redColor;
        christmasDecorationColors[1] = editorChristmasDecorationColors.greenColor;
        christmasDecorationColors[2] = editorChristmasDecorationColors.amberColor;
        christmasDecorationColors[3] = editorChristmasDecorationColors.blueColor;
        christmasDecorationColors[4] = editorChristmasDecorationColors.whiteColor;

        SpawnableLight.SetGraphicsLevel(GraphicsTier.Tier1);
    }


    public static Color GetChristmasLightColor(int index)
    {
        return christmasLightColors[index];
    }

    public static Color GetChristmasDecorationColor(int index)
    {
        return christmasDecorationColors[index];
    }
}