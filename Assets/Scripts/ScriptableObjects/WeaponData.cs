using UnityEngine;

/// <summary>
/// Weapon Data object for weapon scripts.
/// </summary>

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Statistics")]
    public string weaponName = "DefaultWeapon";
    public GameObject weaponObject;
    public EnumLibrary.EWeaponType weaponType;

    [Space]
    [Tooltip("The amount of each damage type this weapon deals upon a successful hit")]
    public StructLibrary.Struct_DamageEntry[] damageData;
    [Range(10, 1000)]
    [Tooltip("Rate of fire in shots per minute")]
    public float rateOfFire = 100;
    
    [Space]
    [Range(1, 20)]
    [Tooltip("The amount of bullets that are fired with every shot")]
    public int multishot = 1;
    
    [Space]
    [Range(1, 10)]
    [Tooltip("The amount of shots that are fired when attack is triggered once")]
    public int shotsPerBurst = 1;
    [Tooltip("The time between shots in a burst, if shots per burst is larger than 1")]
    public float timePerShotInBurst = 0.03f;

    [Header("Magazine")]
    [Tooltip("The amount of bullets in the magazine")]
    public int magazineSize;
    public float reloadDuration;


    [Header("Audio")]
    public AudioClip firingAudio;
    public AudioClip reloadAudio;


    [Header("Accuracy")]
    [Tooltip("Minimum dispersion in degrees for the weapon. X is horizontal and Y is vertical")]
    public Vector2 minDispersion;
    [Tooltip("Maximum dispersion in degrees for the weapon. X is horizontal and Y is vertical")]
    public Vector2 maxDispersion;
    [Tooltip("The amount of dispersion in degrees that is added to the minimum dispersion with each shot,X is horizontal and Y is vertical")]
    public Vector2 dispersionPerShot;
    [Tooltip("The rate at which the current dispersion is lerped towards the target dispersion each second")]
    public float dispersionBloomSpeed;
    [Tooltip("The rate at which the current dispersion is lerped towards the minimum dispersion each second")]
    public float dispersionRecoverySpeed;
    

    [Header("Recoil")]
    [Tooltip("Max amount of recoil, randomly calculated per shot.")]
    public Vector2 recoilAmount;
    [Tooltip("Offset of the recoil. If X=-1, the recoil is always to the left. If X=1, it is always to the right. Y works the same way for vertical recoil. At extreme -1 or 1 values, it does double the recoil of the weapon in that direction.")]
    public Vector2 recoilOffset;
    [Tooltip("Max recoil angle in degrees")]
    public Vector2 maxRecoilAngle;

    [Space]
    [Tooltip("The rate at which the current recoil is lerped towards the target recoil each second")]
    public float recoilSnappiness;
    [Tooltip("The rate at which the current recoil is lerped towards zero recoil each second")]
    public float recoilRecoverySpeed;


    [Header("Range")]
    [Tooltip("Minimum range in meters")]
    public float minRange = 0.5f;
    [Tooltip("The max distance in meters where the weapon deals max damage. After this number linear falloff begins, reaching 0 damage at Max Range. Should be a number between Min Range and Max Range")]
    public float optimalRange = 5f;
    [Tooltip("Maximum range in meters")]
    public float maxRange = 15f;
    [Tooltip("Whether to round the damage after falloff to a whole number")]
    public bool roundDamage;


    [Header("RayCast Statistics")]
    [Range(0, 90)]
    [Tooltip("The angle in degrees that the Raycast uses for aim assist")]
    public float aimAssist = 0;
    [Range(1, 5)]
    [Tooltip("The amount of rings around the central raycast that are filled with additional raycasts. Should only be increased past 2 if there is a large angle of aim assist on the weapon")]
    public int aimAssistFidelity = 3;
    
    [Space]
    [Tooltip("Whether the target hits all damageable targets within the raycast, rather than only the closest target")]
    public bool penetratesTargets;
    [Tooltip("What layers should be tested with the raycasts")]
    public LayerMask layerMask;


    [Header("Projectile Statistics")]
    [Tooltip("What bullet prefab to use")]
    public Bullet bullet;
    [Tooltip("The velocity of the bullet in meters per second")]
    public float velocity;
    [Tooltip("Whether the bullet is affected by gravity")]
    public bool bulletDrop;

    [Space]
    [Range(0, 10)]
    [Tooltip("Whether the bullet has an explosion on impact, and how big it is. Disabled at 0")]
    public float explosionMaxRadius;
    [Range(0, 10)]
    [Tooltip("The optimal range at which the full damage of the weapon is dealt, then falling off towards the explosion maximum radius. Should be a smaller number than explosion maximum radius")]
    public float explosionOptimalRadius;

    [Space]
    [Tooltip("Enables homing behavior. You should configure the homing stats before enabling this. Homing always targets the closest target within the homing cone")]
    public bool isHoming;
    [Tooltip("If homing is enabled, this allows it to target the center of mass of the homing target rather than the hit point")]
    public bool targetCenterOfMass;
    [Tooltip("The rate at which the projectile homes in on the target")]
    public float homingSpeed;
    [Tooltip("The angle of the homing cone in degrees")]
    public float homingConeAngle;
    [Tooltip("The range of the homing cone in meters")]
    public float homingMaxRange;
    [Tooltip("The layers that will be homed in to")]
    public LayerMask homingLayerMask;
}
