using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
      bool isSpawning = false;
    public float spawnCoolDown = 4f;
    public GameObject enemyPrefeb;
    public Transform spawnPoint;

    public float maxHealth = 4f;
    private float currentHealth;
    EnemyHealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        currentHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
    IEnumerator SpawnEnemies()
    {
        isSpawning = true;
        Instantiate(enemyPrefeb, spawnPoint.position,spawnPoint.rotation );
        yield return new WaitForSeconds(spawnCoolDown);
        isSpawning = false;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
