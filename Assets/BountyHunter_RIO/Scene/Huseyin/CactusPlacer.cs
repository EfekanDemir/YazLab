using UnityEngine;

public class CactusPlacer : MonoBehaviour
{   private int countPerPrefab = 30;
    private Vector2 mapSize = new Vector2(660, 660);
    public GameObject[] cactusPrefabs;  
    [ContextMenu("Kaktüsleri Yerleştir")]
    public void PlaceCacti()
    { 
        foreach (GameObject cactusPrefab in cactusPrefabs)
        {
            for (int i = 0; i < countPerPrefab; i++)
            {
                float randomX = Random.Range(-mapSize.x / 2, mapSize.x / 2);
                float randomZ = Random.Range(-mapSize.y / 2, mapSize.y / 2);
                Vector3 spawnPosition = new Vector3(
                    transform.position.x + randomX, 
                    transform.position.y + 100f, 
                    transform.position.z + randomZ
                );

                RaycastHit hit;
                if (Physics.Raycast(spawnPosition, Vector3.down, out hit, 200f))
                {
                    GameObject cactus = Instantiate(cactusPrefab, hit.point, Quaternion.identity);
                }
            }
        }
    }
}