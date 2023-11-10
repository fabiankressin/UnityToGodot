using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveSpeedSprintModifier = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float playerHeight = 1f;
    [SerializeField] private GameInput gameInput;

    private Rigidbody rb;

    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool isGrounded = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Player instance");
        }
        Instance = this;

        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        gameInput.OnSprintPressAction += GameInput_OnSprintPressAction;
        gameInput.OnSprintReleaseAction += GameInput_OnSprintReleaseAction;
        gameInput.OnCrouchPressAction += GameInput_OnCrouchPressAction;
        gameInput.OnCrouchReleaseAction += GameInput_OnCrouchReleaseAction;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandlePlayerObjectMovement();
    }

    private void HandlePlayerObjectMovement()
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;

        if (isSprinting)
        {
            moveDistance = (moveSpeed + moveSpeedSprintModifier) * Time.deltaTime;
        }

        if (isCrouching)
        {
            //for later
        }

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);


        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
    }

    private void HandleInteract()
    {
        //for later
    }

    private void Jump()
    {
        if (isGrounded)
        {

        }
    }

    private void Crouch()
    {

    }

    private void GameInput_OnSprintPressAction(object sender, System.EventArgs e)
    {
        isSprinting = true;
    }

    private void GameInput_OnSprintReleaseAction(object sender, System.EventArgs e)
    {
        isSprinting = false;
    }

    private void GameInput_OnCrouchPressAction(object sender, System.EventArgs e)
    {
        isCrouching = true;
    }

    private void GameInput_OnCrouchReleaseAction(object sender, System.EventArgs e)
    {
        isCrouching = false;
    }

    private void GameInput_OnJumpAction(object sender, System.EventArgs e)
    {
        Jump();
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        HandleInteract();
    }
}
