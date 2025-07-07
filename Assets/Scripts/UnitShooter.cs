using UnityEngine;

public class UnitShooter : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float shootCooldown = 1f;

    float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;

        var target = FindClosestEnemy();
        if (target && cooldown <= 0f)
        {
            Shoot(target.position);
            cooldown = shootCooldown;
        }
    }

    void Shoot(Vector3 targetPos)
    {
        var dir = targetPos - transform.position;
        var proj = Instantiate(projectilePrefab, transform.position + dir.normalized, Quaternion.identity);
        proj.GetComponent<Projectile>().Init(dir);
    }

    Transform FindClosestEnemy()
    {
        float minDist = float.MaxValue;
        Transform closest = null;

        foreach (var enemy in FindObjectsByType<UnitHealth>(FindObjectsSortMode.None))
        {
            if (enemy.gameObject == gameObject) continue; // skip self
            var dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < attackRange && dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }
}
