
using UnityEngine;

public class CactusPlacer : MonoBehaviour
{
    [Header("Ayarlar")]
    public int countPerPrefab = 30; // Her bir prefab'dan kaç tane yerleştirileceği
    public GameObject[] cactusPrefabs; // Birden fazla prefab için dizi

    [Header("Harita Alanı")]
    public Vector2 mapSize = new Vector2(660, 660); // 8x8'lik alanı kapsayacak şekilde güncellendi

    // Script'in eklendiği nesnenin pozisyonunu merkez alır.
    // Haritanızın merkezi (0,0,0) değilse bu nesneyi merkeze taşıyın.
    private Vector3 centerPosition;

    void Awake()
    {
        centerPosition = transform.position;
    }

    [ContextMenu("Kaktüsleri Yerleştir")]
    public void PlaceCacti()
    {
        if (cactusPrefabs == null || cactusPrefabs.Length == 0)
        {
            Debug.LogError("Kaktüs Prefab'ları atanmamış! Lütfen Inspector'dan atama yapın.");
            return;
        }

        // Önceki kaktüsleri temizle
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        int totalPlaced = 0;
        // Her bir kaktüs prefabı için döngü
        foreach (GameObject cactusPrefab in cactusPrefabs)
        {
            for (int i = 0; i < countPerPrefab; i++)
            {
                // Harita alanı içinde rastgele bir X ve Z konumu seç
                float randomX = Random.Range(-mapSize.x / 2, mapSize.x / 2);
                float randomZ = Random.Range(-mapSize.y / 2, mapSize.y / 2);
                Vector3 spawnPosition = new Vector3(centerPosition.x + randomX, centerPosition.y + 100f, centerPosition.z + randomZ);

                // Yüzeyi bulmak için yukarıdan aşağıya bir ışın gönder
                RaycastHit hit;
                if (Physics.Raycast(spawnPosition, Vector3.down, out hit, 200f))
                {
                    // Işın bir yüzeye çarptıysa, o noktada kaktüsü oluştur
                    GameObject cactus = Instantiate(cactusPrefab, hit.point, Quaternion.identity);
                    cactus.transform.parent = this.transform; // Kaktüsleri bu nesnenin altına topla
                    totalPlaced++;
                }
            }
        }
        Debug.Log(totalPlaced + " adet kaktüs başarıyla yerleştirildi.");
    }
}
