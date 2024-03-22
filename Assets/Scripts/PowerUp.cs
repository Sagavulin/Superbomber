using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    enum PowerUps
    {
        MaxBombs,
        Range,
        Speed
    };

    [SerializeField] PowerUps powerUpType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Used with bomb powerup
            if (powerUpType == PowerUps.MaxBombs)
            {
                FindObjectOfType<GameManager>().IncreaseMaxBombs();
                Destroy(gameObject);
            }
            // Used with range powerup
            else if (powerUpType == PowerUps.Range)
            {
                FindObjectOfType<GameManager>().IncreaseExplodeRange();
                Destroy(gameObject);
            }
            // Used with speed powerup
            else if (powerUpType == PowerUps.Speed)
            {
                Debug.Log("Pickup Speed Powerup");
            }
        }
    }
}
