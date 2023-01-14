using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent class for enemy object.
public class Enemy
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
}
