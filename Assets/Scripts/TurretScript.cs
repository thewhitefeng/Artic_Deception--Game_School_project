using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform player;
    public Transform firePoint;
    public float maxHealth = 4f;
    private float currentHealth;
    EnemyHealthBar healthBar;
    public float detectionRange = 40f;
    public float shootingRange = 39f;
    public float fireForce = 30f;
    private float lastShotTime;
    public float rotateSpeed = 0.25f;
    public float fireRate = 0.9f;
    private float rayRange = 50f;
    // Start is called before the first frame update
   private void Awake()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    void Start()
    {    
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, player.position - firePoint.position, rayRange);

            if (distanceToPlayer <= detectionRange)
            {
                RotateTowardsTarget();
                if (hit.collider != null && hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Turret"))
                {
                    if (distanceToPlayer <= shootingRange)  // range to shoot
                    {
                        Shoot();
                    }
                }
            }
        }
    }
    void Shoot()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            GameObject EnemyBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = EnemyBullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            lastShotTime = Time.time;
        }
    }
    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = player.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
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
