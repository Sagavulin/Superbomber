using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private PlayerController player;
    
    [SerializeField] private float explosionDelay = 2f;
    private float explosionTimer = 0;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explodeSpeed = 200f;
    private int explodeRange = 1;

    [SerializeField] private AudioClip bombExplodeAudio;

    private bool hasExploded = false;
    [SerializeField] private GameObject bombModel;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        explodeRange = FindObjectOfType<GameManager>().GetExplodeRange();
    }

    void Update()
    {
        explosionTimer += Time.deltaTime;
        
        if (explosionTimer >= explosionDelay && !hasExploded)
        {
            Explode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<SphereCollider>().isTrigger = false;
        }
    }

    public void Explode()
    {
        GameObject explosionRight = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionRight.GetComponent<Explosion>().SetExplosion(Vector3.right, explodeSpeed, explodeRange);
        
        GameObject explosionLeft = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionLeft.GetComponent<Explosion>().SetExplosion(Vector3.left, explodeSpeed, explodeRange);
        
        GameObject explosionUp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionUp.GetComponent<Explosion>().SetExplosion(Vector3.forward, explodeSpeed, explodeRange);
        
        GameObject explosionDown = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionDown.GetComponent<Explosion>().SetExplosion(Vector3.back, explodeSpeed, explodeRange);

        player.BombExploded();

        GetComponent<AudioSource>().PlayOneShot(bombExplodeAudio);
        
        // Destroy collider on the bomb and turn off the bomb model
        Destroy(GetComponent<Collider>());
        bombModel.SetActive(false);

        Destroy(gameObject, 1.0f);
        
        hasExploded = true;
    }
}
