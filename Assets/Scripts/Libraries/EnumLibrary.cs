using UnityEngine;

public class EnumLibrary : MonoBehaviour
{
    public enum EDamageType { impact, puncture, slash, heat, cold, toxic, electric, healing };
    public enum EWeaponType { hitscan, projectile };

    public enum EAttackType { Homing, Area };
}
