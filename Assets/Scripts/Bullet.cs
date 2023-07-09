using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10;

    void Update() {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

}
