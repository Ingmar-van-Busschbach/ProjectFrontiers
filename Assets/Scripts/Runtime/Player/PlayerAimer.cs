using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform[] barrelEnds;
    [SerializeField] private LayerMask layerMask;

    private void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            foreach(Transform barrelEnd in barrelEnds)
            {
                barrelEnd.rotation = Quaternion.LookRotation(hit.point - barrelEnd.position, Vector3.up);
            }
        }
        else
        {
            foreach (Transform barrelEnd in barrelEnds)
            {
                barrelEnd.localRotation = Quaternion.identity;
            }
        }
    }
}
