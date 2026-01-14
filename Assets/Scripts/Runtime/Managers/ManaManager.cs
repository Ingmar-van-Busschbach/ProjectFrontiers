using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    public static ManaManager Instance { get; private set; }
    [SerializeField] private float maxMana;
    [Tooltip("In mana per second")]
    [SerializeField] private float manaRegenerationSpeed;
    [SerializeField] private Slider manaSlider;
    private float currentMana;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        currentMana = maxMana;
    }
    private void OnDestroy()
    {
        Instance = null; // This is technically not needed as on scene loading it should automatically delete the Instance reference, but it is a precaution.
    }

    private void Update()
    {
        currentMana = Mathf.Min(currentMana + manaRegenerationSpeed * Time.deltaTime, maxMana);
        if(manaSlider != null)
        {
            manaSlider.value = currentMana / maxMana;
        }
        else
        {
            Debug.LogWarning("Mana Manager does not have its display slider assigned!");
        }
    }

    public bool TryUseMana(float manaAmount)
    {
        if(currentMana > manaAmount)
        {
            currentMana -= manaAmount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
