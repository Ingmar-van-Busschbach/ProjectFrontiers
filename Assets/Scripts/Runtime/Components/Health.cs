using UnityEngine;
using UnityEngine.UI;


[RequireComponent (typeof(Collider))]
public abstract class Health : MonoBehaviour, IDamageAble
{
    [SerializeField] private StructLibrary.Struct_ResistanceEntry[] resistanceEntries;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Slider healthSlider;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
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
            OnDamaged(damage);
        if (currentHealth <= 0)
        {
            OnDeath();
        }

    }

    private void UpdateHealthSlider()
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
    protected abstract void OnDamaged(float damage);
    protected abstract void OnDeath();
}
