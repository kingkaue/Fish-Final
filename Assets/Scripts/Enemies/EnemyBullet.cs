using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 moveDirection;

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized; // Normalize so speed is consistent
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
