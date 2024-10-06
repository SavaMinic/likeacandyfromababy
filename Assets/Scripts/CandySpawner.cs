using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    [SerializeField] private GameObject candyPrefab;
    [SerializeField] private int initialCandyCount = 10;

    [SerializeField] private float areaDistance = 7f;
    [SerializeField] private float minHeight = 0.5f;
    [SerializeField] private float maxHeight = 1.2f;

    private void Start()
    {
        // one is always at 0,0,0
        Instantiate(candyPrefab, Vector3.up, Random.rotation, transform);
        SpawnCandy(initialCandyCount - 1);
    }

    public void SpawnCandy(int count)
    {
        // others are random
        for (var i = 0; i < count; i++)
        {
            var spawnedCandy = Instantiate(candyPrefab, transform);
            spawnedCandy.transform.position = new Vector3(
                Random.Range(-areaDistance, areaDistance),
                Random.Range(minHeight, maxHeight),
                Random.Range(-areaDistance, areaDistance)
            );
            spawnedCandy.transform.rotation = Random.rotation;
        }
    }
}