using UnityEngine;

public class SpawnableBauble : MonoBehaviour
{
    [SerializeField] private MeshRenderer baubleMeshReference;
    private Color baubleColor;
    private static int christmasColorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        baubleColor = ChristmasColorManager.GetChristmasDecorationColor((christmasColorIndex = (christmasColorIndex + 1) % 4));
        baubleMeshReference.material.color = baubleColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
