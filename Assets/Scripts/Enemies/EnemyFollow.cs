using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [Header ("Tracking")]
    private GameObject destination;
    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(destination.transform.position);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerManager>().isInvincible == false)
            {
                other.gameObject.GetComponent<PlayerManager>().TakeDamage(10);
                Debug.Log("Dealt damage to player");
            }
        }
    }
}
