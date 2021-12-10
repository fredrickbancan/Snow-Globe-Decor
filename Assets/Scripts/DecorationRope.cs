using System.Collections.Generic;
using UnityEngine;

public class DecorationRope : MonoBehaviour
{
    [SerializeField] private float dangleToDistanceRatio = 0.25F;
    [SerializeField] private float distanceBetweenLights = 0.2F;
    [SerializeField] private float distanceBetweenBaubles = 0.4F;
    [SerializeField] private float maxSimulationTime = 5.0F;
    [SerializeField] private SpawnableLight spawnableChristmasLightPrefab;
    [SerializeField] private SpawnableBauble spawnableBaublePrefab;
    [SerializeField] private LineRenderer lineRendererPrefab;

    private LineRenderer lineRenderer = null;
    private List<Rigidbody> ropeRigidBodies = null;
    private List<Vector3> ropeRigidBodiesPrevPositions = null;
    private List<Collider> ropeColliders = null;
    private List<DecorationRopePhysConstraint> ropeConstraints = null;
    private float secondsSimulated = 0.0F;
    private bool isSimulatingPhysics = false;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 danglePoint;
    private Vector3 startToEnd;
    private Vector3 startToEndDir;
    private float directDist = 0.0F;
    private float decorDistNormalized = 0.0F;
    private float ropeLineYOffset = 0.02F;
    private float distBetweenDecor = 0.0F;
    private int simItterations = 30;

    public void CreateWithPhysics(DecorationType drt, Vector3 startPoint, Vector3 endPoint)
    {
        isSimulatingPhysics = true;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        startToEnd = endPoint - startPoint;
        startToEndDir = startToEnd.normalized;
        directDist = startToEnd.magnitude;

        distBetweenDecor = drt == DecorationType.BAUBLES ? distanceBetweenBaubles : distanceBetweenLights;

        int physicsDecorCount = (int)(directDist / distBetweenDecor) - 1;
        if (physicsDecorCount < 0) physicsDecorCount = 0;

        lineRenderer = Instantiate(lineRendererPrefab);
        lineRenderer.positionCount = physicsDecorCount + 2;

        Vector3 decorStep = startToEndDir * distBetweenDecor;
        Vector3 currentDecorPos = startPoint;
        lineRenderer.SetPosition(0, currentDecorPos);

        DecorationRopePhysConstraint sh = new DecorationRopePhysConstraint();
        Rigidbody prevDecorRB = null;
        GameObject currentDecor = null;
        Rigidbody currentDecorRB = null;
        ropeRigidBodies = new List<Rigidbody>();
        ropeRigidBodiesPrevPositions = new List<Vector3>();
        ropeColliders = new List<Collider>();
        ropeConstraints = new List<DecorationRopePhysConstraint>();

       // currentDecorPos += decorStep;
        currentDecor = InstantiateDecorationByType(drt, currentDecorPos);
        currentDecorRB = currentDecor.GetComponent<Rigidbody>();
        ropeColliders.Add(currentDecor.GetComponent<Collider>());
        currentDecorRB.isKinematic = true;
        ropeRigidBodiesPrevPositions.Add(currentDecorPos);
        currentDecorPos += decorStep;

        ropeRigidBodies.Add(currentDecorRB);

        for (int i = 0; i < physicsDecorCount; i++)
        {
            prevDecorRB = currentDecorRB;
            currentDecor = InstantiateDecorationByType(drt, currentDecorPos);
            currentDecorRB = currentDecor.GetComponent<Rigidbody>();
            ropeRigidBodies.Add(currentDecorRB);
            ropeColliders.Add(currentDecor.GetComponent<Collider>());
            sh.rbA = prevDecorRB;
            sh.rbB = currentDecorRB;
            sh.restingDist = distBetweenDecor;
            ropeConstraints.Add(sh);
            lineRenderer.SetPosition(i + 1, currentDecorPos - decorStep);
            ropeRigidBodiesPrevPositions.Add(currentDecorPos);
            currentDecorPos += decorStep;
        }
        prevDecorRB = currentDecorRB;
        currentDecor = InstantiateDecorationByType(drt, currentDecorPos);
        ropeRigidBodiesPrevPositions.Add(currentDecorPos);
        currentDecorRB = currentDecor.GetComponent<Rigidbody>();
        currentDecorRB.isKinematic = true;

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentDecorPos);

