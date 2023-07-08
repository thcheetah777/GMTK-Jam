using UnityEngine;

public class Rat : LivingEntity
{
    [HideInInspector] public int flagIndex;
    private Vector3 flag => flagIndex != -1 ? HordeController.GetFlag(flagIndex) : transform.position;

    [Header("Rat Properties")]
    [SerializeField] private float movementSpeed;

    protected override void Awake()
    {
        base.Awake();

        // Add this rat to the global horde.
        HordeController.AddRat(this);
        flagIndex = -1;
    }

    public override void PausableUpdate()
    {
        // Follow focused flag.
        transform.Translate((flag - transform.position).normalized * movementSpeed * Time.deltaTime);
    }

    protected override void OnDeath()
    {
        HordeController.RemoveRat(this);
        Destroy(gameObject);
    }
}
