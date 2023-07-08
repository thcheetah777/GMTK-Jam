using System.Collections;
using UnityEngine;

public class LivingEntity : PausableMonoBehaviour
{
    [Header("Living Entity Properties")]
    [SerializeField] protected int maxHealth;
    protected int health;
    [SerializeField] private float trapInvinicibilityTime;
    private new CircleCollider2D collider;

    private void Awake()
    {
        health = maxHealth;

        collider = GetComponent<CircleCollider2D>();
        StartCoroutine(TrapCheck());
    }

    private IEnumerator TrapCheck()
    {
        while (true)
            if (Trap.Check(collider) && !GameManager.GlobalPause)
            {
                health -= Trap.Damage;
                yield return new WaitForSeconds(trapInvinicibilityTime);
            }
            else
                yield return null;
    }
}