        sh.rbA = prevDecorRB;
        sh.rbB = currentDecorRB;
        sh.restingDist = distBetweenDecor;
        ropeConstraints.Add(sh);
        ropeRigidBodies.Add(currentDecorRB);
        ropeColliders.Add(currentDecor.GetComponent<Collider>());

    }

    public void CreateBezier(DecorationType drt, Vector3 startPoint, Vector3 endPoint)
    {
        isSimulatingPhysics = false;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        directDist = Vector3.Distance(startPoint, endPoint);
        danglePoint = startPoint + (endPoint - startPoint) * 0.5F + (Vector3.down * directDist * dangleToDistanceRatio);
        float apprxRopeLen = ApproximateRopeLength();
        float distBetweenDecor = drt == DecorationType.BAUBLES ? distanceBetweenBaubles : distanceBetweenLights;
        int approxDecorCount = (int)(apprxRopeLen / distBetweenDecor);
        decorDistNormalized = distBetweenDecor / apprxRopeLen;
        LineRenderer lr = Instantiate(lineRendererPrefab);
        lr.positionCount = approxDecorCount + 1;
        int j = 0;
        for (float i = 0.0F; i < 1.0F; i += decorDistNormalized)
        {
            Vector3 pos = GetPositionOnRopeBezier(i);
            lr.SetPosition(j, pos + Vector3.up * ropeLineYOffset);
            InstantiateDecorationByType(drt, pos);
            j++;
        }

    }

    private void Update()
    {
        if (!isSimulatingPhysics) return;

        //Simulate Physics
        SimulateRopePhysics();

        //update line renderer positions
        for (int i= 0; i < ropeRigidBodies.Count; i++)
        {
            Vector3 pos = ropeRigidBodies[i].transform.position;
            lineRenderer.SetPosition(i, pos + Vector3.up * ropeLineYOffset);
        }

        secondsSimulated += Time.deltaTime;

        //check if should kill physics sim
        if(secondsSimulated >= maxSimulationTime)
        {
            isSimulatingPhysics = false;

            for(int i = 0; i < ropeRigidBodies.Count; i++)
            {
                Destroy(ropeRigidBodies[i]);
            }

            ropeRigidBodies.Clear();

            for (int i = 0; i < ropeColliders.Count; i++)
            {
                Destroy(ropeColliders[i]);
            }

            ropeColliders.Clear();
            ropeConstraints.Clear();
            ropeRigidBodiesPrevPositions.Clear();
            Destroy(this.gameObject);
        }
    }

    private void SimulateRopePhysics()
    {
        for (int i = 0; i < ropeRigidBodies.Count; i++)
        {
            Rigidbody p = ropeRigidBodies[i];
            if (p.isKinematic)
                continue;
            Vector3 positionBeforeUpdate = p.position;
            p.position += p.position - ropeRigidBodiesPrevPositions[i];
            p.position += Vector3.up * Physics.gravity.y * Time.deltaTime * Time.deltaTime;
            ropeRigidBodiesPrevPositions[i] = positionBeforeUpdate;
        }

        for (int i = 0; i < simItterations; i++)
        {
            foreach (DecorationRopePhysConstraint j in ropeConstraints)
            {
                Vector3 jointCentre = (j.rbA.position + j.rbB.position) / 2;
                Vector3 jointDirection = (j.rbA.position - j.rbB.position).normalized;
                if (!j.rbA.isKinematic)
                    j.rbA.position = jointCentre + jointDirection * (j.currentDist + distBetweenDecor) / 2;
                if (j.rbB.isKinematic)
                    continue;
                j.rbB.position = jointCentre - jointDirection * (j.currentDist + distBetweenDecor) / 2;
            }
        }
    }

    private GameObject InstantiateDecorationByType(DecorationType drt, Vector3 pos)
    {
        GameObject result = null;
        SpawnableLight sl = null;
        SpawnableBauble sb = null;
        switch (drt)
        {
            case DecorationType.CHRISTMAS_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.CHRISTMAS_PATTERN);
                result = sl.gameObject;
                ropeLineYOffset = 0.02F;
                break;
            case DecorationType.RED_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.RED);
                result = sl.gameObject;
                ropeLineYOffset = 0.02F;
                break;
            case DecorationType.GREEN_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.GREEN);
                result = sl.gameObject;
                ropeLineYOffset = 0.02F;
                break;
            case DecorationType.AMBER_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.AMBER);
                result = sl.gameObject;
                ropeLineYOffset = 0.02F;
                break;
            case DecorationType.BLUE_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.BLUE);
                result = sl.gameObject; 
                ropeLineYOffset = 0.02F;
                break;
            case DecorationType.WHITE_LIGHTS:
                sl = Instantiate(spawnableChristmasLightPrefab, pos, Quaternion.identity);
                sl.SetDecorationLightType(DecorationLightType.WHITE);
                result = sl.gameObject;
                ropeLineYOffset = 0.02F;
                break;
            case DecorationType.BAUBLES:
                sb = Instantiate(spawnableBaublePrefab, pos, Quaternion.identity);
                result = sb.gameObject;
                ropeLineYOffset = 0.14F;
                break;
            default:
                break;
        }
        return result;
    }

    private Vector3 GetPositionOnRopeBezier(float progress)
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
