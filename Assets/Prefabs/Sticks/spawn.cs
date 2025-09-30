using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject[] stickPrefabs;
    public Terrain terrain;
    public int count = 100;
    public Vector2 areaSize = new Vector2(100, 100); // ширина x довжина зони
    public float offsetY = 0f; // на скільки підняти паличку над землею
    
    void Start()
    {
        Invoke("SpawnSticks", 1);
    }

    void SpawnSticks()
    {
        terrain = GameObject.Find("Terrain(0, 0)").transform.GetChild(0).gameObject.GetComponent<Terrain>();
        Vector3 terrainPos = terrain.transform.position;

        for (int i = 0; i < count; i++)
        {
            // Випадкова позиція в зоні
            float x = Random.Range(0, areaSize.x);
            float z = Random.Range(0, areaSize.y);

            // Перетворення у світові координати
            float worldX = terrainPos.x + x;
            float worldZ = terrainPos.z + z;

            // Отримання висоти террейну
            float y = terrain.SampleHeight(new Vector3(worldX, 0, worldZ)) + terrainPos.y;

            // Спавн об'єкта
            Vector3 position = new Vector3(worldX, y + offsetY, worldZ);
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            Instantiate(stickPrefabs[Random.Range(0, stickPrefabs.Length)], position, rotation);
        }
    }
}