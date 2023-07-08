using UnityEngine;

enum CheeseState
{
    Far,
    Medium,
    Close
}

public class Cheese : LivingEntity
{
    [Header("Cheese Properties")]
    [SerializeField] private LayerMask ratLayer;

    [Header("State")]

    [SerializeField] private float _farRange;
    [SerializeField] private float _mediumRange;
    [SerializeField] private float _closeRange;

    [SerializeField] private float _farSpeed;
    [SerializeField] private float _mediumSpeed;
    [SerializeField] private float _closeSpeed;

    private CheeseState _state;

    protected override void Awake() => base.Awake();

    public override void PausableUpdate() {
        _state = CheeseState.Close;

        float distanceFromRat = Vector2.Distance(transform.position, HordeController.Rats[0].transform.position);
        if (distanceFromRat >= _mediumRange) _state = CheeseState.Medium;
        if (distanceFromRat >= _farRange) _state = CheeseState.Far;

        switch (_state)
        {
            case CheeseState.Far: {
                Move(_farSpeed);
                break;
            }
            case CheeseState.Medium: {
                Move(_mediumSpeed);
                break;
            }
            case CheeseState.Close: {
                Move(_closeSpeed);
                break;
            }
        }
    }

    private void Move(float speed) {
        Vector2 direction = HordeController.Rats[0].transform.position;
        Vector3 targetPos = new Vector3(direction.x, direction.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    protected override void OnDeath()
    {

    }
}
