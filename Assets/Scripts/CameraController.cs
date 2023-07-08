using UnityEngine;

public class CameraController : PausableMonoBehaviour
{
    [SerializeField] private Transform target;

    public override void PausableUpdate()
    {
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.back);
    }
}
