using System;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform moveOrientation;
    // Usually Main Camera, or Camera Pivot

    private Vector2 moveInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (moveOrientation == null && Camera.main != null)
        {
            moveOrientation = Camera.main.transform;
        }
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = GetCameraRelativeMoveDirection();
        Vector3 targetPosition = 
            rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);
    }

    private Vector3 GetCameraRelativeMoveDirection()
    {
        if (moveOrientation == null)
        {
            return new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        }
        
        Vector3 forward = moveOrientation.forward;
        Vector3 right = moveOrientation.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y +
                                right * moveInput.x;

        return moveDirection.normalized;
    }
}
