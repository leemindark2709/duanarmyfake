using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTableManager : MonoBehaviour
{
    public static PlayerTableManager Instance { get; private set; }
    public List<Transform> playerPrefabs; // List of player prefabs
    private Queue<Transform> playerQueue = new Queue<Transform>(); // Queue of spawned players

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
        HideAll();
        LoadPlayer();
    }

    public virtual void LoadPlayer()
    {
        // Ensure playerPrefabs is initialized
        if (playerPrefabs == null)
        {
            playerPrefabs = new List<Transform>();
        }

        foreach (Transform player in transform)
        {
            playerPrefabs.Add(player);
        }
    }

    public virtual Transform Spawn(string playerName, Vector3 spawnPosition)
    {
        Transform playerPrefab = GetPlayerByName(playerName);
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not found: " + playerName);
            return null;
        }

        Transform newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        playerQueue.Enqueue(newPlayer); // Add the spawned player to the queue
        return newPlayer;
    }

    public virtual Transform GetPlayerByName(string playerName)
    {
        foreach (Transform player in playerPrefabs)
        {
            if (player.name == playerName)
            {
                return player;
            }
        }
        return null;
    }

    public Transform GetFirstPlayerFromQueue()
    {
        if (playerQueue.Count == 0)
        {
            Debug.LogWarning("Player queue is empty.");
            return null;
        }

        return playerQueue.Peek(); // Return the first player in the queue without removing it
    }

    public void RotateFirstPlayerToEndOfQueue()
    {
        if (playerQueue.Count == 0)
        {
            Debug.LogWarning("Player queue is empty.");
            return;
        }

        Transform firstPlayer = playerQueue.Dequeue(); // Remove the first player from the queue
        playerQueue.Enqueue(firstPlayer); // Add the first player to the end of the queue
    }
    protected virtual void HideAll()
    {
        foreach (Transform bullet in this.playerPrefabs)
        {
            bullet.gameObject.SetActive(false);
        }
    }
}
