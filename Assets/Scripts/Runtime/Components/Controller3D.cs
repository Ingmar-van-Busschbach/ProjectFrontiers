using UnityEngine;

/// <summary>
/// 3D Character controller used for a player that moves forwards and backwards and rotates on sideways input.
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class Controller3D : MonoBehaviour
{
    [HideInInspector] public bool isSprinting;

    [Header("Movement Statistics")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float gravityStrength = 2f;
    [SerializeField] private float maxFallSpeed = 10f;

    private CharacterController controller;

    [Header("Physics")]
    [SerializeField] private LayerMask groundMask;
    private float verticalVelocity;
    private float currentRotation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void HandleGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f; // keep grounded
        }
        else
        {
            if (verticalVelocity < maxFallSpeed)
            {
                verticalVelocity -= gravityStrength * Time.fixedDeltaTime;
            }
        }
    }

    /// <summary>
    /// Call from FixedUpdate
    /// </summary>
    /// <param name="input">Control vector in xz space. Advised usage is move.ReadValue<Vector2>()</param>
    public void HandleMove(Vector2 moveInput)
    {
        HandleGravity();
        controller.Move(transform.rotation * ((isSprinting ? sprintSpeed : moveSpeed) * Time.fixedDeltaTime * new Vector3(moveInput.normalized.x, 0, moveInput.normalized.y) + Time.fixedDeltaTime * new Vector3(0, verticalVelocity, 0)));
        
    }

    public void HandleHorizontalLook(float lookInput, float lookSensitivity)
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + lookSensitivity * Time.fixedDeltaTime * lookInput, 0);
    }
}