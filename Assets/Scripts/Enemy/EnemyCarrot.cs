using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarrot : Enemy
{
    public void Reset()
    {
        // Movement Variables
        moveSpeed = 5;

        // Health Variables
        health = 10;

        // Animation Variables
        deathSplatters = new GameObject[] { };
        impactEffect = null;

    }
}

