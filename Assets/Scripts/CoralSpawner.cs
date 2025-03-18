using UnityEngine;
using System.Collections.Generic;

public class CoralSpawner : MonoBehaviour
{
    public GameObject coralPrefab; // Reference to the coral prefab
    public Transform player; // Reference to the player transform
    public Transform targetPlane; // Assign the target plane in the Inspector
    public float spawnRadius = 20f; // Radius around the player to spawn corals
    public float despawnRadius = 30f; // Radius beyond which corals are despawned
    public int maxCoralsPerChunk = 10; // Maximum number of corals per chunk
    public float yPadding = 0.01f; // Small padding to ensure corals spawn on top of the plane
    public float xpadding = 0.01f;

    private List<GameObject> activeCorals = new List<GameObject>(); // List of active corals
    private Vector2Int lastPlayerChunk; // Last chunk the player was in

    private void Start()
    {
        if (targetPlane == null || player == null)
        {
            Debug.LogError("Target plane or player is not assigned. Please assign them in the Inspector.");
            return;
        }

        lastPlayerChunk = GetChunkCoordinates(player.position);

        // Spawn initial corals multiple times to prepopulate the world
        int preSpawnCycles = 3; // Adjust this to control how much coral spawns before gameplay
        for (int i = 0; i < preSpawnCycles; i++)
        {
            SpawnCoralsAroundPlayer();
        }
    }


    private void Update()
    {
        // Check if the player has moved to a new chunk
        Vector2Int currentPlayerChunk = GetChunkCoordinates(player.position);
        if (currentPlayerChunk != lastPlayerChunk)
        {
            lastPlayerChunk = currentPlayerChunk;
            SpawnCoralsAroundPlayer();
            DespawnDistantCorals();
        }
    }

    private void SpawnCoralsAroundPlayer()
    {
        // Calculate the area around the player to spawn corals
        Vector3 playerPosition = player.position;
        for (int i = 0; i < maxCoralsPerChunk; i++)
        {
            // Generate a random position within the spawn radius
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
            playerPosition.x + randomOffset.x,
            targetPlane.position.y + 5f, // Start closer to the plane
            playerPosition.z + randomOffset.y
            );
            //x padding
            spawnPosition.x += xpadding;

            // Adjust the Y position to be on top of the assigned plane
            spawnPosition.y = GetSurfaceHeight(spawnPosition) + yPadding;
            // Instantiate the coral at the calculated position
            GameObject coral = Instantiate(coralPrefab, spawnPosition, Quaternion.identity);
            activeCorals.Add(coral);
        }
    }

    private void DespawnDistantCorals()
    {
        // Remove corals that are too far from the player
        for (int i = activeCorals.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(player.position, activeCorals[i].transform.position) > despawnRadius)
            {
                Destroy(activeCorals[i]);
                activeCorals.RemoveAt(i);
            }
        }
    }

    private float GetSurfaceHeight(Vector3 position)
    {
        Ray ray = new Ray(position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            return hit.point.y;
        }

        Debug.LogWarning("Raycast did not hit anything. Defaulting to plane height.");
        return targetPlane.position.y; // Fallback to plane height
    }


    private Vector2Int GetChunkCoordinates(Vector3 position)
    {
        // Convert world position to chunk coordinates
        int x = Mathf.FloorToInt(position.x / spawnRadius);
        int z = Mathf.FloorToInt(position.z / spawnRadius);
        return new Vector2Int(x, z);
    }
}