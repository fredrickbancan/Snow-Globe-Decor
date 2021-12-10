using UnityEngine;

public class GameManagerCameraProvider : MonoBehaviour
{
    [SerializeField] private Camera mainMenuCam;
    [SerializeField] private Camera playerGlobeCam;
    [SerializeField] private Camera playerTableTopCam;
    [SerializeField] private Camera tweenCam;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance_ProvideCameras(mainMenuCam, playerGlobeCam, playerTableTopCam, tweenCam);
    }

}
