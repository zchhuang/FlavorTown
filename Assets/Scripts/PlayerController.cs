using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    private Vector2 _moveInput;

    public Rigidbody2D rigidBody;

    private Camera _camera;

    public Animator animator;

    // Awake is called when the script is loaded
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        rigidBody.velocity = _moveInput * moveSpeed;

        Vector3 mousePos = Input.mousePosition;
    }
}
