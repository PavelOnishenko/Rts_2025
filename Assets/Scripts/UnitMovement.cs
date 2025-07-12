using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float avoidanceStrength = 1f;
    [SerializeField] float obstacleRayLength = 1f;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float initialCooldown = 50f;

    private float cooldown = 0;

    Vector3? target;

    public void MoveTo(Vector3 position)
    {
        target = position;
    }

    void Update()
    {
        if (!target.HasValue) return;

        //Debug.DrawLine(transform.position, target.Value, Color.green);
        var dir = (target.Value - transform.position).normalized;
        var nextStep = transform.position + dir * moveSpeed * Time.deltaTime;
        var hit = Physics2D.Raycast(transform.position, dir, obstacleRayLength, obstacleMask);

        //Debug.Log($"transform.position: [{transform.position}]; dir: [{dir}]; obstacleRayLength: [{obstacleRayLength}]; " +
        //    $"hit.collider is present: [{hit.collider == null}]; cooldown: [{cooldown}].");
        Debug.DrawRay(
            transform.position, dir * obstacleRayLength, hit.collider != null && hit.collider.gameObject.name != "Unit" && cooldown == 0 ? Color.red : Color.blue);

        if (hit.collider != null && hit.collider.gameObject.name != "Unit" || cooldown > 0)
        {
            if (hit.collider != null)
                cooldown = initialCooldown;
            var right = Vector3.Cross(dir, Vector3.forward); // perpendicular
            Debug.DrawLine(transform.position, transform.position + right, Color.magenta);
            nextStep = transform.position + (dir + right.normalized * avoidanceStrength).normalized * moveSpeed * Time.deltaTime;
            if(cooldown > 0)
                cooldown--;
        }
        transform.position = nextStep;
        if (Vector3.Distance(transform.position, target.Value) < 0.3f)
            target = null;
    }
}
