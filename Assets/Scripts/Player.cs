using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float interactionDistance = 1f; // Variable for maximum distance of raycast

    private bool isWalking;
    private Vector3 moveDirection;

    private PlayerInputActions playerInputActions;
    private ClearCounter previouslyHighlightedClearCounter;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleInteraction();
    }

    private void HandleMovementInput()
    {
        // Read move input from PlayerInputActions
        Vector2 moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        // Calculate movement direction based on input
        moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        // Normalize move direction if it's not zero
        if (moveDirection != Vector3.zero)
        {
            moveDirection = moveDirection.normalized;
        }

        // Move the player
        MovePlayer();

        // Update isWalking state
        CheckIsWalking();
    }

    private void MovePlayer()
    {
        // Move the player based on the calculated movement direction
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Rotate the player to face the movement direction (optional)
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void CheckIsWalking()
    {
        // Check if the player is currently moving
        isWalking = moveDirection.magnitude > 0f;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteraction()
    {
        // Shoot a raycast in front of the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            // Check if the hit object has the class ClearCounter
            ClearCounter clearCounter = hit.transform.GetComponent<ClearCounter>();
            if (clearCounter != null)
            {
                clearCounter.Highlight(true); // Highlight the ClearCounter

                // Store the previously highlighted ClearCounter
                previouslyHighlightedClearCounter = clearCounter;

                // Check if the "E" key is pressed
                if (playerInputActions.Player.Interact.WasPressedThisFrame())
                {
                    // Call the method in ClearCounter class
                    clearCounter.Interact();
                }
            }
        }
        else
        {
            // If the raycast doesn't hit anything, unhighlight the previously highlighted ClearCounter
            if (previouslyHighlightedClearCounter != null)
            {
                previouslyHighlightedClearCounter.Highlight(false);
                previouslyHighlightedClearCounter = null;
            }
        }
    }
}