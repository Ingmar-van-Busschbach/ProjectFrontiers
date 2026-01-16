using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class AimHandler : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private Weapon weapon;
    private float normalFieldOfView;
    private float currentFieldOfView;
    
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
        cameraZoomDirection = Mathf.Clamp(cameraZoomDirection, -weapon.weaponData.aimDownSightsSpeed * Time.deltaTime, weapon.weaponData.aimDownSightsSpeed * Time.deltaTime);
        camera.fieldOfView = camera.fieldOfView + cameraZoomDirection;
    }
    public void HandleAim(bool aiming)
    {
        currentFieldOfView = aiming ? weapon.weaponData.aimDownSightsFieldOfView : normalFieldOfView;
    }
}
