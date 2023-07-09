using UnityEngine;

public class Rat : LivingEntity
{
    [HideInInspector] public int flagIndex;

    [HideInInspector] public float movementSpeedTimeOffset;
    [HideInInspector] public float massTimeOffset;

    protected override void Awake()
    {
        base.Awake();

        // Add this rat to the global horde.
        HordeController.AddRat(this);
        flagIndex = -1;
    }

    public void Die()
    {
        AudioManager.I.PlayRandomSound(AudioManager.I.ratDeaths);
        HordeController.RemoveRat(this);
        Destroy(gameObject);
    }
}
