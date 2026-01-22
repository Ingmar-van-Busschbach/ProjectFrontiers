using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float moveSpeed = 0.1f;
    public void Constructor(float damage)
    {
        damage = Mathf.Round(damage);
        text.text = damage.ToString();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position = transform.position + Vector3.up * moveSpeed * Time.deltaTime;
        transform.LookAt(PlayerIdentifier.Instance.transform.position);
    }
}
