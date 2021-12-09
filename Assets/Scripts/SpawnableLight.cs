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
        lightColor.a = 1.0F;
        lightReference.color = lightColor;
        bulbMeshReference.material.color = lightColor;
        bulbMeshReference.material.SetColor("_EmissionColor", lightColor * 4.0F);
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
