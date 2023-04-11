using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Character/Attack")]
public class AttackComponent : MonoBehaviour
{
    public int attackDamage;
    public int attackRange;
    public Weapon weapon;
    // public AttackPattern AttackPattern;

    public CharacterScriptableObject characterScriptableObject;

    private void Start()
    {
        attackDamage = characterScriptableObject.attackDamage;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
    HealthComponent healthComponent = collision.gameObject.GetComponent<HealthComponent>();

    // Only continue if the thing we hit has a Health component
    if (healthComponent) {
       // Reduce their health by our damage value!
       healthComponent.ChangeHealth(-attackDamage);
    }
  }

    public void Attack()
    {
      //do the attack and detect collision
    }
}
