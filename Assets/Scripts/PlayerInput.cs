using UnityEngine;

public enum PlayerInputMode
{
    IN_GLOBE,
    IN_MENU,
    CAMERA_TWEENING,
    TABLETOP_VIEW
}

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private DecorationSpawner decorSpawner;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 12.0f;
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private Camera playerCam;
    private PlayerInputMode currentInputMode = PlayerInputMode.IN_MENU;
    private float xRotation = 0.0f;
    private AudioSource footstepSound = null;
    [SerializeField] private AudioClip[] footstepSounds = null;

    private void Awake()
    {
        footstepSound = GetComponent<AudioSource>();
        GameManager.beginGame += OnBeginGame;
        GameManager.camTweenStart += OnCamTweenStart;
    }
    private void OnDestroy()
    {
        GameManager.beginGame -= OnBeginGame;
        GameManager.camTweenStart -= OnCamTweenStart;
    }

    private void OnBeginGame()
    {
        currentInputMode = PlayerInputMode.IN_GLOBE;
        GameManager.Instance_MakeHudVisible();
    }
    private void OnCamTweenStart()
    {
        currentInputMode = PlayerInputMode.CAMERA_TWEENING;
        GameManager.Instance_HideHud();
    }

    void Update()
    {
        switch (currentInputMode)
        {
            case PlayerInputMode.IN_GLOBE:
                HandleInput_IN_GLOBE();
                break;
            case PlayerInputMode.IN_MENU:
                HandleInput_IN_MAINMENU();
                break;
            case PlayerInputMode.CAMERA_TWEENING:
                HandleInput_CAMERA_TWEENING();
                break;
            case PlayerInputMode.TABLETOP_VIEW:
                HandleInput_TABLETOP_VIEW();
                break;
            default:
                break;
        }
        
    }

    private void HandlePauseRequestInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance_TogglePauseGame();
        }
    }

    private void HandleInput_IN_GLOBE()
    {
        HandlePauseRequestInput();

        if (GameManager.Instance_GetIsPaused())
            return;

        Vector2 moveInput;
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);
        controller.Move(Vector3.up * -9.8F);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        transform.Rotate(Vector3.up * mouseX);

        //Cycle footstep sounds while walking
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            if (!footstepSound.isPlaying)
            {
                int totalStepSounds = footstepSounds.Length;
                footstepSound.clip = footstepSounds[Random.Range(0, totalStepSounds)];
                footstepSound.Play();
            }
        }

        if (Input.mouseScrollDelta.y > 0.1F)
        {
            GameManager.DoWeaponScrollNext();
        }

        if (Input.mouseScrollDelta.y < -0.1F)
        {
            GameManager.DoWeaponScrollPrev();
        }

        decorSpawner.HandleInput();
    }

    private void HandleInput_IN_MAINMENU()
    {

    }

    private void HandleInput_CAMERA_TWEENING()
    {

    }

    private void HandleInput_TABLETOP_VIEW()
    {
        HandlePauseRequestInput();
    }
}
