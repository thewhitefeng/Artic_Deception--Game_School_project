using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private TrailRenderer trailRenderer;
    public int damage = 20;
    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        Destroy(gameObject, 25.5f);
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true; // Enable the trail renderer immediately
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") && !collision.CompareTag("Weapon") && !collision.CompareTag("EnemyBullet") && !collision.CompareTag("Enemy"))
        {
            if (!playerMovement.isDashing)
            {
                Destroy(gameObject);
            }
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.TakeDamage(damage);
              
            }
        }
    }
}
