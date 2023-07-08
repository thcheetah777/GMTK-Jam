using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    [HideInInspector] public int flagIndex;
    private Vector3 flag => flagIndex != -1 ? HordeController.Flags[flagIndex] : transform.position;

    [SerializeField] private float movementSpeed;

    private void Awake()
    {
        // Add this rat to the global horde.
        HordeController.Rats.Add(this);
        flagIndex = -1;
    }

    private void Update()
    {
        // Follow focused flag.
        transform.Translate((flag - transform.position).normalized * movementSpeed * Time.deltaTime);
    }
}
