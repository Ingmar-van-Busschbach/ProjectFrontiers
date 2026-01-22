using UnityEngine;
using UnityEngine.UI;


[RequireComponent (typeof(Collider))]
public abstract class Health : MonoBehaviour, IDamageAble
{
    [SerializeField] private StructLibrary.Struct_ResistanceEntry[] resistanceEntries;
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] private Slider healthSlider;

    protected float currentHealth;

    protected void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
        BeginPlay();
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthSlider();
            OnDamaged(damage, hitData);
        if (currentHealth <= 0)
        {
            OnDeath();
        }

    }

    protected void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
        else
        {
            Debug.LogWarning(gameObject.name + "'s health component does not have its display slider assigned!");
        }
    }
    protected abstract void OnDamaged(float damage, RaycastHit hitData);
    protected abstract void OnDeath();

    protected abstract void BeginPlay();
}
