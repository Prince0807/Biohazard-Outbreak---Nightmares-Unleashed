using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FpsController : MonoBehaviour
{
    public Camera playerCamera;
    CharacterController characterController;


    [HideInInspector] public bool canMove = true;

    [Header("Movement")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    [HideInInspector] public Vector3 moveDirection = Vector3.zero;

    private float movementDirectionY;
    private float xInput;
    private float yInput;

    [Space]
    [Header("Jump")]
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    [Space]
    [Header("Camera Rotation")]
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    float rotationX = 0;

    [Space]
    [Header("Crouch")]
    public bool isCrouching = false;
    public float crouchHeight = 0.5f;
    public float crouchSpeed = 3.5f;
    public float crouchTransitionSpeed = 10f;
    private float originalHeight;
    public float crouchCameraOffset = -0.5f;
    private Vector3 cameraStandPosition;
    private Vector3 cameraCrouchPosition;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        // Define camera positions for standing and crouching
        originalHeight = characterController.height;
        cameraStandPosition = playerCamera.transform.localPosition;
        cameraCrouchPosition = cameraStandPosition + new Vector3(0, crouchCameraOffset, 0);
    }

    void Update()
    {
        HandleCrouching();
        HandleMovement();
        HandleJumping();

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void LateUpdate()
    {
            HandleCameraRotation();
    }

    private void HandleMovement()
    {
        if (!canMove)
            return;

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrouching;

        movementDirectionY = moveDirection.y;
        moveDirection = (forward * yInput) + (right * xInput);
        moveDirection.Normalize();
        moveDirection = (isRunning ? runningSpeed : (isCrouching ? crouchSpeed : walkingSpeed)) * moveDirection;
        moveDirection.y = movementDirectionY;
    }

    private void HandleCameraRotation()
    {
        if (!canMove)
            return;

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private void HandleCrouching()
    {
        if (!canMove)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            HandleCrouching(isCrouching);
        }
    }
    private void HandleCrouching(bool _isCrouching)
    {
        if (_isCrouching)
        {
            characterController.height = crouchHeight;
            walkingSpeed = crouchSpeed;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraCrouchPosition, crouchTransitionSpeed * Time.deltaTime);
        }
        else
        {
            characterController.height = originalHeight;
            walkingSpeed = 7.5f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraStandPosition, crouchTransitionSpeed * Time.deltaTime);
        }
    }

    private void HandleJumping()
    {
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }
}
