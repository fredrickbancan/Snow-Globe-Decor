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
    [SerializeField] private Camera playerCameraReference;
    [SerializeField] private float lightPosPickMaxDist = 10.0F;
    private bool ropeStartChosen = false;
    private Vector3 chosenRopeStartPos;
    private Vector3 chosenRopeEndPos;
    private DecorationType selectedDecorationType = DecorationType.CHRISTMAS_LIGHTS;


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
        }
    }

    private void DoPickRopeStart()
    {
        if(GetClickedWorldPos(out chosenRopeStartPos, out Vector3 norm))
        {
            Debug.Log(chosenRopeStartPos);
            ropeStartChosen = true;
        }
    }

    private void DoPickRopeEndAndCreate()
    {
        if(!GetClickedWorldPos(out chosenRopeEndPos, out Vector3 norm))
        {
            return;
        }
        DecorationRope dr = Instantiate(decorationRopePrefab);
        dr.CreateWithPhysics(selectedDecorationType, chosenRopeStartPos, chosenRopeEndPos);
        chosenRopeStartPos = chosenRopeEndPos;
        chosenRopeEndPos = Vector3.zero;
    }

    private bool GetClickedWorldPos(out Vector3 pos, out Vector3 normal)
    {
        RaycastHit hit;
        bool gotHit = Physics.Raycast(playerCameraReference.transform.position + playerCameraReference.transform.forward, playerCameraReference.transform.forward, out hit, lightPosPickMaxDist);

        pos = hit.point + hit.normal * 0.05F;
        normal = hit.normal;
        return gotHit;
    }
}
