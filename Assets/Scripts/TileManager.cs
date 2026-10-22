using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Tile Ayarları")]
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private float zSpawn;

    [SerializeField, Min(0.01f)]
    private float tileLength = 19f;

    [SerializeField, Min(1)]
    private int numberOfTiles = 8;

    [SerializeField]
    private float deleteDistanceBehindPlayer = 40f;

    [Header("Bağlantılar")]
    [SerializeField]
    private Transform playerTransform;

    private readonly List<GameObject> activeTiles =
        new List<GameObject>();

    private void Start()
    {
        if (!ValidateSettings())
        {
            enabled = false;
            return;
        }

        for (int i = 0; i < numberOfTiles; i++)
        {
            int tileIndex = i == 0
                ? 0
                : Random.Range(
                    0,
                    tilePrefabs.Length
                );

            SpawnTile(tileIndex);
        }
    }

    private void Update()
    {
        if (playerTransform == null)
            return;

        float playerProgress = Vector3.Dot(
            playerTransform.position -
            transform.position,
            transform.forward
        );

        while (
            playerProgress -
            deleteDistanceBehindPlayer >
            zSpawn -
            numberOfTiles * tileLength
        )
        {
            SpawnTile(
                Random.Range(
                    0,
                    tilePrefabs.Length
                )
            );

            DeleteOldestTile();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        if (tilePrefabs == null ||
            tilePrefabs.Length == 0)
        {
            return;
        }

        if (tileIndex < 0 ||
            tileIndex >= tilePrefabs.Length)
        {
            Debug.LogWarning(
                "TileManager: Geçersiz tile indexi: " +
                tileIndex,
                this
            );

            return;
        }

        GameObject prefab =
            tilePrefabs[tileIndex];

        if (prefab == null)
        {
            Debug.LogWarning(
                "TileManager: " +
                tileIndex +
                " indexindeki prefab boş.",
                this
            );

            return;
        }

        Vector3 spawnPosition =
            transform.position +
            transform.forward * zSpawn;

        GameObject spawnedTile = Instantiate(
            prefab,
            spawnPosition,
            transform.rotation
        );

        activeTiles.Add(spawnedTile);

        zSpawn += tileLength;
    }

    private void DeleteOldestTile()
    {
        if (activeTiles.Count == 0)
            return;

        Destroy(activeTiles[0]);

        activeTiles.RemoveAt(0);
    }

    private bool ValidateSettings()
    {
        if (playerTransform == null)
        {
            Debug.LogError(
                "TileManager: Player Transform atanmamış.",
                this
            );

            return false;
        }

        if (tilePrefabs == null ||
            tilePrefabs.Length == 0)
        {
            Debug.LogError(
                "TileManager: Tile prefab listesi boş.",
                this
            );

            return false;
        }

        if (tileLength <= 0f)
        {
            Debug.LogError(
                "TileManager: Tile Length sıfırdan büyük olmalı.",
                this
            );

            return false;
        }

        return true;
    }
}