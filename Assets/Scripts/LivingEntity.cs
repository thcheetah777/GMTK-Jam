using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    [Header("Living Entity Properties")]
    [SerializeField] protected int maxHealth;
    protected int health;
    private CircleCollider2D collider;

    /*private void Update()
    {
        if ()
    }*/
}
