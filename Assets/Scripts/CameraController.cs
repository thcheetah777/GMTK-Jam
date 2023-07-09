using UnityEngine;

public class CameraController : PausableMonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float forwardMoveSpeed;
    [SerializeField] private float sidewaysMoveSpeed;
    [SerializeField] private float leadingFactor;

    private Vector3 targetPosition;
    private Vector3 oldTargetPosition;

    private void Awake()
    {
        SetTargetPosition();
        oldTargetPosition = targetPosition;
    }

    public override void PausableUpdate()
    {
        SetTargetPosition();
        Vector3 targetDistance = targetPosition - oldTargetPosition;

        float forwardFactor = Mathf.Abs(1 - (Vector2.Angle(transform.forward, targetDistance) / 90));
        transform.position +=
            forwardMoveSpeed * forwardFactor * (targetPosition - oldTargetPosition) +
            sidewaysMoveSpeed * (1 - forwardFactor) * (Vector3)Vector2.Perpendicular(targetDistance);

        VisibleAABB();

        transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.back);

        oldTargetPosition = targetPosition;
    }

    private void SetTargetPosition()
    {
        targetPosition = Vector2.Lerp(HordeController.MeanRatPosition(), HordeController.Instance.cheeseTransform.position, leadingFactor);
    }

    private Rect VisibleAABB()
    {
        Rect _out = new Rect(HordeController.Instance.cheeseTransform.position, Vector2.zero);
        foreach (Rat rat in HordeController.Rats)
        {
            _out.min = Vector2.Min(_out.min, rat.transform.position);
            _out.max = Vector2.Max(_out.max, rat.transform.position);
        }
        _out.position += _out.size / 2;
        return _out;
    }
}
