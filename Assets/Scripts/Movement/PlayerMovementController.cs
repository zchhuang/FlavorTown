using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerMovementController : MonoBehaviour
{
    private CharacterMovement characterMovement;

    void Start()
    {
      characterMovement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
      Vector3 move_vector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
      move_vector.Normalize();

      characterMovement.Move(move_vector);
    }
}
