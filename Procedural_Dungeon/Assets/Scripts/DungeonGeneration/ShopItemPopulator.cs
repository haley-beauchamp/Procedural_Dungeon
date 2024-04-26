using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemPopulator : MonoBehaviour
{
    [SerializeField] GameObject[] shopItemPrefabs;

    void Start()
    {
        PlaceItems();
    }

    private void PlaceItems()
    {
        List<Vector3> spawnPositions = CalculateItemSpawnPositions();

        foreach (Vector3 spawnPosition in spawnPositions)
        {
            int randomItemPrefabIndex = Random.Range(0, shopItemPrefabs.Length);
            GameObject shopItem = Instantiate(shopItemPrefabs[randomItemPrefabIndex], spawnPosition, Quaternion.identity);
            shopItem.GetComponent<Item>().isShopItem = true;
        }
    }

    private List<Vector3> CalculateItemSpawnPositions()
    {
        List<Vector3> spawnPositions = new();
        SpriteRenderer[] spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spawnPositions.Add(spriteRenderer.bounds.center);
        }

        return spawnPositions;
    }
}