using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnPickedSomething;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float interactionDistance = 1f; // Variable for maximum distance of raycast
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 moveDirection;

    private PlayerInputActions playerInputActions;
    private BaseCounter previouslyHighlightedClearCounter;
    private KitchenObject kitchenObject;

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
            //draw a line to see the raycast
            //Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red);

            // Check if the hit object has the class ClearCounter
            BaseCounter baseCounter = hit.transform.GetComponent<BaseCounter>();
            if (baseCounter != null)
            {
                baseCounter.Highlight(true);

                // Store the previously highlighted BaseCounter
                previouslyHighlightedClearCounter = baseCounter;

                // Check if the "E" key is pressed
                if (playerInputActions.Player.Interact.WasPressedThisFrame())
                {
                    // Call the method in BaseCounter class
                    baseCounter.Interact(this);
                }
                if (playerInputActions.Player.InteractAlternate.WasPressedThisFrame())
                {
                    baseCounter.InteractAlternate(this);
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

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}