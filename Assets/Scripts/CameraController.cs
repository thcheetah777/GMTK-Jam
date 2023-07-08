using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.back);
    }
}
