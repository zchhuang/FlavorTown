using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent class for enemy object.
public class Enemy : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    
    // Movement Variables
    public float moveSpeed;
    private Vector3 _moveDirection;
    
    // Health Variables
    public int health;

    // Animation Variables
    public Animator anim;
    public GameObject[] deathSplatters;
    public GameObject impactEffect;
    public SpriteRenderer sprite;
    private bool _isMoving;
    public float idleWaitForSeconds;
    private float _idleCounter;

    // Player-Related Variables
    public float rangeToChasePlayer;

    void Start()
    {
        _idleCounter = idleWaitForSeconds;
    }

    // This needs to be marked virtual, so it can be properly overriden in specific enemy type subclasses.
    public virtual void Movement()
    {
        // This ensures that the enemy will only move if visible on the screen
        if (sprite.isVisible)
        {
            // This condition only allows the enemy to chase the player if within a certain range
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
            {
                _moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                _moveDirection = Vector3.zero;
            }
            _moveDirection.Normalize();

            rigidBody.velocity = _moveDirection * moveSpeed;
        }

        // Update psrite appearance and animation based on movement;
        UpdateSprite();
    }

    public virtual void UpdateSprite()
    {
        // Handles animation variable checking for movement
        if (_moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
            _isMoving = true;
        }
        else
        {
            anim.SetBool("isMoving", false);
            _isMoving = false;
        }

        if (_isMoving)
        { 
            // Handles sprite direction to match movement direction
            if (_moveDirection.x > 0)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_moveDirection.x < 0)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        } else
        {
            // If idle. ranmdomly change sprite direction with 1 sec cool down
            if (_idleCounter <= 0)
            {
                if (Random.value > 0.5f)
                {
                    float curr_x = gameObject.transform.localScale.x;
                    gameObject.transform.localScale = new Vector3(-curr_x, 1, 1);
                }
                _idleCounter = idleWaitForSeconds;
            }
            else if (_idleCounter > 0 )
            {
                _idleCounter -= Time.deltaTime;
            }
        }
    }
}

