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

    // Player-Related Variables
    public float rangeToChasePlayer;

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

        // Handles animation variable checking for movement
        if (_moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
