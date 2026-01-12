using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float currentRotation;
    public void HandleVerticalLook(float verticalInput, float sensitivity, Vector2 maxAngle)
    {
        currentRotation += verticalInput * sensitivity * Time.deltaTime;
        currentRotation = Mathf.Clamp(currentRotation, maxAngle.x, maxAngle.y);
        transform.localRotation = Quaternion.Euler(currentRotation, 0, 0);
    }
}
