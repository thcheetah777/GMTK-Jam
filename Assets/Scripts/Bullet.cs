using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float range = 10;

    void Update() {
        Rat closestRat = null;
        foreach (var rat in HordeController.Rats)
        {
            if (closestRat == null)
            {
                closestRat = rat;
            } else {
                closestRat = Vector2.Distance(transform.position, rat.transform.position) < Vector2.Distance(transform.position, closestRat.transform.position) ? rat : closestRat;
            }
        }

        if (closestRat != null && Vector2.Distance(transform.position, closestRat.transform.position) < range)
        {
            closestRat.Die();
            Destroy(gameObject);
        }
    }

}
