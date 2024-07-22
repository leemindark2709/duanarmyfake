using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    public List<Transform> mapPrefabs; // List of map prefabs
    private Queue<Transform> activeMaps = new Queue<Transform>(); // Queue of activated maps

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //SpawnMap();
    }

    public void LoadMaps()
    {
        // Ensure mapPrefabs is initialized
        if (mapPrefabs == null)
        {
            mapPrefabs = new List<Transform>();
        }

        foreach (Transform map in transform)
        {
            mapPrefabs.Add(map);
        }
    }

    public Transform SpawnMap(string mapName, Vector3 spawnPosition)
    {
        Transform mapPrefab = GetMapByName(mapName);
        if (mapPrefab == null)
        {
            Debug.LogError("Map prefab not found: " + mapName);
            return null;
        }

        Transform newMap = Instantiate(mapPrefab, spawnPosition, Quaternion.identity);
        activeMaps.Enqueue(newMap); // Add the spawned map to the queue of active maps
        return newMap;
    }

    public Transform GetMapByName(string mapName)
    {
        foreach (Transform map in mapPrefabs)
        {
            if (map.name == mapName)
            {
                return map;
            }
        }
        return null;
    }

    public Transform GetRandomMap()
    {
        if (mapPrefabs.Count == 0)
        {
            Debug.LogWarning("No map prefabs found.");
            return null;
        }

        int randomIndex = Random.Range(0, mapPrefabs.Count);
        return mapPrefabs[randomIndex];
    }



   public void HideAllMaps()
    {
        foreach(Transform map in transform)
        {
            map.gameObject.SetActive(false);
        }
    }

    public void ActivateRandomMap()
    {
        // Disable all active maps first
        foreach (Transform map in mapPrefabs)
        {
            map.gameObject.SetActive(false);
        }

        // Get a random map prefab
        Transform randomMapPrefab = GetRandomMap();
        if (randomMapPrefab == null)
        {
            Debug.LogError("Failed to get random map prefab.");
            return;
        }

        // Spawn the random map at position (0, 0, 0)
        Vector3 spawnPosition = Vector3.zero;
        Transform newMap = SpawnMap(randomMapPrefab.name, spawnPosition);

        // Activate the new map
        if (newMap != null)
        {
            newMap.gameObject.SetActive(true);
        }
    }
   public void SpawnMap()
    {
        DeleteLastSpawnedMap();
        LoadMaps();
        HideAllMaps();
        
        ActivateRandomMap();
    }

    public void DeleteLastSpawnedMap()
    {
        if (activeMaps.Count == 0)
        {
            Debug.LogWarning("No active maps to delete.");
            return;
        }

        Transform lastSpawnedMap = activeMaps.Dequeue(); // Remove the last spawned map from the queue
        Destroy(lastSpawnedMap.gameObject); // Destroy the map object
    }
}
