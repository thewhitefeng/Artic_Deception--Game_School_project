using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject _MuzzleFlash;
    public float fireForce;
    public float fireRate;
    private float lastShotTime;
    public Transform player;
    public EnemyAI enemyAI;
    private Transform aimTransform;
    public float rayRange = 20f;
    public float detectionRadius = 40f; // The radius for the circular detection
    // Start is called before the first frame update
    void Start()
    {
        aimTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
               // AimAtPlayer();
                Debug.Log("Player found within circle");
                break; // Break out of the loop once the player is found
            }
        }
        
       // Shoot();
    }


    public void Shoot()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            GameObject EnemyBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = EnemyBullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
            SoundManager.PlaySound(SoundManager.SoundType.EnemyHit);
            Instantiate(_MuzzleFlash, firePoint.position, firePoint.rotation);
            lastShotTime = Time.time;
        }
    }
    public void AimAtPlayer()
    {
     
        Vector2 aimDirection = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (aimDirection.x < 0)
        {
            aimTransform.localRotation = Quaternion.Euler(180, 0, -angle);
        }
        else
        {
            aimTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }

    }
    // To visualize the detection radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
