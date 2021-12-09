using UnityEngine;

public enum DecorationRopeType
{ 
    RANDOM_COLOR_LIGHTS,
    CHRISTMAS_LIGHTS,
    RED_LIGHTS,
    GREEN_LIGHTS,
    AMBER_LIGHTS,
    BLUE_LIGHTS,
    WHITE_LIGHTS
}

public class DecorationRopeMaker : MonoBehaviour
{
    [SerializeField] private float dangleToDistanceRatio = 0.25F;
    [SerializeField] private float distanceBetweenDecor = 0.05F;
    [SerializeField] private SpawnableLight spawnableChristmasLightPrefab;
    [SerializeField] private LineRenderer lineRendererPrefab;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 danglePoint;
    private float directDist = 0.0F;
    private float decorDistNormalized = 0.0F;

    public void Create(DecorationRopeType drt, Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        directDist = Vector3.Distance(startPoint, endPoint);
        danglePoint = startPoint + (endPoint - startPoint) * 0.5F + (Vector3.down * directDist * dangleToDistanceRatio);
        float apprxRopeLen = ApproximateRopeLength();
        int apprxLightCount = (int)(apprxRopeLen / distanceBetweenDecor);
        decorDistNormalized = distanceBetweenDecor / apprxRopeLen;
        LineRenderer lr = Instantiate(lineRendererPrefab);
        lr.positionCount = apprxLightCount + 1;
        int j = 0;
        for (float i = 0.0F; i < 1.0F; i += decorDistNormalized)
        {
            Vector3 pos = GetPositionOnRope(i);
            lr.SetPosition(j, pos + Vector3.up * 0.02F);
            InstantiateDecorationByType(drt, pos);
            j++;
        }
        
    }

    private GameObject InstantiateDecorationByType(DecorationRopeType drt, Vector3 pos)
    {
        GameObject result = null;
        SpawnableLight sl = null;
        switch (drt)
        {
            case DecorationRopeType.RANDOM_COLOR_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.RANDOM_COLORS);
                result = sl.gameObject;
                break;
            case DecorationRopeType.CHRISTMAS_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.CHRISTMAS_PATTERN);
                result = sl.gameObject;
                break;
            case DecorationRopeType.RED_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.RED);
                result = sl.gameObject;
                break;
            case DecorationRopeType.GREEN_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.GREEN);
                result = sl.gameObject;
                break;
            case DecorationRopeType.AMBER_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.AMBER);
                result = sl.gameObject;
                break;
            case DecorationRopeType.BLUE_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.BLUE);
                result = sl.gameObject;
                break;
            case DecorationRopeType.WHITE_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.WHITE);
                result = sl.gameObject;
                break;
            default:
                break;
        }
        return result;
    }

    private Vector3 GetPositionOnRope(float progress)
    {
        float progSq = progress * progress;
        float omp = 1.0F - progress;
        float ompSq = omp * omp;
        return startPoint * ompSq + danglePoint * 2 * omp * progress + endPoint * progSq;
        //P(t) = P0*(1-t)^2 + P1*2*(1-t)*t + P2*t^2
    }

    private float ApproximateRopeLength()
    {
        float dangleDist = Vector3.Distance(startPoint, danglePoint) + Vector3.Distance(danglePoint, endPoint);

        return (directDist + dangleDist) * 0.5F;
    }
}
