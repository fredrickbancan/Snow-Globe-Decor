using UnityEngine;
using UnityEngine.Rendering;

public class SpawnableLight : MonoBehaviour
{
    [SerializeField] private Light lightReference;
    [SerializeField] private LensFlareComponentSRP lightFlareReference;
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
    private static int christmasLightIndex = 0;

    private static int skippedLightIndex = 0;
    private static int skippedLightIndex1 = 0;
    private static int skipLightNum = 3;
    private static int skipLightNum1 = int.MaxValue;
    private static float lightBrightnessCompensation = 0.333F;//value used to compensate for brightness lossed to culled lights 

    public static void SetGraphicsLevel(GraphicsTier graphics)
    {
        switch (graphics)
        {
            case GraphicsTier.Tier1://low tier graphics skips 3/4 christmas lights
                skipLightNum = 3;
                skipLightNum1 = 2;
                lightBrightnessCompensation = 0.75F;
                break;
            case GraphicsTier.Tier2://mid tier graphics skips 1/3 christmas lights
                skipLightNum = 3;
                skipLightNum1 = int.MaxValue;
                lightBrightnessCompensation = 0.333F;
                break;
            case GraphicsTier.Tier3://high tier grapics skips no christmas lights
                
                skipLightNum = int.MaxValue;
                skipLightNum1 = int.MaxValue;
                lightBrightnessCompensation = 0.0F;
                break;
            default:
                break;
        }
    }

    void Start()
    {
        //RandomizeLightColor();
        GetChristmasLightColor();
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

    private void GetChristmasLightColor()
    {
        lightColor = LightColorManager.GetChristmasLightColor(christmasLightIndex++);
        christmasLightIndex %= 4;
        bulbMeshReference.material.color = lightColor;
        bulbMeshReference.material.SetColor("_EmissionColor", lightColor * 4.0F);

        if ((skippedLightIndex = (skippedLightIndex + 1) % skipLightNum) == 0 || (skippedLightIndex1 = (skippedLightIndex1 + 1) % skipLightNum1) == 0)
        {
            Destroy(lightReference.gameObject);
            return;
        }

        lightColor.a = 1.0F;
        lightReference.color = lightColor;
        lightReference.intensity = lightReference.intensity * (1.0F + lightBrightnessCompensation);
       // lightReference.range = lightReference.range * (1.0F + lightBrightnessCompensation * 2.0F);
    }

    private void RandomizeLightColor()
    {
        lightColor.r = UnityEngine.Random.Range(0.0F, 1.0F);
        lightColor.g = UnityEngine.Random.Range(0.0F, 1.0F);
        lightColor.b = UnityEngine.Random.Range(0.0F, 1.0F);
        lightColor.a = 1.0F;

        float maxComponent = Mathf.Max(lightColor.r, Mathf.Max(lightColor.g, lightColor.b));
        lightColor.r /= maxComponent;
        lightColor.g /= maxComponent;
        lightColor.b /= maxComponent;

        lightReference.color = lightColor;
        bulbMeshReference.material.color = lightColor;
        bulbMeshReference.material.SetColor("_EmissionColor", lightColor * 4.0F);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
    }
}
