using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 12.0f;
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] private Camera playerCam;
    private float xRotation = 0.0f;
    private AudioSource footstepSound = null;
    [SerializeField] private AudioClip[] footstepSounds = null;

    private void Awake()
    {
        footstepSound = GetComponent<AudioSource>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
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
    }
}
