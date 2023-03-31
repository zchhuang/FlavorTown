using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Settings")]
    [SerializeField] private Image healthBar;

    private float playerCurrentHealth;
    private float playerMaxHealth;

    private void Start()
    {
        playerCurrentHealth = 5;
        playerMaxHealth = 5;
    }

    private void Update()
    {
        InternalUpdate();

        if (Input.GetKeyDown(KeyCode.K))
        {
            playerCurrentHealth++;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            playerCurrentHealth--;
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {

        playerCurrentHealth = currentHealth;
        playerMaxHealth = maxHealth;
    }

    private void InternalUpdate()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, playerCurrentHealth / playerMaxHealth, 10f * Time.deltaTime);
    }
}
