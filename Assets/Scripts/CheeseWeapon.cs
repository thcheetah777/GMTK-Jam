using System.Collections;
using UnityEngine;

public class CheeseWeapon : PausableMonoBehaviour
{

    [SerializeField] private float _weaponDelay;

    [Header("Shotgun")]

    [SerializeField] private GameObject _shotgunBulletPrefab;
    [SerializeField] private Transform _shotgunFirePoint;

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
        AudioManager.I.PlaySound(AudioManager.I.shotgun);
        Vector2 direction = HordeController.ClosestRatPosition(_shotgunFirePoint.position) - _shotgunFirePoint.position;
        _shotgunFirePoint.rotation = Quaternion.LookRotation(direction, Vector3.back);
        GameObject bullet = Instantiate(_shotgunBulletPrefab, (Vector2)_shotgunFirePoint.position, _shotgunBulletPrefab.transform.rotation);
    }

    private void UseBomb() {
        // TODO: Add bombs if we have time (probably not, but we'll see)
    }

}
