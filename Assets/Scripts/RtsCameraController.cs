using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float edgeSize = 10f;

    void Update()
    {
        Vector3 move = Vector3.zero;
        Vector2 mouse = Input.mousePosition;

        if (mouse.x < edgeSize || Input.GetKey(KeyCode.A)) move.x -= 1;
        if (mouse.x > Screen.width - edgeSize || Input.GetKey(KeyCode.D)) move.x += 1;
        if (mouse.y < edgeSize || Input.GetKey(KeyCode.S)) move.y -= 1;
        if (mouse.y > Screen.height - edgeSize || Input.GetKey(KeyCode.W)) move.y += 1;

        transform.position += move.normalized * moveSpeed * Time.deltaTime;
    }
}