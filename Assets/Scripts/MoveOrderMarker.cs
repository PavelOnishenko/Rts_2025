using UnityEngine;

public class MoveOrderMarker : MonoBehaviour
{
    [SerializeField] float lifetime = 1.5f;

    void Start() => Destroy(gameObject, lifetime);
}
