using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float currentRotation;
    public void HandleVerticalLook(float lookInput, float lookSensitivity, Vector2 maxAngle)
    {
        currentRotation += lookInput * lookSensitivity * Time.deltaTime;
        currentRotation = Mathf.Clamp(currentRotation, maxAngle.x, maxAngle.y);
        transform.localRotation = Quaternion.Euler(currentRotation, 0, 0);
    }
}
