using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float lifetime = 2f;
    [SerializeField] int damage = 1;

    Vector3 direction;

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6) // obstacle, todo to enum
        {
            if(other.TryGetComponent(out UnitHealth target))
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}