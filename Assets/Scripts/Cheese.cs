using UnityEngine;

enum CheeseState
{
    Far,
    Medium,
    Close
}

public class Cheese : MonoBehaviour
{

    [SerializeField] private LayerMask ratLayer;

    [Header("State")]

    [SerializeField] private float _farRange;
    [SerializeField] private float _mediumRange;
    [SerializeField] private float _closeRange;

    [SerializeField] private float _farSpeed;
    [SerializeField] private float _mediumSpeed;
    [SerializeField] private float _closeSpeed;

    private CheeseState _state;

    void Update() {
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
        transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);
    }

}
