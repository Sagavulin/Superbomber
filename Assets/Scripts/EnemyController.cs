using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] target;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float enemyDelayMin = 0.25f;
    [SerializeField] private float enemyDelayMax = 3f;
    [SerializeField] private float enemyHitBombDelayMin = 0.5f;
    [SerializeField] private float enemyHitBombDelayMax = 3f;
    [SerializeField] private int scoreValue = 50;
    
    private Rigidbody m_Rigidbody;

    private bool isMoving = true;
    private bool movingForward = true;
    private int wayPointDestination = 0;

    private bool isDead = false;

    private Animator m_Animator;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        
        if (target.Length == 0)
        {
            isMoving = false;
            Debug.LogWarning("Enemy " + gameObject.name + "has no waypoints!");
        }
    }

    private void Update()
    {
	    UpdateAnimator();
    }

    void FixedUpdate()
    {
	    if (isDead) { return; }
	    
	    if (isMoving)
        {
            m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, target[wayPointDestination].position, Time.deltaTime * moveSpeed));
            transform.LookAt(target[wayPointDestination].position);
            if (Vector3.Distance(transform.position, target[wayPointDestination].position) < 0.1f)
            {
                isMoving = false;
                if (movingForward)
                {
                    if (wayPointDestination >= target.Length - 1)
                    {
                        movingForward = false;
                        Invoke("DecreaseDestination", Random.Range(enemyDelayMin, enemyDelayMax));
                    }
                    // If enemy is moving forward AND has not reached the last waypoint in the array
                    else
                    {
                        Invoke("IncreaseDestination", Random.Range(enemyDelayMin, enemyDelayMax));
                    }
                }
                // Enemy is moving backwards
                else
                {
                    if (wayPointDestination <= 0)
                    {
                        movingForward = true;
                        Invoke("IncreaseDestination", Random.Range(enemyDelayMin, enemyDelayMax));
                    }
                    // If enemy is moving backward AND has not reached the first waypoint in the array
                    else
                    {
                        Invoke("DecreaseDestination", Random.Range(enemyDelayMin, enemyDelayMax));
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //isMoving = false;
        }

        if (collision.gameObject.tag == "Bomb")
        {
            isMoving = false;
            if (movingForward)
            {
                Invoke("DecreaseDestination", Random.Range(enemyHitBombDelayMin, enemyHitBombDelayMax));
            }
            else
            {
                Invoke("IncreaseDestination", Random.Range(enemyHitBombDelayMin, enemyHitBombDelayMax));
            }
            movingForward = !movingForward;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bomb")
        {
            isMoving = false;
            if (movingForward)
            {
                Invoke("DecreaseDestination", Random.Range(enemyHitBombDelayMin, enemyHitBombDelayMax));
            }
            else
            {
                Invoke("IncreaseDestination", Random.Range(enemyHitBombDelayMin, enemyHitBombDelayMax));
            }
            movingForward = !movingForward;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
	    if (collision.gameObject.tag == "Bomb")
	    {
		    isMoving = false;
	    }
    }

    private void OnTriggerStay(Collider other)
    {
	    if (other.gameObject.tag == "Bomb")
	    {
		    isMoving = false;
	    }
    }

    private void OnCollisionExit(Collision collision)
    {
	    if (collision.gameObject.tag == "Bomb")
	    {
		    isMoving = true;
	    }
    }
    
    private void OnTriggerExit(Collider other)
    {
	    if (other.gameObject.tag == "Bomb")
	    {
		    isMoving = true;
	    }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            GameManager myGameManager = FindObjectOfType<GameManager>();
            myGameManager.UpdateScore(scoreValue);
            myGameManager.EnemyHasDied();
            Destroy(gameObject, 3.0f);
            GetComponent<Collider>().enabled = false;
            m_Animator.SetBool("isDead", true);
        }
        
    }

    private void IncreaseDestination()
    {
        if (wayPointDestination + 1 < target.Length)
        {
            wayPointDestination++;
        }
        isMoving = true;
    }

    private void DecreaseDestination()
    {
        if (wayPointDestination - 1 >= 0)
        {
            wayPointDestination--;
        }
        isMoving = true;
    }

    private void UpdateAnimator()
    {
	    m_Animator.SetBool("isWalking", isMoving);
    }
}
