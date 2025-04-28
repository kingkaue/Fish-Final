using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float delay = 2f;
    float countdown;
    bool hasExploded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded)
        {
            Destroy(gameObject);
        }
    }
}
