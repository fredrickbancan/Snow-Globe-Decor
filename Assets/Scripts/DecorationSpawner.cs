using UnityEngine;

public enum DecorationType
{
    CHRISTMAS_LIGHTS,
    RED_LIGHTS,
    GREEN_LIGHTS,
    AMBER_LIGHTS,
    BLUE_LIGHTS,
    WHITE_LIGHTS,
    BAUBLES,
    SNOWMAN,
    TREE
}

public class DecorationSpawner : MonoBehaviour
{
    [SerializeField] private DecorationRope decorationRopePrefab;
    [SerializeField] private GameObject snowmanPrefab;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject ropeAnchorIndicatorPrefab;
    [SerializeField] private LineRenderer ropeIndicatorPrefab;
    [SerializeField] private float ropeIndicatorSegmentLength = 0.25F;
    [SerializeField] private float ropeIndicatorLengthToDangleRatio = 0.15F;
    [SerializeField] private Camera playerCameraReference;
    [SerializeField] private float lightPosPickMaxDist = 10.0F;
    private GameObject ropeStartAnchorIndicator = null;
    private GameObject ropeEndAnchorIndicator = null;
    private LineRenderer ropeIndicator = null;
    private bool ropeStartChosen = false;
    private Vector3 chosenRopeStartPos;
    private Vector3 chosenRopeEndPos;
    private Vector3 chosenRopeDanglePoint;
    private DecorationType selectedDecorationType = DecorationType.CHRISTMAS_LIGHTS;

    //Sounds
    public GameSound ropeStartSound;
    public GameSound ropeEndSound;
    public GameSound spawnRopeSound;
    public GameSound spawnSnowmanSound;
    public GameSound spawnTreeSound;

    private void Start()
    {
        ropeStartAnchorIndicator = Instantiate(ropeAnchorIndicatorPrefab);
        ropeStartAnchorIndicator.transform.position = Vector3.zero;
        ropeEndAnchorIndicator = Instantiate(ropeAnchorIndicatorPrefab);
        ropeEndAnchorIndicator.transform.position = Vector3.zero;
        ropeIndicator = Instantiate(ropeIndicatorPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        int ropeTypeInt = (int)selectedDecorationType;

        if (Input.mouseScrollDelta.y < -0.1F)
        {
            ropeTypeInt--;
            if (ropeTypeInt < 0)
            {
                ropeTypeInt = 0;
            }
            else
            {
                ropeStartChosen = false;
                selectedDecorationType = (DecorationType)ropeTypeInt;
                Debug.Log("Selected Decor Type:" + selectedDecorationType);
                GameManager.DoMouseScrollDown();
            }
        }

        if (Input.mouseScrollDelta.y > 0.1F)
        {
            ropeTypeInt++;
            if (ropeTypeInt > 8)
            {
                ropeTypeInt = 8;
            }
            else
            {
                ropeStartChosen = false;
                selectedDecorationType = (DecorationType)ropeTypeInt;
                Debug.Log("Selected Decor Type:" + selectedDecorationType);
                GameManager.DoMouseScrollUp();
            }
        }

        if (selectedDecorationType == DecorationType.SNOWMAN || selectedDecorationType == DecorationType.TREE)
        {
            HandleInputSingleObjectMode();
            return;
        }
        HandleInputRopeMode();
    }

    private void HandleInputRopeMode()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (ropeStartChosen)
            {
                DoPickRopeEndAndCreate();
            }
            ropeStartChosen = false;
            ropeIndicator.positionCount = 1;
            ropeStartAnchorIndicator.transform.position = Vector3.zero;
            ropeEndAnchorIndicator.transform.position = Vector3.zero;
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            ropeIndicator.positionCount = 1;
            DoPickRopeStart();
            return;
        }

