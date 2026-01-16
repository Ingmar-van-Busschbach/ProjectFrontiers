using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    private Vector3 currentRotation; //Euler
    private Vector3 targetRotation; //Euler

    private float snappiness;
    private float recoverySpeed;

    private void Update()
    {
        targetRotation = Vector3.Slerp(targetRotation, Vector3.zero, recoverySpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void ApplyRecoil(Vector2 recoil, float snappiness, float recoverySpeed, Vector2 maxRecoilAngle)
    {
        targetRotation += new Vector3(recoil.y, recoil.x, 0);
        targetRotation = new Vector3(Mathf.Clamp(targetRotation.x, -maxRecoilAngle.y, maxRecoilAngle.y), Mathf.Clamp(targetRotation.y, -maxRecoilAngle.x, maxRecoilAngle.x));
        this.snappiness = snappiness;
        this.recoverySpeed = recoverySpeed;
    }
}
