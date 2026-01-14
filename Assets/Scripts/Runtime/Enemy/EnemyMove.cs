using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] List<Transform> Locations;
    [Tooltip("m/s")]
    [SerializeField] private float speed = 2;
    [Tooltip("Angle in degrees")]  
    [SerializeField] private float fieldOfView;
    [Tooltip("Length of field of view")]
    [SerializeField] private float viewDistance;
    [Tooltip("enemy doesn't go closer to player then this")]
    [SerializeField] private float minDistanceToPlayer;
    [Tooltip("m/s^2")]
    [SerializeField] private float gravityStrength = 2f;
    [Tooltip("m/s")]
    [SerializeField] private float maxFallSpeed = 10f;

    [SerializeField] private Transform eyeTransform;
    [SerializeField] private LayerMask PlayerLayer;

    private int currentLocation;
    private int nextLocation;
    private float acceptanceRadius = 0.7f;
    private float verticalVelocity;

    private CharacterController controller;

    public Transform target;
    public bool isFollowing = false;

    

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void FixedUpdate()
    {
        CheckLineOfSight();

        if (isFollowing) 
        {
            float distance = Vector3.Distance(transform.position, target.position);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            HandleGravity();
            controller.Move(Time.deltaTime * new Vector3(0, verticalVelocity, 0));
            if (distance > minDistanceToPlayer)
            {
                Vector3 direction = (target.position - transform.position);
                direction.y = 0;
                controller.Move(direction.normalized * Time.deltaTime * speed);
            }
        }
        else
        {
            if (Locations.Count == 0 || Locations == null)
            {
                return;
            }
            if (currentLocation < Locations.Count)
            {
                FollowInstuctions();
            }
            else
            {
                currentLocation = 0;
            }
        }
    }

    void FollowInstuctions()
    {
        if (Vector3.Distance(transform.position, Locations[nextLocation].position) < acceptanceRadius)
        {
            currentLocation = nextLocation;
            nextLocation = (currentLocation + 1) % Locations.Count;
        }
        Vector3 lookDirection = Locations[nextLocation].position - transform.position;
        lookDirection.y = 0; // never look down or up
        lookDirection.Normalize();
        Vector3 upDirection = Vector3.up;

        transform.rotation = Quaternion.LookRotation(lookDirection, upDirection);
        Vector3 moveDirection = lookDirection * Time.deltaTime * speed;
        if ((moveDirection).magnitude > (Locations[nextLocation].position - transform.position).magnitude)
        {
            moveDirection = Locations[nextLocation].position - transform.position;
        }

        controller.Move(moveDirection);
    }
    public void CheckLineOfSight()
    {
        if (ConePhysics.ConeCast(out RaycastHit hit, eyeTransform.position, eyeTransform.forward, fieldOfView, 3, 0.5f, viewDistance, PlayerLayer, false, 0.1f, QueryTriggerInteraction.Ignore, true, Color.cyan))
        {
            target = hit.collider.transform;
            isFollowing = true;
        }

        else { isFollowing = false; }
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
