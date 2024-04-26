using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] objectPrefabs;
    [SerializeField] int minObjectsPerRoom;
    [SerializeField] int maxObjectsPerRoom;

    private GameObject[] floorObjects;
    private Tilemap[] floorTilemaps;

    void Start()
    {
        StartCoroutine(DelayedStart(2)); // delayed start to ensure that all rooms are placed before this script runs
    }

    /* Method to spawn objects in every room by getting valid positions on the floor tilemap of each room */
    private void SpawnObjects()
    {
        foreach (Tilemap floorTilemap in floorTilemaps)
        {
            BoundsInt tilemapBounds = floorTilemap.cellBounds;
            int randomNumberOfObjects = Random.Range(minObjectsPerRoom, maxObjectsPerRoom + 1); // add 1 to make the range inclusive

            List<Vector3Int> validCellPositions = new();
            foreach (Vector3Int pos in tilemapBounds.allPositionsWithin)
            {
                if (floorTilemap.HasTile(pos) && !IsOnEdge(pos, tilemapBounds))
                {
                    validCellPositions.Add(pos);
                }
            }

            for (int i = 0; i < randomNumberOfObjects; i++)
            {
                int randomIndex = Random.Range(0, validCellPositions.Count);
                Vector3Int randomCell = validCellPositions[randomIndex];
                validCellPositions.RemoveAt(randomIndex);
                Vector3 spawnPosition = floorTilemap.CellToWorld(randomCell);

                GameObject randomObjectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
                Instantiate(randomObjectPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    IEnumerator DelayedStart(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        floorObjects = GameObject.FindGameObjectsWithTag("Floor");
        floorTilemaps = new Tilemap[floorObjects.Length];
        for (int i = 0; i < floorObjects.Length; i++)
        {
            floorTilemaps[i] = floorObjects[i].GetComponent<Tilemap>();
        }

        SpawnObjects();
    }

    /* Method to check if the item would spawn on the edges of the floor tilemap
     * This prevents items from spawning too close to or in walls */
    private bool IsOnEdge(Vector3Int position, BoundsInt bounds)
    {
        if ((position.x <= bounds.min.x + 1)
            || (position.x >= bounds.max.x - 1)
            || (position.y <= bounds.min.y + 1)
            || (position.y >= bounds.max.y - 1))
        {
            return true;
        }
        return false;
    }
}