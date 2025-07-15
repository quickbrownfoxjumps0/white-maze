using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float runSpeed = 32f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Camera")]
    public Transform playerCamera;
    private float xRotation = 0f;

    [Header("Head Bob")]
    public float bobFrequency = 8f;
    public float bobAmount = 0.05f;

    private float bobTimer = 0f;
    private Vector3 originalCameraLocalPos;

    private CharacterController controller;
    private PortalableObject portalableObject;

    private Vector3 velocity;
    private bool isGrounded;

    // For interpolation
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Vector3 renderPosition;
    private Quaternion renderRotation;

    private Vector3 moveInput;
    private bool jumpPressed;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        portalableObject = GetComponent<PortalableObject>();
        portalableObject.HasTeleported += PortalableObjectOnHasTeleported;

        Cursor.lockState = CursorLockMode.Locked;
        originalCameraLocalPos = playerCamera.localPosition;

        targetPosition = transform.position;
        targetRotation = transform.rotation;

        renderPosition = targetPosition;
        renderRotation = targetRotation;
    }

    private void PortalableObjectOnHasTeleported(Portal sender, Portal destination, Vector3 newposition, Quaternion newrotation)
    {
        Physics.SyncTransforms();

        // Ensure interpolation is reset after teleport
        targetPosition = newposition;
        targetRotation = newrotation;
        renderPosition = newposition;
        renderRotation = newrotation;
        transform.SetPositionAndRotation(newposition, newrotation);
    }

    private void Update()
    {
        HandleMouseLook();

        // Gather input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveInput = transform.right * x + transform.forward * z;
        if (moveInput.magnitude > 1f)
            moveInput.Normalize();

        jumpPressed = Input.GetButtonDown("Jump");

        // Interpolate position & rotation
        float interpSpeed = 50f;

        renderPosition = Vector3.Lerp(renderPosition, targetPosition, Time.deltaTime * interpSpeed);
        renderRotation = Quaternion.Slerp(renderRotation, targetRotation, Time.deltaTime * interpSpeed);

        transform.SetPositionAndRotation(renderPosition, renderRotation);

        HandleHeadBob(moveInput);
    }

    private void FixedUpdate()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.fixedDeltaTime;

        Vector3 moveDelta = moveInput * runSpeed * Time.fixedDeltaTime;
        Vector3 verticalDelta = new Vector3(0, velocity.y, 0) * Time.fixedDeltaTime;

        controller.Move(moveDelta + verticalDelta);

        // Save the true position for interpolation
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        targetRotation *= Quaternion.Euler(0f, mouseX, 0f);
    }

    void HandleHeadBob(Vector3 moveInput)
    {
        if (controller.isGrounded && moveInput.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;

            float bobOffsetY = Mathf.Sin(bobTimer) * bobAmount;
            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmount * 0.5f;

            playerCamera.localPosition = originalCameraLocalPos + new Vector3(bobOffsetX, bobOffsetY, 0f);
        }
        else
        {
            bobTimer = 0f;
            playerCamera.localPosition = Vector3.Lerp(
                playerCamera.localPosition,
                originalCameraLocalPos,
                Time.deltaTime * 5f
            );
        }
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }
}

