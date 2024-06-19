using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;
    public Transform player;
    public Transform firePoint;
    private Rigidbody2D rb;
    NavMeshAgent agent;
    EnemyHealthBar healthBar;
    public float maxHealth = 20f;
    private float currentHealth;
    public int dashDamage = 20; // Amount of damage dealt during dash

    public float dashDuration = 0.5f;
    public float dashSpeed = 30f;
    public float dashCooldown = 5f;

    private float rayRange = 140f;
    public float detectionRange = 150f;
    bool seenPlayer = false;
    bool canDash = true;   
    bool isAttacking = false;
    private bool canSpawnBullets = true;
    public LayerMask layerMask;


    public float bulletSpreadAngle = 45f; // Angle spread of bullets
    public int bulletCount = 25; // Number of bullets to spawn
    public float bulletSpeed = 20f; // Speed of bullets
    public float bulletSpawnCooldown = 3f; // Cooldown for bullet spawning
    public GameObject bulletPrefab;
    // Start is called before the first frame update



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();

        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustPlayerFacingDirection();
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, directionToPlayer, rayRange, layerMask );
            Debug.DrawRay(firePoint.position, directionToPlayer * rayRange, Color.red);

            if (hit.collider != null && !hit.collider.CompareTag("Enemy") && hit.collider.CompareTag("Player"))
            {
                seenPlayer = true;
              
                //  enemyWeapon.AimAtPlayer();
                agent.SetDestination(player.position);
                if (canDash && !isAttacking)
                {
                    StartCoroutine(DashAttack());
                }
                if (canSpawnBullets && !isAttacking)
                {
                    StartCoroutine(SpawnBullets());
                }              
                // enemyWeapon.Shoot();
            }
        }
        if (seenPlayer)
        {
            agent.SetDestination(player.position);
            // enemyWeapon.AimAtPlayer();
            if (canDash && !isAttacking)
            {
                StartCoroutine(DashAttack());
            }
            if (canSpawnBullets && !isAttacking)
            {
                StartCoroutine(SpawnBullets());
            }         
            // enemyWeapon.Shoot();
        }

        myAnimator.SetFloat("moveX", agent.velocity.x);
        myAnimator.SetFloat("moveY", agent.velocity.y);
    }

    IEnumerator DashAttack()
    {
        canDash = false;
        isAttacking = true;
        agent.isStopped = true;
        Vector3 dashDirection = (player.position - transform.position).normalized;
        Vector3 dashTarget = player.position + dashDirection * -10f;
        float dashEndTime = Time.time + dashDuration;

        while (Time.time < dashEndTime && Vector3.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }
        agent.isStopped = false;
        isAttacking = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    IEnumerator SpawnBullets()
    {
        canSpawnBullets = false;
        isAttacking = true;
        agent.isStopped = true;

        float angleStep = bulletSpreadAngle / (bulletCount - 1);
        float angle = -bulletSpreadAngle / 2;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = firePoint.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirY = firePoint.position.y + Mathf.Cos((angle * Mathf.PI) / 180);
            Vector3 bulletVector = new Vector3(bulletDirX, bulletDirY, 0);
            Vector3 bulletMoveDirection = (bulletVector - firePoint.position).normalized * bulletSpeed;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            rbBullet.velocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y);

            angle += angleStep;
        }

        yield return new WaitForSeconds(bulletSpawnCooldown);
        isAttacking = false;
        agent.isStopped = false;
        canSpawnBullets = true;
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isAttacking && collision.gameObject.CompareTag("Player"))
        {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.TakeDamage(dashDamage);    
            }
        }
    }
    void Die()
    {
        //Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        // Implement death behavior here (e.g., play death animation, disable GameObject)
        //SoundManager.PlaySound(SoundManager.SoundType.PistolShoot);
        // myAnimator.SetTrigger("isDead");
        // myAnimator.SetBool("isDead", true);
        // DisableEnemyActions();
        Destroy(gameObject);
        //StartCoroutine(DestroyAfterAnimation());
    }
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (player.position.x < transform.position.x)
        {
            if (seenPlayer)
            {
                mySpriteRenderer.flipX = true;  //flips the sprite to the players movement              
            }
        }
        else
        {
            if (seenPlayer)
            {
                mySpriteRenderer.flipX = false;
            }
        }
    }
}
