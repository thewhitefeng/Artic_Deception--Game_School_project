using System.Collections;
using System.Collections.Generic;
// UIManager.cs
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;

    public void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        playerHealthText.text = "Player Health: " + currentHealth + "/" + maxHealth;
    }

    public void UpdateEnemyHealth(int currentHealth, int maxHealth)
    {
        enemyHealthText.text = "Enemy Health: " + currentHealth + "/" + maxHealth;
    }
}
