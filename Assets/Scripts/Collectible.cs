using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleEffect collectibleEffect;

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collectibleEffect.Apply(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
