using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float avoidanceStrength = 10f;
    [SerializeField] float obstacleRayLength = 4f;
    [SerializeField] LayerMask obstacleMask = 6;
    [SerializeField] float initialCooldown = 100f;
    [SerializeField] float turnSpeed = 720f; // degrees per second

    float cooldown = 0f;
    Vector3? target;

    public void MoveTo(Vector3 position)
    {
        target = position;
    }

    void Update()
    {
        if (!target.HasValue) return;

        Vector3 dir = (target.Value - transform.position).normalized;
        Vector3 moveVec = dir;

        // Obstacle avoidance
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, obstacleRayLength, obstacleMask);
        if (hit.collider != null || cooldown > 0f)
        {
            if (hit.collider != null) cooldown = initialCooldown;
            Vector3 right = Vector3.Cross(dir, Vector3.forward); // perpendicular in 2D plane
            moveVec = (dir + right.normalized * avoidanceStrength).normalized;
            if (cooldown > 0f) cooldown--;
        }

        // Rotate toward movement direction (sprite faces up)
        if (moveVec.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, moveVec);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }

        // Translate
        transform.position += moveVec * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.Value) < 0.3f)
            target = null;
    }
}
