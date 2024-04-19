using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    // Array of all powerup type prefabs
    [SerializeField] private GameObject[] powerUpPrefabs;

    // Method is called each time a destructible block is destroyed
    // Will check to see if a random powerup should be spawned
    public void BlockDestroyed(Vector3 pos)
    {
        if (Random.value < 0.75f)
        {
            Instantiate(powerUpPrefabs[Random.Range(0,powerUpPrefabs.Length)], pos, Quaternion.identity);
        }
    }
}
