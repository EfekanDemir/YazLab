
using UnityEngine;

public class HousePlacer : MonoBehaviour
{
    [Header("Yerleştirme Ayarları")]
    public int numberOfHouses = 10;
    public GameObject[] housePrefabs;

    [Header("Harita Alanı")]
    public Vector2 mapSize = new Vector2(500, 500);
    [Tooltip("Haritanın kenarlarından ne kadar boşluk bırakılacağı.")]
    public float borderPadding = 20f;

    [Header("Zemin Kontrolü")]
    [Tooltip("Bir evin yerleşebileceği maksimum zemin eğim açısı.")]
    public float maxGroundAngle = 10f;

    [Tooltip("Uygun bir yer bulmak için deneme sayısı.")]
    public int placementAttempts = 100;

    private Vector3 centerPosition;

    void Awake()
    {
        centerPosition = transform.position;
    }

    [ContextMenu("Evleri Yerleştir")]
    public void PlaceHouses()
    {
        if (housePrefabs == null || housePrefabs.Length == 0)
        {
            Debug.LogError("Ev Prefab'ları atanmamış! Lütfen Inspector'dan atama yapın.");
            return;
        }

        // Önceki evleri temizle
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        int placedCount = 0;
        for (int i = 0; i < numberOfHouses; i++)
        {
            bool spotFound = false;
            for (int attempt = 0; attempt < placementAttempts; attempt++)
            {
                // Kenar boşluklarını hesaba katarak rastgele bir konum seç
                float minX = (-mapSize.x / 2) + borderPadding;
                float maxX = (mapSize.x / 2) - borderPadding;
                float minZ = (-mapSize.y / 2) + borderPadding;
                float maxZ = (mapSize.y / 2) - borderPadding;

                float randomX = Random.Range(minX, maxX);
                float randomZ = Random.Range(minZ, maxZ);
                Vector3 spawnPosition = new Vector3(centerPosition.x + randomX, centerPosition.y + 100f, centerPosition.z + randomZ);

                RaycastHit hit;
                if (Physics.Raycast(spawnPosition, Vector3.down, out hit, 200f))
                {
                    // Zemin eğimini kontrol et
                    float groundAngle = Vector3.Angle(hit.normal, Vector3.up);
                    if (groundAngle <= maxGroundAngle)
                    {
                        // Zemin yeterince düz, evi yerleştir
                        GameObject housePrefab = housePrefabs[Random.Range(0, housePrefabs.Length)];
                        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * housePrefab.transform.rotation;
                        GameObject house = Instantiate(housePrefab, hit.point, rotation);
                        house.transform.parent = this.transform;
                        placedCount++;
                        spotFound = true;
                        break; // Bu ev için denemeyi bitir, sonraki eve geç
                    }
                }
            }

            if (!spotFound)
            {
                Debug.LogWarning("Ev " + (i + 1) + " için yeterince düz bir alan bulunamadı.");
            }
        }
        Debug.Log(placedCount + " adet ev başarıyla yerleştirildi.");
    }
}
