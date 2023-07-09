using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAnimation : MonoBehaviour {
    [SerializeField] private Vector3 offsetPerSecond;
    void Start() {
        
    }

    void Update() {
        this.gameObject.transform.Rotate(offsetPerSecond);
    }
}
