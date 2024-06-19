using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
    public float bulletTime = 2.5f;
    private TrailRenderer trailRenderer;
    void Start()
    {
        Destroy(gameObject, bulletTime);

        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true; // Enable the trail renderer immediately
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") && !collision.gameObject.CompareTag("EnemyBullet") &&
            !collision.CompareTag("Weapon") && !collision.CompareTag("Player") && !collision.CompareTag("EnemyWeapon"))
        {
            Destroy(gameObject);

            // Handle damage to different components
            TurretScript turretScript = collision.gameObject.GetComponent<TurretScript>();
            if (turretScript != null)
            {
                turretScript.TakeDamage(damage);
            }

            testranged TestRanged = collision.gameObject.GetComponent<testranged>();
            if (TestRanged != null)
            {
                TestRanged.TakeDamage(damage);
            }

            EnemyRanged enemyRanged = collision.gameObject.GetComponent<EnemyRanged>();
            if (enemyRanged != null)
            {
                enemyRanged.TakeDamage(damage);
            }

            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage);
            }

            Boss boss = collision.gameObject.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Spawner spawner = collision.gameObject.GetComponent<Spawner>();
            if (spawner != null)
            {
                spawner.TakeDamage(damage);
            }
        }
    }
}
