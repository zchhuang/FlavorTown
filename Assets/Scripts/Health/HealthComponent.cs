using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Character/Health")]
public class HealthComponent : MonoBehaviour
{
    public int currentHealth;
    public int maximumHealth;

    public CharacterScriptableObject characterScriptableObject;

    private void Start()
    {
        //Object has just spawned and gains the amount of Health in the Scriptable Object
        maximumHealth = characterScriptableObject.maximumHealth;
        currentHealth = maximumHealth;
    }

    public void ChangeHealth(int damage)
    {
        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
            // Die
        }
        else
        {
            currentHealth = currentHealth - damage;
        }
    }
}
