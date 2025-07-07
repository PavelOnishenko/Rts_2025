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

        Vector3 dir = (target.Value - transform.position).normalized;
        Debug.Log($"dir = [{dir}].");
        Vector3 nextStep = transform.position + dir * moveSpeed * Time.deltaTime;
        Debug.Log($"nextStep = [{nextStep}].");

        // Raycast forward to detect obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, obstacleRayLength, obstacleMask);
        if (hit.collider != null || cooldown > 0)
        {
            Debug.Log($"IN IF");
            if (hit.collider != null)
            {
                cooldown = 200;
                Debug.Log($"cooldown = [{cooldown}].");
            }
            // Obstacle ahead — steer sideways
            Vector3 right = Vector3.Cross(dir, Vector3.forward); // perpendicular
            Debug.Log($"right = [{right}].");
            nextStep = transform.position + (dir + right * avoidanceStrength).normalized * moveSpeed * Time.deltaTime;
            Debug.Log($"IN IF nextStep = [{nextStep}].");
            if(cooldown > 0) 
                cooldown--;
            Debug.Log($"cooldown decreased = [{cooldown}].");
        }
        Debug.DrawRay(transform.position, dir * obstacleRayLength, hit.collider == null && cooldown == 0 ? Color.blue : Color.red);

        transform.position = nextStep;

        if (Vector3.Distance(transform.position, target.Value) < 0.1f)
        {
            target = null;
            Debug.Log($"Target is reached.");
        }
    }
}
