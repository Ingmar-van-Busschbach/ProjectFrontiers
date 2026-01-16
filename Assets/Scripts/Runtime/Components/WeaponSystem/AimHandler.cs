using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class AimHandler : MonoBehaviour
{
    [HideInInspector] public AimDownSightHandler weaponObject;
    [SerializeField] private CrosshairBloom crossHair;
    [SerializeField] private Camera camera;
    private Weapon weapon;
    private float normalFieldOfView;
    private float currentFieldOfView;
    private Vector3 targetPosition;
    
    private void Start()
    {
        normalFieldOfView = camera.fieldOfView;
        currentFieldOfView = normalFieldOfView;
        weapon = GetComponent<Weapon>();
    }
    private void Update()
    {
        if(camera == null)
        {
            Debug.LogWarning("Aim Handler does not have a camera applied!");
            return;
        }
        float cameraZoomDirection = currentFieldOfView - camera.fieldOfView;
        cameraZoomDirection = Mathf.Clamp(cameraZoomDirection, -weapon.weaponData.aimDownSightFieldOfViewSpeed * Time.deltaTime, weapon.weaponData.aimDownSightFieldOfViewSpeed * Time.deltaTime);
        camera.fieldOfView = camera.fieldOfView + cameraZoomDirection;
        if (weaponObject == null)
        {
            Debug.LogWarning("Aim Handler does not have a weaponObject applied!");
            return;
        }
        Vector3 moveDirection = targetPosition - weaponObject.transform.localPosition;
        moveDirection = Vector3.ClampMagnitude(moveDirection, weapon.weaponData.aimDownSightLerpSpeed * Time.deltaTime);
        weaponObject.transform.localPosition = weaponObject.transform.localPosition + moveDirection;
    }
    public void HandleAim(bool aiming)
    {
        if (camera == null)
        {
            Debug.LogWarning("Aim Handler does not have a camera applied!");
            return;
        }
        currentFieldOfView = aiming ? weapon.weaponData.aimDownSightFieldOfView : normalFieldOfView;
        targetPosition = aiming ? (camera.transform.localPosition - weaponObject.transform.parent.localPosition - weaponObject.sightTransform.localPosition) : Vector3.zero ;
        crossHair.gameObject.SetActive(!aiming);
    }
}
