using System.Collections;
using UnityEngine;

public class CheeseWeapon : PausableMonoBehaviour
{

    [SerializeField] private float _weaponDelay;

    [Header("Shotgun")]

    [SerializeField] private Rigidbody _shotgunBulletPrefab;
    [SerializeField] private Transform _shotgunFirePoint;
    [SerializeField] private float _shotgunFireForce;

    IEnumerator Start() {
        while (true) {
            yield return new WaitForSeconds(_weaponDelay);
            if (!GameManager.GlobalPause)
                UseShotgun();
            else
                yield return null;
        }
    }

    private void UseShotgun() {
        // ! THIS IS BROKEN, THE SHOTGUN IS POINTING IN THE WRONG DIRECTION
        _shotgunFirePoint.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.identity * (HordeController.Rats[0].transform.position - _shotgunFirePoint.position));
        Rigidbody bullet = Instantiate(_shotgunBulletPrefab, _shotgunFirePoint.position, _shotgunFirePoint.rotation);
        bullet.AddRelativeForce(Vector3.forward * _shotgunFireForce, ForceMode.Impulse);
    }

    private void UseBomb() {
        // TODO: Add bombs if we have time (probably not, but we'll see)
    }

}
