using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public int heal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.PlaySound(SoundManager.SoundType.HealSound);// add sound
            HealthManager healthManager = other.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                // Increase the player's health
                healthManager.TakeHeal(heal);
                Destroy(gameObject);
            }
        }
    }
}
