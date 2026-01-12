using UnityEngine;

/// <summary>
/// Interface for applying damage to health components. You can use GetComponent<IDamageAble>() to get either script and apply damage
/// to them.
/// </summary>
public interface IDamageAble
{
    public void ApplyDamage(float damageAmount, EnumLibrary.EDamageType damageType, RaycastHit hitData);
}