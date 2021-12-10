using UnityEngine;

public class DecorationSpawner : MonoBehaviour
{
    [SerializeField] private DecorationRope decorationRopePrefab;
    [SerializeField] private Camera playerCameraReference;
    [SerializeField] private float lightPosPickMaxDist = 10.0F;
    private bool ropeStartChosen = false;
    private Vector3 chosenRopeStartPos;
    private Vector3 chosenRopeEndPos;
    private DecorationRopeType selectedRopeType = DecorationRopeType.CHRISTMAS_LIGHTS;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ropeStartChosen)
            {
                DoPickRopeEndAndCreate();
            }
            else
            {
                DoPickRopeStart();
            }        
        }

        int ropeTypeInt = (int)selectedRopeType;

        if (Input.mouseScrollDelta.y < -0.1F)
        {
            ropeTypeInt++;
            if (ropeTypeInt > 6) ropeTypeInt = 6;
            selectedRopeType = (DecorationRopeType)ropeTypeInt;
            Debug.Log("Selected Decor Type:" + selectedRopeType);
        }

        if (Input.mouseScrollDelta.y > 0.1F)
        {
            ropeTypeInt--;
            if (ropeTypeInt < 0) ropeTypeInt = 0;
            selectedRopeType = (DecorationRopeType)ropeTypeInt;
            Debug.Log("Selected Decor Type:" + selectedRopeType);
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
        DecorationRope dr = Instantiate(decorationRopePrefab);
        dr.CreateWithPhysics(selectedRopeType, chosenRopeStartPos, chosenRopeEndPos);
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
