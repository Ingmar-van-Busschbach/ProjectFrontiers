using UnityEngine;
using UnityEngine.UI;

public class CrosshairBloom : MonoBehaviour
{
    [SerializeField] private Image[] image;
    [SerializeField] private float spacing = 10;
    [Tooltip("A multiplier for how far the crosshair should be pushed apart when the gun becomes inaccurate. Should generally be kept at the default value")]
    [SerializeField] private float accuracySize = 5;
    private float width;
    private float height;
    void Update()
    {
        //Create crosshairs at even spacing on a circle
        float angle = (Mathf.PI * 2) / image.Length;
        for (int i = 0; i < image.Length; i++)
        {
            //Affected by accuracy of the weapon
            float xPos = width * Mathf.Cos(angle * i);
            float yPos = height * Mathf.Sin(angle * i);
            //Default minimum spacing
            float xOffset = spacing * Mathf.Cos(angle * i);
            float yOffset = spacing * Mathf.Sin(angle * i);

            image[i].transform.localPosition = new Vector3(xPos + xOffset, yPos + yOffset, 0);
        }
    }

    public void SetBloom(Vector2 dispersion)
    {
        width = dispersion.x * accuracySize;
        height = dispersion.y * accuracySize;
    }
}
