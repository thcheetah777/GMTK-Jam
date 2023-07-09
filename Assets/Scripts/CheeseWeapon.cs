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
        Vector3 direction = HordeController.ClosestRatPosition(_shotgunFirePoint.position) - _shotgunFirePoint.position;
        _shotgunFirePoint.rotation = Quaternion.LookRotation(direction - _shotgunFirePoint.position, Vector3.back);
        Rigidbody bullet = Instantiate(_shotgunBulletPrefab, _shotgunFirePoint.position, _shotgunFirePoint.rotation);
        bullet.AddForce(direction * _shotgunFireForce, ForceMode.Impulse);
    }

    private void UseBomb() {
        // TODO: Add bombs if we have time (probably not, but we'll see)
    }

}
