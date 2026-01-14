using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private void Update()
    {
        canvas.transform.rotation = Quaternion.LookRotation((transform.position - PlayerIdentifier.Instance.transform.position).normalized, Vector3.up);
    }
}
