using System;
using System.Net.Sockets;
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
    [Tooltip("to rotate Vertiacally")]
    [SerializeField] private float minRotationX = 0;
    [Tooltip("to rotate Vertiacally")]
    [SerializeField] private float maxRotationX = 90;
    [Tooltip("To rotate horizontally")]
    [SerializeField] private float minRotationY = 0;
    [Tooltip("To rotate horizontally")]
    [SerializeField] private float maxRotationY = 90;
    [SerializeField] private float rotationTime = 5;
         
    private Weapon weapon;
    private Transform target;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
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
            intendedRotation = Quaternion.LookRotation(target.position - transform.position);
        }
        
        //rotationClamp doesnt work, please fix yourself<3
        float rotationX = Mathf.Clamp(intendedRotation.eulerAngles.x, minRotationX, maxRotationX);
        float rotationY = Mathf.Clamp(intendedRotation.eulerAngles.y, minRotationY, maxRotationY);

        quaternion rotation = quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = (intendedRotation);
    }   
}

