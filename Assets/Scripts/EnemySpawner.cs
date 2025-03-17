using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    public float maxenemies = 1000;
    private float playerlevel = 1;
    public GameObject enemyprefab;
    public GameObject enemyprefab2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private int randomint;
    void Start()
    {
        StartCoroutine(enemyspawner());
    }

    // Update is called once per frame
    void Update()
    {
        randomint = Random.Range(3, 7);
    }

    private IEnumerator enemyspawner()
    {
        yield return new WaitForSeconds(1f);
            if(playerlevel == 1)
            {
                for (int j = 0; j <= maxenemies; j++)
                {
                    yield return new WaitForSeconds(1f);
                    Instantiate(enemyprefab);
                    yield return new WaitForSeconds(2f);
                    if (randomint == 5)
                    {
                    yield return new WaitForSeconds(1f);
                    Instantiate(enemyprefab2);
                    yield return new WaitForSeconds(2f);
                    }
                }
            }
            
    }
}
