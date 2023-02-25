using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterMovement))]
public class AIMovementController : MonoBehaviour
{
    private CharacterMovement characterMovement;

    public float rangeToChasePlayer;

    public Animator anim;
    public GameObject[] deathSplatters;
    public GameObject impactEffect;
    public SpriteRenderer sprite;

    void Start()
    {
      characterMovement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
      //temporary
      Vector3 _moveDirection = Vector3.zero;
      // This ensures that the enemy will only move if visible on the screen
      if (sprite.isVisible)
      {
          // This condition only allows the enemy to chase the player if within a certain range
          if (shouldChasePlayer())
          {
              //_moveDirection = PlayerController.instance.transform.position - transform.position;
          }
          else
          {
              //_moveDirection = Vector3.zero;
          }
          //_moveDirection.Normalize();
          characterMovement.Move(_moveDirection);
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

    public bool shouldChasePlayer()
    {
     ///Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer
        return true;
    }
}
