using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableComponent : MonoBehaviour
{
  public bool ableToBeDamaged;

  public CharacterScriptableObject characterScriptableObject;

  private void Start()
  {
      //Object has just spawned and gains the amount of Health in the Scriptable Object
      int maximumHealth = characterScriptableObject.maximumHealth;
  }

  public void TakeDamage()
  {
    return;
  }
}
