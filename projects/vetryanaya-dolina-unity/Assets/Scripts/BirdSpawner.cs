using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Route
    {
        public string routeName;
        public Transform[] waypoints;
    }

    [Header("Prefabs")]
    [SerializeField] private GameObject birdPrefab; 

    [Header("Routes")]
    [SerializeField] private Route[] routes;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDelay = 10f;
    [SerializeField] private float maxSpawnDelay = 30f;
    [SerializeField] private float birdSpeed = 8f;

    private void Start()
    {
        if (routes.Length > 0 && birdPrefab != null)
        {
            StartCoroutine(SpawnBirdsRoutine());
        }
    }

    private IEnumerator SpawnBirdsRoutine()
    {
        while (true)
        {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);

            int countToSpawn = Random.Range(1, 3); 

            for (int i = 0; i < countToSpawn; i++)
            {
                SpawnBird();
                yield return new WaitForSeconds(0.8f); 
            }
        }
    }

    private void SpawnBird()
    {
        int randomRouteIndex = Random.Range(0, routes.Length);
        Route selectedRoute = routes[randomRouteIndex];

        if (selectedRoute.waypoints.Length < 2) return;

        GameObject newBird = Instantiate(birdPrefab);

        BirdFlight birdFlight = newBird.GetComponent<BirdFlight>();
        if (birdFlight != null)
        {
            birdFlight.InitRoute(selectedRoute.waypoints, birdSpeed);
        }
    }
}
