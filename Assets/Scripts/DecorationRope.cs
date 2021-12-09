using UnityEngine;

enum DecorationRopeType
{ 
    CHRISTMAS_LIGHTS
}

public class DecorationRope : MonoBehaviour
{
    [SerializeField] private float dangleToDistanceRatio = 0.25F;
    [SerializeField] private float distanceBetweenDecor = 0.05F;
    [SerializeField] private GameObject spawnableChristmasLightPrefab;
    [SerializeField] private LineRenderer lineRendererPrefab;
    [SerializeField] private DecorationRopeType decorationType = DecorationRopeType.CHRISTMAS_LIGHTS;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 danglePoint;
    private float directDist = 0.0F;
    private float decorDistNormalized = 0.0F;

    public void Create(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        directDist = Vector3.Distance(startPoint, endPoint);
        danglePoint = startPoint + (endPoint - startPoint) * 0.5F + (Vector3.down * directDist * dangleToDistanceRatio);
        float apprxRopeLen = ApproximateRopeLength();
        int apprxLightCount = (int)(apprxRopeLen / distanceBetweenDecor);
        decorDistNormalized = distanceBetweenDecor / apprxRopeLen;

        GameObject decor = GetDecorationByType();
        

        LineRenderer lr = Instantiate(lineRendererPrefab);
        lr.positionCount = apprxLightCount + 1;
        int j = 0;
        for (float i = 0.0F; i < 1.0F; i += decorDistNormalized)
        {
            Vector3 pos = GetPositionOnRope(i);
            lr.SetPosition(j, pos + Vector3.up * 0.02F);
            Instantiate(decor, pos, Quaternion.identity);
            j++;
        }
        
    }

    private GameObject GetDecorationByType()
    {
        switch (decorationType)
        {
            case DecorationRopeType.CHRISTMAS_LIGHTS:
                return spawnableChristmasLightPrefab;
            default:
                return spawnableChristmasLightPrefab;
        }
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
