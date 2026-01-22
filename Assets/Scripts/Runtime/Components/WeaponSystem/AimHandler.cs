using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class AimHandler : MonoBehaviour
{
    [HideInInspector] public WeaponObject weaponObject;
    [SerializeField] private CrosshairBloom crossHair;
    [SerializeField] private Camera cam;
    private Weapon weapon;
    private float normalFieldOfView;
    private float currentFieldOfView;
    private Vector3 targetPosition;
    
    private void Start()
    {
        normalFieldOfView = cam.fieldOfView;
        currentFieldOfView = normalFieldOfView;
        weapon = GetComponent<Weapon>();
    }
    private void Update()
    {
        if(cam == null)
        {
            Debug.LogWarning("Aim Handler does not have a camera applied!");
            return;
        }
        float cameraZoomDirection = currentFieldOfView - cam.fieldOfView;
        cameraZoomDirection = Mathf.Clamp(cameraZoomDirection, -weapon.weaponData.aimDownSightFieldOfViewSpeed * Time.deltaTime, weapon.weaponData.aimDownSightFieldOfViewSpeed * Time.deltaTime);
        cam.fieldOfView = cam.fieldOfView + cameraZoomDirection;
        if (weaponObject == null)
        {
            return;
        }
        Vector3 moveDirection = targetPosition - weaponObject.transform.localPosition;
        moveDirection = Vector3.ClampMagnitude(moveDirection, weapon.weaponData.aimDownSightLerpSpeed * Time.deltaTime);
        weaponObject.transform.localPosition = weaponObject.transform.localPosition + moveDirection;
    }
    public void HandleAim(bool aiming)
    {
        if (cam == null)
        {
            Debug.LogWarning("Aim Handler does not have a camera applied!");
            return;
        }
        currentFieldOfView = aiming ? weapon.weaponData.aimDownSightFieldOfView : normalFieldOfView;
        targetPosition = aiming ? (cam.transform.localPosition - weaponObject.transform.parent.localPosition - weaponObject.sightTransform.localPosition) : Vector3.zero ;
        crossHair.gameObject.SetActive(!aiming);
    }
}
