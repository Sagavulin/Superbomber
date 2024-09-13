using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Vector3 explodeDirection = Vector3.zero; // Vector3.zero is the same as "new Vector3(0,0,0)"
    private float explodeSpeed = 200f;
    private float explodeRange = 2f;
    private Vector3 explosionStartPosition;
    
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        explosionStartPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, explosionStartPosition) >= explodeRange)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        m_Rigidbody.velocity = explodeDirection * explodeSpeed * Time.deltaTime;
    }

    public void SetExplosion(Vector3 direction, float speed, float range)
    {
        explodeDirection = direction;
        explodeSpeed = speed;
        explodeRange = range;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Block")
        {
            Destroy(gameObject);
        }
        
        if (other.gameObject.tag == "Bomb")
        {
            other.gameObject.GetComponent<Bomb>().Explode();
            Destroy(gameObject);
        }
        
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Die();
        }
        
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().Die();
        }

        if (other.gameObject.tag == "Breakable")
        {
            FindObjectOfType<PowerUpSpawner>().BlockDestroyed(other.transform.position);
            
            // Play destroy block animation
            other.gameObject.GetComponent<Animator>().SetTrigger("isDestroyed");
            Destroy(other.gameObject, .5f);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "PowerUp")
        {
	        Destroy(other.gameObject);
	        Destroy(gameObject);
        }
    }
}
