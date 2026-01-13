using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class ShotHandler : MonoBehaviour
{
    public WeaponData weaponData;
    [HideInInspector] public bool canShoot = true;

    [SerializeField] protected Transform barrelPoint;
    [SerializeField] protected CrosshairBloom crosshairHandler;
    [SerializeField] protected CameraRecoil cameraRecoilHandler;
    [SerializeField] protected GameObject[] objectsToIgnore;

    private Vector2 currentDispersion;
    private Vector2 targetDispersion;
    private float timeOfNextShot;
    private int currentMagazine;
    private bool isReloading;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        WeaponSetup();
    }
    private void Update()
    {
        if (crosshairHandler == null)
        {
            Debug.LogWarning("The crosshair handler has not been applied! Recoil only uses weapondata minimum dispersion now!", this);
            return;
        }
        targetDispersion = Vector2.Lerp(targetDispersion, weaponData.minDispersion, weaponData.dispersionRecoverySpeed * Time.deltaTime);
        currentDispersion = Vector2.Lerp(currentDispersion, targetDispersion, weaponData.dispersionBloomSpeed * Time.deltaTime);
        crosshairHandler.SetBloom(currentDispersion);
    }
    protected void WeaponSetup()
    {
        currentDispersion = weaponData.minDispersion;
        targetDispersion = weaponData.minDispersion;
        currentMagazine = weaponData.magazineSize;
    }
    public void Shoot()
    {
        if (!canShoot || isReloading)
        {
            return;
        }
        if(barrelPoint == null)
        {
            Debug.LogWarning("The barrel point has not been applied! Unable to shoot!", this);
            return;
        }
        if (objectsToIgnore == null)
        {
            Debug.LogWarning("No objects to ignore have been applied. Adding the player's geometry is recommended.", this);
        }
        if (Time.time > timeOfNextShot && currentMagazine > 0)
        {
            timeOfNextShot = Time.time + weaponData.timePerShotInBurst * (weaponData.shotsPerBurst - 1) + 60 / weaponData.rateOfFire;
            if(weaponData.shotsPerBurst > 1)
            {
                StartCoroutine(HandleBurst());
            }
            else
            {
                currentMagazine--;
                HandleMultishot();
            }
        }
    }
    public void Reload()
    {
        StartCoroutine(HandleReload());
    }
    private IEnumerator HandleReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(weaponData.reloadDuration);
        currentMagazine = weaponData.magazineSize;
        isReloading = false;
    }
    protected abstract void HandleShot();
    
    protected void HandleMultishot()
    {
        for(int i = 0; i < weaponData.multishot; i++)
        {
            HandleRecoil();
            HandleAudio();
            HandleShot();
        }
    }

    IEnumerator HandleBurst()
    {
        for(int i = 0; i < weaponData.shotsPerBurst; i++)
        {
            if(currentMagazine > 0)
            {
                currentMagazine--;
                HandleMultishot();
                yield return new WaitForSeconds(weaponData.timePerShotInBurst);
            }
        }
    }
    protected Quaternion HandleDispersion()
    {
        targetDispersion += weaponData.dispersionPerShot;
        targetDispersion = new Vector2(Mathf.Clamp(targetDispersion.x, weaponData.minDispersion.x, weaponData.maxDispersion.x), Mathf.Clamp(targetDispersion.y, weaponData.minDispersion.y, weaponData.maxDispersion.y));
        Vector2 dispersion = Random.insideUnitCircle;
        dispersion = new Vector2(dispersion.x * currentDispersion.x, dispersion.y * currentDispersion.y);
        return Quaternion.Euler(dispersion.x, dispersion.y, 0);
    }
    protected void HandleRecoil()
    {
        if (cameraRecoilHandler == null)
        {
            Debug.LogWarning("The recoil handler has not been applied! Unable to apply recoil!", this);
            return;
        }
        Vector2 recoilAmount = new Vector2((Random.value - 0.5f + weaponData.recoilOffset.x) / 2 * weaponData.recoilAmount.x, (Random.value - 0.5f + weaponData.recoilOffset.y) / 2 * weaponData.recoilAmount.y);
        cameraRecoilHandler.ApplyRecoil(recoilAmount, weaponData.recoilSnappiness, weaponData.recoilRecoverySpeed, weaponData.maxRecoilAngle);
    }

    protected void HandleAudio()
    {
        audioSource.PlayOneShot(weaponData.firingAudio);
    }

    public void SwapWeapon(WeaponData newWeapon)
    {
        weaponData = newWeapon;
        WeaponSetup();
    }
}
