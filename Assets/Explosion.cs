using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float delay = 2f;
    float countdown;

    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            // Destroys partifle effect after delay
            Destroy(gameObject);
        }
    }
}
