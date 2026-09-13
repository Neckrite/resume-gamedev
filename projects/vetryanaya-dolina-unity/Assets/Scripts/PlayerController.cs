using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5.0f;
    public float gravity = 9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2.0f;
    public float upDownRange = 60.0f;

    private CharacterController characterController;
    private float verticalRotation = 0;
    private float verticalVelocity = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleLook()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void HandleMovement()
    {
        float forwardSpeed = Input.GetAxis("Vertical") * walkSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * walkSpeed;

        Vector3 speed = new Vector3(sideSpeed, 0, forwardSpeed);
        speed = transform.rotation * speed;

        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        speed.y = verticalVelocity;

        characterController.Move(speed * Time.deltaTime);
    }
}
