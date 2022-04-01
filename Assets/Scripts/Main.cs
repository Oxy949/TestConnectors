using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Main class
/// </summary>
public class Main : MonoBehaviour
{
    [Header("Generator:")]
    [SerializeField] private float radius = 10;
    [SerializeField] private int count = 10;
    
    [Header("Prefabs:")]
    [SerializeField] private GameObject connectablePrefab;

    public float Radius => radius;

    /// <summary>
    /// Initialization
    /// </summary>
    private void Start()
    {
        SpawnPrefabs();
    }

    /// <summary>
    /// Spawns prefabs with random position
    /// </summary>
    private void SpawnPrefabs()
    {
        for (int i = 0; i < count; i++)
        {
            // Get random 2D position (X, Y only) in circle radius
            Vector2 randomCircleCoords = Random.insideUnitCircle * radius;

            Vector3 randomPosition = new Vector3()
            {
                x = randomCircleCoords.x,
                y = 0,
                z = randomCircleCoords.y,
            };

            // Spawn the prefab
            Instantiate(connectablePrefab, randomPosition, Quaternion.identity);
        }
    }
}
