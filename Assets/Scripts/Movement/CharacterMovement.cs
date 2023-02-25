using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{

    // Movement Variables
    public float moveSpeed;

    public Rigidbody2D rigidBody;

    private Camera _camera;

    public Animator animator;

    // Update is called once per frame
    public void Move(Vector3 _moveInput)
    {
        rigidBody.velocity = _moveInput * moveSpeed;

        Vector3 mousePos = Input.mousePosition;
    }
}
