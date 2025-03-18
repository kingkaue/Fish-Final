using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float maxenemies = 1000;
    private float playerlevel = 1;
    public GameObject enemyprefab;
    public GameObject enemyprefab2;
    public Camera mainCamera;

    private int randomint;

    void Start()
    {
        StartCoroutine(enemyspawner());
    }

    void Update()
    {
        randomint = Random.Range(3, 7); // Update randomint every frame
    }

    private IEnumerator enemyspawner()
    {
        yield return new WaitForSeconds(1f); // Initial delay

        if (playerlevel == 1)
        {
            for (int j = 0; j <= maxenemies; j++)
            {
                // Recalculate the off-screen position for each enemy
                int side = Random.Range(0, 4);
                Vector3 offScreenViewportPosition = GetRandomOffScreenPosition(side);
                Vector3 offScreenWorldPosition = mainCamera.ViewportToWorldPoint(offScreenViewportPosition);

                // Spawn the first enemy
                Instantiate(enemyprefab, offScreenWorldPosition, Quaternion.identity);
                yield return new WaitForSeconds(2f); // Delay between spawns

                // Randomly spawn the second enemy
                if (randomint == 5)
                {
                    // Recalculate the off-screen position for the second enemy
                    side = Random.Range(0, 4);
                    offScreenViewportPosition = GetRandomOffScreenPosition(side);
                    offScreenWorldPosition = mainCamera.ViewportToWorldPoint(offScreenViewportPosition);

                    Instantiate(enemyprefab2, offScreenWorldPosition, Quaternion.identity);
                    yield return new WaitForSeconds(2f); // Delay between spawns
                }
            }
        }
    }

    Vector3 GetRandomOffScreenPosition(int side)
    {
        float x = 0f, y = 0f;

        switch (side)
        {
            case 0: // Left side
                x = Random.Range(-0.5f, -0.1f); // Random x outside the left edge
                y = Random.Range(0.1f, 0.9f);   // Random y within the screen height
                break;
            case 1: // Right side
                x = Random.Range(1.1f, 1.5f);   // Random x outside the right edge
                y = Random.Range(0.1f, 0.9f);  // Random y within the screen height
                break;
            case 2: // Top side
                x = Random.Range(0.1f, 0.9f);   // Random x within the screen width
                y = Random.Range(1.1f, 1.5f);   // Random y outside the top edge
                break;
            case 3: // Bottom side
                x = Random.Range(0.1f, 0.9f);   // Random x within the screen width
                y = Random.Range(-0.5f, -0.1f); // Random y outside the bottom edge
                break;
        }

        // Set z to the distance from the camera (adjust based on your game)
        float z = 50f;

        return new Vector3(x, y, z);
    }
}