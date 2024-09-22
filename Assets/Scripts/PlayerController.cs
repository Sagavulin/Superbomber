using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    float moveSpeed = 0;

    private Rigidbody m_Rigidbody;

    [SerializeField] private GameObject bombPrefab;

    private GameManager m_GameManager;

    private int maxBombs = 1;
    private int currentBombsPlaced = 0;

    private bool hasControl = true;
    [SerializeField] private float destroyTime = 2.0f;

    private bool isPaused = false;
    private bool isDead = false;

    [SerializeField] LayerMask whatAreBombLayers;

    private Animator m_Animator;
    private AudioSource m_AudioSource;

    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip powerupPickup;
    
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_GameManager = FindObjectOfType<GameManager>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl && !isPaused)
        {
            Movement();
			Rotation();
            UpdateAnimator();
            PlaceBomb();    
        }
    }

    private void Rotation()
    {
	    if (m_Rigidbody.velocity != Vector3.zero)
	    {
		    transform.forward = m_Rigidbody.velocity;
	    }
    }
    
    private void Movement()
    {
        Vector3 newVelocity = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            newVelocity += new Vector3(0f, 0f, moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newVelocity += new Vector3(0f, 0f, -moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newVelocity += new Vector3(-moveSpeed, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newVelocity += new Vector3(moveSpeed, 0f, 0f);
        }

        m_Rigidbody.velocity = newVelocity;
    }

    private void PlaceBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentBombsPlaced < maxBombs)
        {
            Vector3 center = new Vector3(Mathf.Round(transform.position.x), 0f, Mathf.Round(transform.position.z));

            // Create an overlap sphere where the new bomb will be placed to check if there's already a bomb there
            Collider[] hitColliders = Physics.OverlapSphere(center, 0.5f, whatAreBombLayers);
            if (hitColliders.Length > 0)
            {
                return;
            }
            
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb.transform.position = center;
            currentBombsPlaced++;
            m_GameManager.UpdateBombsText();
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            
            // Play death sound
            m_AudioSource.PlayOneShot(playerDeathSound, 1f);
            
            // Player has died and no control
            hasControl = false;
        
            // Removes player velocity
            m_Rigidbody.velocity = Vector3.zero;

            m_Rigidbody.isKinematic = true;
            
            // Destroy the player a set delay time
            Destroy(gameObject, destroyTime);
        
            // Tell the GameManager that the player has died
            m_GameManager.PlayerDied();

            //Play death animation
            m_Animator.SetBool("isDead", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Player hit the enemy and has died
            Die();
        }
    }

    public void BombExploded()
    {
        currentBombsPlaced--;
    }

    public float GetDestroyDelayTime()
    {
        return destroyTime;
    }

    // Called when the player is spawned and sets its starting values
    public void InitializePlayer(int bombs, float speed)
    {
        maxBombs = bombs;
        moveSpeed = speed;
    }

    public void SetPaused(bool state)
    {
        isPaused = state;
    }

    private void UpdateAnimator()
    {
	    // If player has no velocity play idle animation
	    if (m_Rigidbody.velocity == Vector3.zero)
	    {
		    m_Animator.SetBool("isWalking", false);
	    }
	    else
	    {
		    m_Animator.SetBool("isWalking", true);
	    }
    }

    public void PlayVictory()
    {
	    m_Animator.SetBool("isVictory", true);
	    hasControl = false;
    }

    public void PlayPowerupPickUpAudio()
    {
	    m_AudioSource.PlayOneShot(powerupPickup, 1f);
    }
}
