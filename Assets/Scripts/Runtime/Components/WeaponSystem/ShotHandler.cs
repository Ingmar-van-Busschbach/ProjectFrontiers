using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class ShotHandler : MonoBehaviour
{
    public WeaponData weaponData;
    [HideInInspector] public bool canShoot = true;
    [HideInInspector] public WeaponObject weaponObject;
    [SerializeField] protected Transform[] barrelPoints;
    [SerializeField] protected CrosshairBloom crosshairHandler;
    [SerializeField] protected CameraRecoil cameraRecoilHandler;
    [SerializeField] protected GameObject[] objectsToIgnore;
    [SerializeField] protected bool debugEnabled;

    protected List<Transform> availableBarrels = new List<Transform>();
    protected Transform barrelPoint;

    private Vector2 currentDispersion;
    private Vector2 targetDispersion;
    private float timeOfNextShot;
    private int currentMagazine;
    private bool isReloading;
    private int barrelIndex;
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
            if (debugEnabled)
            {
                Debug.LogWarning("The crosshair handler has not been applied! Recoil only uses weapondata minimum dispersion now!", this);
            }
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
        timeOfNextShot = Time.time;
        SelectAvailableBarrels();
    }
    public void Shoot()
    {
        if (!canShoot || isReloading)
        {
            return;
        }
        if(barrelPoints.Length == 0)
        {
            if (debugEnabled)
            {
                Debug.LogWarning("The barrel point has not been applied! Unable to shoot!", this);
            }
            return;
        }
        if (objectsToIgnore == null)
        {
            if (debugEnabled)
            {
                Debug.LogWarning("No objects to ignore have been applied. Adding the player's geometry is recommended.", this);
            }
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
                SelectCurrentBarrel();
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
        audioSource.PlayOneShot(weaponData.reloadAudio);
        isReloading = true;
        yield return new WaitForSeconds(weaponData.reloadDuration);
        currentMagazine = weaponData.magazineSize;
        isReloading = false;
    }
    private void SelectAvailableBarrels()
    {
        availableBarrels.Clear();
        if(weaponData.weaponBarrelIndexes.Length == 0)
        {
            availableBarrels.Add(barrelPoints[0]);
        }
        else
        {
            foreach (int index in weaponData.weaponBarrelIndexes)
            {
                availableBarrels.Add(barrelPoints[index]);
            }
        } 
    }
    private void SelectCurrentBarrel()
    {
        barrelIndex++;
        barrelIndex = barrelIndex % availableBarrels.Count;
        barrelPoint = availableBarrels[barrelIndex];
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
                SelectCurrentBarrel();
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
            if (debugEnabled)
            {
                Debug.LogWarning("The recoil handler has not been applied! Unable to apply recoil!", this);
            }
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
