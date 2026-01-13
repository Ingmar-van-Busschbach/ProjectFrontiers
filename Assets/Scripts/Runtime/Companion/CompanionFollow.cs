using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
    
public class CompanionFollow : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float speed;
    [SerializeField] private float gravityStrength = 2f;
    [SerializeField] private float maxFallSpeed = 10f;

    private float verticalVelocity;

    [SerializeField] private Transform followPoint;


    private CharacterController controller;

    //bool wasFoxWalking = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (followPoint != null)
        {
            float distance = Vector3.Distance(transform.position, followPoint.position);
            transform.LookAt(new Vector3 (followPoint.position.x, transform.position.y, followPoint.position.z));
            HandleGravity();
            controller.Move(Time.deltaTime * new Vector3(0, verticalVelocity, 0));
            if (distance > minDistance) 
            {
                Vector3 direction = (followPoint.position - transform.position);
                direction.y = 0;
                controller.Move(direction.normalized * Time.deltaTime * speed);
            }
        }
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


}

