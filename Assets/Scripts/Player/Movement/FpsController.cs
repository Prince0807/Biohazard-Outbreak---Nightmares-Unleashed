    using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class FpsController : MonoBehaviour, IDamageable
{
    public Camera playerCamera;
    PlayerInput playerInput;
    CharacterController characterController;


    [HideInInspector] public bool canMove = true;

    [Header("Movement")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    [HideInInspector] public Vector3 moveDirection = Vector3.zero;

    private float movementDirectionY;

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

    public int health { get; set; }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        
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

        PlayerInput.crouchInput += HandleCrouching;
        PlayerInput.jumpInput += HandleJumping;
    }

    void Update()
    {
        HandleMovement();
        HandleFalling();

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

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (playerInput.isSprinting)
            HandleCrouching(false);

        movementDirectionY = moveDirection.y;
        moveDirection = (forward * playerInput.movementInput.y) + (right * playerInput.movementInput.x);
        moveDirection.Normalize();
        moveDirection = (playerInput.isSprinting ? runningSpeed : (isCrouching ? crouchSpeed : walkingSpeed)) * moveDirection;
        moveDirection.y = movementDirectionY;
    }

    private void HandleCameraRotation()
    {
        if (!canMove)
            return;

        rotationX += -playerInput.mouseInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, playerInput.mouseInput.x * lookSpeed, 0);
    }

    private void HandleCrouching()
    {
        if (!canMove)
            return;

        isCrouching = !isCrouching;
        HandleCrouching(isCrouching);
    }
    private void HandleCrouching(bool _isCrouching)
    {
        isCrouching = _isCrouching;
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
        if (canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            HandleCrouching(false);
        }
    }

    private void HandleFalling()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
    }

    public void Damage(int damage)
    {
        health -= damage;
    }
}
