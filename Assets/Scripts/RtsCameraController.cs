using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) move.x -= 1;
        if (Input.GetKey(KeyCode.D)) move.x += 1;
        if (Input.GetKey(KeyCode.S)) move.y -= 1;
        if (Input.GetKey(KeyCode.W)) move.y += 1;

        transform.position += move.normalized * moveSpeed * Time.deltaTime;
    }
}