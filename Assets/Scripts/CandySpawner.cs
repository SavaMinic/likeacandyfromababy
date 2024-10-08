using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    [SerializeField] private GameObject candyPrefab;

    [SerializeField] private float areaDistance = 7f;
    [SerializeField] private float minHeight = 0.5f;
    [SerializeField] private float maxHeight = 1.2f;

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