using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(Weapon))]
public class EnemyTank : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Transform barrel;
    [Tooltip("Angel of field of vieuw")]
    [SerializeField] private float fieldOfView;
    [Tooltip("Length of field of vieuw")]
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private bool drawDebug;

    [Header("Rotation")]
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Transform verticalRotationTarget;
    [Tooltip("to rotate Vertically")]
    [Range(-180f, 180f)]
    [SerializeField] private float minRotationX = 0;
    [Tooltip("to rotate Vertically")]
    [Range(-180f, 180f)]
    [SerializeField] private float maxRotationX = 90;
    [Tooltip("To rotate horizontally")]
    [Range(-180f, 180f)]
    [SerializeField] private float minRotationY = 0;
    [Tooltip("To rotate horizontally")]
    [Range(-180f, 180f)]
    [SerializeField] private float maxRotationY = 90;
    [SerializeField] private float rotationSpeedX = 5;
    [SerializeField] private float rotationSpeedY = 5;

    private Weapon weapon;
    private Transform target;
    private Quaternion defaultRotation;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        defaultRotation = gameObject.transform.localRotation;
    }

    private void Update()
    {
        HandleRotation();
    }

    private void FixedUpdate()
    {
        CheckLineOfSight();
    }
        private void CheckLineOfSight()
    {
        if (ConePhysics.ConeCast(out RaycastHit hit, barrel.position, barrel.forward, fieldOfView, 3, 0.5f, viewDistance, PlayerLayer, false, 0.1f, QueryTriggerInteraction.Ignore, drawDebug, Color.cyan))
        {
            target = hit.collider.transform;
            //HandleLookAt();
            weapon.Shoot();
        }

        else
        {
           return;
        }
    }

    private void HandleRotation()
    {
        Quaternion intendedRotation = Quaternion.identity;
        if (target != null)
        {
            Vector3 intendedDirection = target.position - transform.position;
            intendedDirection = parentTransform.InverseTransformDirection(intendedDirection);
            intendedRotation = Quaternion.LookRotation(target.position - transform.position);
            intendedRotation = intendedRotation * Quaternion.Inverse(parentTransform.rotation);
        }
        else
        {
            intendedRotation = defaultRotation;
        }
            float rotationX = intendedRotation.eulerAngles.x;

        //Map rotationX from (0, 360) to (-180, 180)
        rotationX = ((rotationX + 180) % 360) - 180;

        if (maxRotationX - minRotationX < 360)
        {
            rotationX = Mathf.Clamp(rotationX, minRotationX, maxRotationX);
        }


        float rotationY = intendedRotation.eulerAngles.y;

        //Map rotationX from (0, 360) to (-180, 180)
        rotationY = ((rotationY + 180) % 360) - 180;

        if (maxRotationY - minRotationY < 360)
        {
            rotationY = Mathf.Clamp(rotationY, minRotationY, maxRotationY);
        }

        Vector3 intendedAngles = new Vector3(rotationX, rotationY, 0);
        Vector3 currentAngles = transform.localEulerAngles;
        if (verticalRotationTarget != null)
        {
            currentAngles.x = verticalRotationTarget.localEulerAngles.x;
        }

        //If the turret can rotate 360 degrees and is facing backwards, we don't map the intended angles to (-180,180)
        if ((rotationY < -170 || rotationY > 170) && maxRotationY - minRotationY == 360)
        {
            intendedAngles = new Vector3(intendedRotation.eulerAngles.x, intendedRotation.eulerAngles.y, 0);

        }
        //Else we map the current angles to (-180, 180)
        else {
            currentAngles = new Vector3(((currentAngles.x + 180) % 360) - 180, ((currentAngles.y + 180) % 360) - 180, 0);
        }    
        Vector3 moveAngles = intendedAngles - currentAngles;
        Debug.Log(moveAngles);

        moveAngles = new Vector3(Mathf.Clamp(moveAngles.x, -rotationSpeedX, rotationSpeedX), Mathf.Clamp(moveAngles.y, -rotationSpeedY, rotationSpeedY), 0);

        
        if(verticalRotationTarget != null)
        {
            Quaternion horiztonalRotation = Quaternion.Euler(new Vector3(0, currentAngles.y, 0) + new Vector3(0, moveAngles.y, 0));
            transform.localRotation = horiztonalRotation;
            Quaternion verticalRotation = Quaternion.Euler(new Vector3(currentAngles.x, 0, 0) + new Vector3(moveAngles.x, 0, 0));
            verticalRotationTarget.localRotation = verticalRotation;
        }
        else
        {
            Quaternion resultRotation = Quaternion.Euler(currentAngles + moveAngles);
            transform.localRotation = resultRotation;
        }
            
    }   
}

