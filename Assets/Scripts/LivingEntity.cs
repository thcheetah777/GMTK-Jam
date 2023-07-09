using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class LivingEntity : PausableMonoBehaviour
{
    [Header("Living Entity Properties")]
    [SerializeField] protected int maxHealth;
    protected int health;
    [SerializeField] private float trapInvinicibilityTime;
    private new CircleCollider2D collider;

    protected virtual void Awake()
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
                if (health <= 0)
                {
                    // OnDeath();
                    break;
                }
                yield return new WaitForSeconds(trapInvinicibilityTime);
            }
            else
                yield return null;
    }

    // abstract void OnDeath();
}
