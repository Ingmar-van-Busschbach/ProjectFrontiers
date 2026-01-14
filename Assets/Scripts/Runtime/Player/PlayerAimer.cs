using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform barrelEnd;
    [SerializeField] private LayerMask layerMask;

    private void Update()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            barrelEnd.rotation = Quaternion.LookRotation(hit.point - barrelEnd.position, Vector3.up);
        }
    }
}
