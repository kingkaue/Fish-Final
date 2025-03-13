using UnityEngine;

public class CoralSpawner : MonoBehaviour
{
    public GameObject coralPrefab; // Reference to the coral prefab
    public int numberOfCorals = 10; // Number of corals to spawn
    public Vector2 spawnArea = new Vector2(10, 10); // Area to spawn corals (X and Z dimensions)
    public float yPadding = 0.01f; // Small padding to ensure corals spawn on top of the plane
    public Transform targetPlane; // Assign the target plane in the Inspector

    private void Start()
    {
        if (targetPlane == null)
        {
            Debug.LogError("Target plane is not assigned. Please assign a plane in the Inspector.");
            return;
        }

        SpawnCorals();
    }

    private void SpawnCorals()
    {
        for (int i = 0; i < numberOfCorals; i++)
        {
            // Generate a random position within the spawn area (in world space)
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2) + targetPlane.position.x,
                targetPlane.position.y + 100, // Start above the plane
                Random.Range(-spawnArea.y / 2, spawnArea.y / 2) + targetPlane.position.z
            );

            // Adjust the Y position to be on top of the assigned plane
            spawnPosition.y = GetSurfaceHeight(spawnPosition) + yPadding;

            // Instantiate the coral at the calculated position
            Instantiate(coralPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private float GetSurfaceHeight(Vector3 position)
    {
        // Raycast in world space to find the height of the plane at the given position
        Ray ray = new Ray(position, Vector3.down); // Shoot the ray downward
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // Debug the name of the object hit by the raycast
            Debug.Log("Raycast hit: " + hit.collider.name);

            // Return the Y position of the hit point in world space
            return hit.point.y;
        }

        Debug.LogWarning("Raycast did not hit anything. Defaulting to y = 0.");
        return 0; // Default to 0 if no hit (fallback)
    }
}