using System.Collections.Generic;
using UnityEngine;

public class ForestSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject terrainPrefab;
    public float terrainSize = 100f;
    public int renderRadius = 1;

    private Dictionary<Vector2Int, GameObject> spawnedTiles = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int currentTile;

    private void Start()
    {
        currentTile = GetTileCoord(player.position);
        SpawnSurroundingTiles(currentTile);
    }

    private void Update()
    {
        Vector2Int playerTile = GetTileCoord(player.position);

        if (playerTile != currentTile)
        {
            currentTile = playerTile;
            SpawnSurroundingTiles(currentTile);
            DespawnDistantTiles(currentTile);
        }
    }

    private Vector2Int GetTileCoord(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / terrainSize);
        int z = Mathf.RoundToInt(position.z / terrainSize);
        return new Vector2Int(x, z);
    }

    private void SpawnTile(Vector2Int coord)
    {
        if (spawnedTiles.ContainsKey(coord)) return;

        Vector3 spawnPos = new Vector3(coord.x * terrainSize, 0f, coord.y * terrainSize);
        GameObject tile = Instantiate(terrainPrefab, spawnPos, Quaternion.identity);
        spawnedTiles.Add(coord, tile);
        tile.name = "Terrain" + coord.ToString();
    }

    private void SpawnSurroundingTiles(Vector2Int center)
    {
        for (int x = -renderRadius; x <= renderRadius; x++)
        {
            for (int z = -renderRadius; z <= renderRadius; z++)
            {
                Vector2Int offset = new Vector2Int(center.x + x, center.y + z);
                SpawnTile(offset);
            }
        }
    }

    private void DespawnDistantTiles(Vector2Int center)
    {
        List<Vector2Int> toRemove = new List<Vector2Int>();

        foreach (var pair in spawnedTiles)
        {
            Vector2Int coord = pair.Key;
            int dx = Mathf.Abs(coord.x - center.x);
            int dz = Mathf.Abs(coord.y - center.y);

            if (dx > renderRadius || dz > renderRadius)
            {
                Destroy(pair.Value);
                toRemove.Add(coord);
            }
        }

        foreach (var coord in toRemove)
        {
            spawnedTiles.Remove(coord);
        }
    }
}
