using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float avoidanceStrength = 1f;
    [SerializeField] float obstacleRayLength = 1f;
    [SerializeField] LayerMask obstacleMask;

    private float cooldown = 0;

    Vector3? target;

    public void MoveTo(Vector3 position)
    {
        target = position;
    }

    void Update()
    {
        if (!target.HasValue) return;

        var dir = (target.Value - transform.position).normalized;
        var nextStep = transform.position + dir * moveSpeed * Time.deltaTime;
        var hit = Physics2D.Raycast(transform.position, dir, obstacleRayLength, obstacleMask);
        if (hit.collider != null || cooldown > 0)
        {
            if (hit.collider != null)
                cooldown = 200;
            var right = Vector3.Cross(dir, Vector3.forward); // perpendicular
            nextStep = transform.position + (dir + right * avoidanceStrength).normalized * moveSpeed * Time.deltaTime;
            if(cooldown > 0) 
                cooldown--;
        }
        transform.position = nextStep;
        if (Vector3.Distance(transform.position, target.Value) < 0.1f)
            target = null;
    }
}
