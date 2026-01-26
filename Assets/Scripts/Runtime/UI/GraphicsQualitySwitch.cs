using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class GraphicsQualitySwitch : MonoBehaviour
{
    [SerializeField] private bool expensiveChanges;
    private TMP_Dropdown graphicsDropdown;

    private void Start()
    {
        graphicsDropdown = GetComponent<TMP_Dropdown>();
    }

    public void GraphicsSettingChange(int index)
    {
        QualitySettings.SetQualityLevel(index, expensiveChanges);
    }
}
