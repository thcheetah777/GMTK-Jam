using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour {
    [SerializeField] private GameObject effect;
    private void OnDestroy() {
        Instantiate(effect, transform.position, effect.transform.rotation);
    }
}