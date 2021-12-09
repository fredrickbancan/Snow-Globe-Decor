using UnityEngine;

public class SpawnableLight : MonoBehaviour
{
    [SerializeField] private Light lightReference;
    [SerializeField] private LensFlare lightFlareReference;
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

    void Start()
    {
        RandomizeLightColor();
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
        lightFlareReference.color = lightColor;
        bulbMeshReference.material.color = lightColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
    }
}
