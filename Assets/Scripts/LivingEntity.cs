using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    [Header("Living Entity Properties")]
    [SerializeField] protected int maxHealth;
    protected int health;
    [SerializeField] private float trapInvinicibilityTime;
    private new CircleCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        StartCoroutine(TrapCheck());
    }

    private IEnumerator TrapCheck()
    {
        while (true)
            if (Trap.Check(collider))
                yield return new WaitForSeconds(trapInvinicibilityTime);
            else
                yield return null;
    }
}
