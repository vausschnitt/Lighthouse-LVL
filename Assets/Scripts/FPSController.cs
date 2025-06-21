using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 2f;
    public float jumpForce = 3f;
    public float gravity = -9.81f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    private bool isCrouching = false;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    [Header("Pickup Settings")]
    public float pickupRange = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalLookRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleCrouch();
        HandlePickup();
    }

    void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching) currentSpeed = sprintSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        bool crouchKeyHeld = Input.GetKey(KeyCode.C);
        isCrouching = crouchKeyHeld;

        // Set target height and camera Y
        targetHeight = isCrouching ? crouchHeight : standingHeight;
        targetCamY = isCrouching ? 0.5f : 1.5f;

        // Smoothly interpolate character height
        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        // Smoothly interpolate camera height
        Vector3 camPos = playerCamera.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCamY, crouchTransitionSpeed * Time.deltaTime);
        playerCamera.localPosition = camPos;
    }


    [Header("Crouch Transition")]
    public float crouchTransitionSpeed = 6f; // Adjust sendiri ke ape (higher = faster)

    private float targetHeight;
    private float targetCamY;
    void HandlePickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.green, 1f); // VISUAL DEBUG

            int layerMask = ~LayerMask.GetMask("Player"); // Ignore the "Player" layer
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, layerMask))
            {
                Debug.Log("Raycast hit: " + hit.collider.name);

                if (hit.collider.CompareTag("Pickup"))
                {
                    Debug.Log("Picking up: " + hit.collider.name);
                    Destroy(hit.collider.gameObject);
                }
            }
            else
            {
                Debug.Log("No pickupable object in sight");
            }
        }
    }

}