        if (ropeStartChosen && Input.GetMouseButton(0))//player is going to place rope
        {
            if (GetClickedWorldPos(out Vector3 pos, out Vector3 norm))
            {
                chosenRopeEndPos = pos;
                UpdateRopeIndicatorBezier();
                ropeEndAnchorIndicator.transform.position = chosenRopeEndPos;
            }
            else
            {
                ropeIndicator.positionCount = 1;
                ropeEndAnchorIndicator.transform.position = Vector3.zero;
            }
        }
    }

    private void HandleInputSingleObjectMode()
    {
        GameObject decor = null;
        switch (selectedDecorationType)
        { 
            case DecorationType.SNOWMAN:
                decor = snowmanPrefab;
                break;
            case DecorationType.TREE:
                decor = treePrefab;
                break;
            default:
                return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos, norm;
            if (!GetClickedWorldPos(out spawnPos, out norm))
                return;

            if (Vector3.Dot(norm, Vector3.up) < 0.5F)//only allow placements on horizontal surfaces
                return;

            Vector3 spawnToPlayer = playerCameraReference.transform.position - spawnPos;
            spawnToPlayer.y = 0;
            Vector3.Normalize(spawnToPlayer);
            GameObject spawnedDecor = Instantiate(decor, spawnPos, Quaternion.identity);
            spawnedDecor.transform.rotation *= Quaternion.LookRotation(spawnToPlayer);

            if (selectedDecorationType == DecorationType.SNOWMAN)
                SoundManager.Instance.PlaySound2D(spawnSnowmanSound);
            if (selectedDecorationType == DecorationType.TREE)
                SoundManager.Instance.PlaySound2D(spawnTreeSound);
        }
    }

    private void DoPickRopeStart()
    {
        if(GetClickedWorldPos(out chosenRopeStartPos, out Vector3 norm))
        {
            ropeStartChosen = true;
            ropeStartAnchorIndicator.transform.position = chosenRopeStartPos;
            SoundManager.Instance.PlaySound2D(ropeStartSound);
        }
    }

    private void DoPickRopeEndAndCreate()
    {
        if(!GetClickedWorldPos(out chosenRopeEndPos, out Vector3 norm) || Vector3.Distance( chosenRopeStartPos, chosenRopeEndPos) < 0.25F)
        {
            return;
        }
        DecorationRope dr = Instantiate(decorationRopePrefab);
        dr.CreateWithPhysics(selectedDecorationType, chosenRopeStartPos, chosenRopeEndPos);
        chosenRopeStartPos = chosenRopeEndPos;
        chosenRopeEndPos = Vector3.zero;
        SoundManager.Instance.PlaySound2D(ropeEndSound);
        SoundManager.Instance.PlaySound2D(spawnRopeSound);
    }

    private bool GetClickedWorldPos(out Vector3 pos, out Vector3 normal)
    {
        RaycastHit hit;
        bool gotHit = Physics.Raycast(playerCameraReference.transform.position + playerCameraReference.transform.forward, playerCameraReference.transform.forward, out hit, lightPosPickMaxDist);

        pos = hit.point + hit.normal * 0.05F;
        normal = hit.normal;
        return gotHit;
    }

    public void UpdateRopeIndicatorBezier()
    {
        float directDist = Vector3.Distance(chosenRopeStartPos, chosenRopeEndPos);
        chosenRopeDanglePoint = chosenRopeStartPos + (chosenRopeEndPos - chosenRopeStartPos) * 0.5F + (Vector3.down * directDist * ropeIndicatorLengthToDangleRatio);
        float apprxRopeLen = ApproximateRopeLength();
        int approxDecorCount = (int)(apprxRopeLen / ropeIndicatorSegmentLength);
        float decorDistNormalized = ropeIndicatorSegmentLength / apprxRopeLen;
        ropeIndicator.positionCount = approxDecorCount + 1;
        int j = 0;
        for (float i = 0.0F; i < 1.0F; i += decorDistNormalized)
        {
            Vector3 pos = GetPositionOnRopeBezier(i);
            ropeIndicator.SetPosition(j, pos);
            j++;
        }

    }

    private Vector3 GetPositionOnRopeBezier(float progress)
    {
        float progSq = progress * progress;
        float omp = 1.0F - progress;
        float ompSq = omp * omp;
        return chosenRopeStartPos * ompSq + chosenRopeDanglePoint * 2 * omp * progress + chosenRopeEndPos * progSq;
        //P(t) = P0*(1-t)^2 + P1*2*(1-t)*t + P2*t^2
    }

    private float ApproximateRopeLength()
    {
        float directDist = Vector3.Distance(chosenRopeStartPos, chosenRopeEndPos);
        float dangleDist = Vector3.Distance(chosenRopeStartPos, chosenRopeDanglePoint) + Vector3.Distance(chosenRopeDanglePoint, chosenRopeEndPos);

        return (directDist + dangleDist) * 0.5F;
    }
}
