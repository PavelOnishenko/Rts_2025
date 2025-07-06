using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    Vector3 target;
    bool hasTarget;

    public void MoveTo(Vector3 position)
    {
        target = position;
        hasTarget = true;
    }

    void Update()
    {
        if (!hasTarget) return;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.1f)
            hasTarget = false;
    }
}
