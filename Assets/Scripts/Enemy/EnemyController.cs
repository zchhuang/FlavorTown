using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to run enemy behavior
public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemy.Movement();
    }

    public void DamageEnemy(int damage)
    {
        enemy.health -= damage;

        if (enemy.health <= 0)
        {
            Destroy(gameObject);

            // We need to handle three things here in the future
            // 1. Death Animation
            // 2. Sprite after Death
            // 3. Enemy Item Drops
        }
    }
}
