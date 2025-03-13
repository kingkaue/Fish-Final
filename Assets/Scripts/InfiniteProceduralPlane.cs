using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class InfiniteProceduralPlane : MonoBehaviour
{
    public GameObject planeChunkPrefab; // Prefab for a single plane chunk (e.g., 10x10 units)
    public Transform player; // Reference to the player transform
    public int chunkSize = 10; // Size of each chunk (e.g., 10x10 units)
    public int renderDistance = 3; // Number of chunks to load around the player

    private Vector2Int currentPlayerChunk; // The chunk the player is currently in
    private Hashtable chunks = new Hashtable(); // Stores all active chunks

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned. Please assign the player in the Inspector.");
            return;
        }

        // Initialize the plane around the player
        UpdatePlane();
    }

    private void Update()
    {
        // Check if the player has moved to a new chunk
        Vector2Int playerChunk = GetChunkCoordinates(player.position);
        if (playerChunk != currentPlayerChunk)
        {
            currentPlayerChunk = playerChunk;
            UpdatePlane();
        }
    }

    private void UpdatePlane()
    {
        // Generate chunks within the render distance
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(currentPlayerChunk.x + x, currentPlayerChunk.y + z);

                // If the chunk doesn't exist, create it
                if (!chunks.ContainsKey(chunkCoord))
                {
                    Vector3 chunkPosition = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);
                    GameObject chunk = Instantiate(planeChunkPrefab, chunkPosition, Quaternion.identity, transform);
                    chunks.Add(chunkCoord, chunk);
                }
            }
        }

        // Remove chunks that are too far from the player
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (DictionaryEntry entry in chunks)
        {
            Vector2Int chunkCoord = (Vector2Int)entry.Key;
            if (Mathf.Abs(chunkCoord.x - currentPlayerChunk.x) > renderDistance ||
                Mathf.Abs(chunkCoord.y - currentPlayerChunk.y) > renderDistance)
            {
                Destroy((GameObject)entry.Value); // Destroy the chunk
                chunksToRemove.Add(chunkCoord); // Mark it for removal from the hashtable
            }
        }

        // Clean up the hashtable
        foreach (Vector2Int chunkCoord in chunksToRemove)
        {
            chunks.Remove(chunkCoord);
        }
    }

    private Vector2Int GetChunkCoordinates(Vector3 position)
    {
        // Convert world position to chunk coordinates
        int x = Mathf.FloorToInt(position.x / chunkSize);
        int z = Mathf.FloorToInt(position.z / chunkSize);
        return new Vector2Int(x, z);
    }
}