using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableMonoBehaviour : MonoBehaviour
{
    private void Update()
    {
        if (!GameManager.GlobalPause)
            PausableUpdate();
        else
            UnpausedUpdate();
    }

    public virtual void PausableUpdate() { }
    public virtual void UnpausedUpdate() { }
}
