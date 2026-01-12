using System.Runtime.InteropServices;
using UnityEngine;


[RequireComponent (typeof(Collider))]
public abstract class Health : MonoBehaviour, IDamageAble
{
    [SerializeField] private StructLibrary.Struct_ResistanceEntry[] resistanceEntries;
    [SerializeField] private float maxHealth = 100;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float damage, EnumLibrary.EDamageType damageType, RaycastHit hitData)
    {
        float currentDamage = damage;
        foreach (StructLibrary.Struct_ResistanceEntry resistanceEntry in resistanceEntries)
        {
            if (resistanceEntry.resistanceType == damageType)
            {
                currentDamage *= resistanceEntry.damageMultiplier + 1;
            }
        }
        currentHealth -= currentDamage;
        OnDamaged(damage);
        if (currentHealth <= 0)
        {
            OnDeath();
        }

    }

    protected abstract void OnDamaged(float damage);
    protected abstract void OnDeath();
}
