using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float avoidanceStrength = 10f;
    [SerializeField] float obstacleRayLength = 2.8f;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float initialCooldown = 100f;
    [SerializeField] float turnSpeed = 720f; // degrees per second

    float cooldown = 0f;
    Vector3? target;

    private void Awake() => Physics2D.queriesStartInColliders = false;

    public void MoveTo(Vector3 position) => target = position;

    void Update()
    {
        if (!target.HasValue) 
            return;

        var dir = (target.Value - transform.position).normalized;
        var moveVec = dir;
        var hit = Physics2D.Raycast(transform.position, dir, obstacleRayLength, obstacleMask);
        if (hit.collider != null && hit.collider.gameObject != gameObject || cooldown > 0f)
        {
            if (hit.collider != null) 
                cooldown = initialCooldown;
            var right = Vector3.Cross(dir, Vector3.forward);
            moveVec = (dir + right.normalized * avoidanceStrength).normalized;
            if (cooldown > 0f) 
                cooldown--;
        }

        if (moveVec.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, moveVec);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }

        transform.position += moveVec * moveSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.Value) < 0.3f)
            target = null;
    }
}
