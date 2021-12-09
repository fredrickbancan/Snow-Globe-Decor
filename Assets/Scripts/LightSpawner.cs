using UnityEngine;

public class LightSpawner : MonoBehaviour
{
    [SerializeField] private DecorationRopeMaker christmasLightRopePrefab;
    [SerializeField] private Camera playerCameraReference;
    [SerializeField] private float lightPosPickMaxDist = 10.0F;
    private bool ropeStartChosen = false;
    private Vector3 chosenRopeStartPos;
    private Vector3 chosenRopeEndPos;
    private DecorationRopeMaker christmasLightRopeInstance;


    private void Start()
    {
        christmasLightRopeInstance = Instantiate(christmasLightRopePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(ropeStartChosen)
            {
                DoPickRopeEndAndCreate();
                return;
            }
            DoPickRopeStart();
        }
    }

    private void DoPickRopeStart()
    {
        if(GetClickedRopeAnchorPos(out chosenRopeStartPos))
        {
            Debug.Log(chosenRopeStartPos);
            ropeStartChosen = true;
        }
    }

    private void DoPickRopeEndAndCreate()
    {
        if(!GetClickedRopeAnchorPos(out chosenRopeEndPos))
        {
            return;
        }
        christmasLightRopeInstance.Create(chosenRopeStartPos, chosenRopeEndPos);
        chosenRopeStartPos = chosenRopeEndPos;
        chosenRopeEndPos = Vector3.zero;
    }

    private bool GetClickedRopeAnchorPos(out Vector3 result)
    {
        RaycastHit hit;
        bool gotHit = Physics.Raycast(playerCameraReference.transform.position + playerCameraReference.transform.forward, playerCameraReference.transform.forward, out hit, lightPosPickMaxDist);

        result = hit.point + hit.normal * 0.1F;

        return gotHit;
    }
}
