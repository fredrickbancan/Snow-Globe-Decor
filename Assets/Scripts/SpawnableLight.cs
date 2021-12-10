using UnityEngine;
using UnityEngine.Rendering;

public enum DecorationLightType
{
    CHRISTMAS_PATTERN,
    RED,
    GREEN,
    AMBER,
    BLUE,
    WHITE
}

public class SpawnableLight : MonoBehaviour
{
    [SerializeField] private Light lightReference;
    [SerializeField] private MeshRenderer bulbMeshReference;
    private Color lightColor;
    private float lightBrightness = 1.0F;
    private static float flickerMinBrightness = 0.8F;
    private static float flickerMaxBrightness = 1.0F;
    private static float flickerIntervalMin = 0.25F;
    private static float flickerIntervalMax = 1.0F;
    private static float flickerChangeSpeed = 0.7F;
    private float timeSinceLastFlicker = 0.0F;
    private float randomFlickerInterval = 0.0F;
    private float flickerDelta = 0.0F;
    private DecorationLightType decorationLightType;
    private bool lightEnabled = true;

    private static int christmasLightIndex = 0;

    private static int skippedLightIndexMonochrome = 0;
    private static int skippedLightIndexMonochrome1 = 0;
    private static int skippedLightIndex = 0;
    private static int skippedLightIndex1 = 0;
    private static int skipLightNumMonochrome = 2;
    private static int skipLightNumMonochrome1 = int.MaxValue;
    private static int skipLightNum = 3;
    private static int skipLightNum1 = int.MaxValue;
    private static float lightBrightnessCompensationMonochrome = 0.5F;
    private static float lightBrightnessCompensation = 0.333F;

    public static void SetGraphicsLevel(GraphicsTier graphics)
    {
        switch (graphics)
        {
            case GraphicsTier.Tier1://low tier graphics skips 3/4 christmas lights and 3/4 monochrome lights
                skipLightNumMonochrome = 3;
                skipLightNum = 3;
                skipLightNumMonochrome1 = 2;
                skipLightNum1 = 2;
                lightBrightnessCompensationMonochrome = 0.75F;
                lightBrightnessCompensation = 0.75F;
                break;
            case GraphicsTier.Tier2://mid tier graphics skips 1/3 christmas lights and 1/2 monochrome lights
                skipLightNumMonochrome = 2;
                skipLightNum = 3;
                skipLightNumMonochrome1 = int.MaxValue;
                skipLightNum1 = int.MaxValue;
                lightBrightnessCompensationMonochrome = 0.5F;
                lightBrightnessCompensation = 0.333F;
                break;
            case GraphicsTier.Tier3://high tier grapics also skips 1/3 christmas lights and 1/2 monochrome lights
                skipLightNumMonochrome = 2;
                skipLightNum = 3;
                skipLightNumMonochrome1 = int.MaxValue;
                skipLightNum1 = int.MaxValue;
                lightBrightnessCompensationMonochrome = 0.5F;
                lightBrightnessCompensation = 0.333F;
                //skipLightNum = int.MaxValue;
                //skipLightNum1 = int.MaxValue;
                //lightBrightnessCompensation = 0.0F;
                break;
            default:
                break;
        }
    }

    public void SetDecorationLightType(DecorationLightType dlt)
    {
        decorationLightType = dlt;
        switch (dlt)
        {
            case DecorationLightType.CHRISTMAS_PATTERN:
                SetColorChristmasPattern();
                break;
            case DecorationLightType.RED:
                SetLightMonochrome(ChristmasColorID.RED);
                break;
            case DecorationLightType.GREEN:
                SetLightMonochrome(ChristmasColorID.GREEN);
                break;
            case DecorationLightType.AMBER:
                SetLightMonochrome(ChristmasColorID.AMBER);
                break;
            case DecorationLightType.BLUE:
                SetLightMonochrome(ChristmasColorID.BLUE);
                break;
            case DecorationLightType.WHITE:
                SetLightMonochrome(ChristmasColorID.WHITE);
                break;
            default:
                break;
        }
    }

    private static bool ShouldSkipMonochromeLight()
    {
        return (skippedLightIndexMonochrome = (skippedLightIndexMonochrome + 1) % skipLightNumMonochrome) == 0 || (skippedLightIndexMonochrome1 = (skippedLightIndexMonochrome1 + 1) % skipLightNumMonochrome1) == 0;
    }

    private void SetLightMonochrome(ChristmasColorID color)
    {
        lightColor = ChristmasColorManager.GetChristmasLightColor((int)color);

        bulbMeshReference.material.color = lightColor;
        bulbMeshReference.material.SetColor("_EmissionColor", lightColor * 4.0F);

        if (ShouldSkipMonochromeLight())
        {
            Destroy(lightReference.gameObject);
            lightEnabled = false;
            return;
        }

        lightColor.a = 1.0F;
        lightReference.color = lightColor;
        lightReference.intensity = lightReference.intensity * (1.0F + lightBrightnessCompensationMonochrome);
    }

    private void SetColorChristmasPattern()
    {
        lightColor = ChristmasColorManager.GetChristmasLightColor((christmasLightIndex = (christmasLightIndex + 1) % 4));
        bulbMeshReference.material.color = lightColor;
        bulbMeshReference.material.SetColor("_EmissionColor", lightColor * 4.0F);

        if ((skippedLightIndex = (skippedLightIndex + 1) % skipLightNum) == 0 || (skippedLightIndex1 = (skippedLightIndex1 + 1) % skipLightNum1) == 0)
        {
            Destroy(lightReference.gameObject);
            lightEnabled = false;
            return;
        }

        lightColor.a = 1.0F;
        lightReference.color = lightColor;
        lightReference.intensity = lightReference.intensity * (1.0F + lightBrightnessCompensation);
    }

    private void Update()
    {

      /*  lightBrightness += flickerDelta * Time.deltaTime;
        lightBrightness = Math.Clamp(lightBrightness, flickerMinBrightness, flickerMaxBrightness);

        Color lightColorMod = lightColor;
        lightColorMod.r *= lightBrightness;
        lightColorMod.g *= lightBrightness;
        lightColorMod.b *= lightBrightness;
        lightReference.color = lightColorMod;
        bulbMeshReference.material.color = lightColorMod;

        timeSinceLastFlicker += Time.deltaTime;

        if (timeSinceLastFlicker < randomFlickerInterval)
        {
            return;
        }

        timeSinceLastFlicker = 0.0F;
        randomFlickerInterval = UnityEngine.Random.Range(flickerIntervalMin, flickerIntervalMax);
        flickerDelta = UnityEngine.Random.Range(0, 2) == 0 ? -flickerChangeSpeed : flickerChangeSpeed;*/
    }
}
