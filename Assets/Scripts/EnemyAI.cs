using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class EnemyAI : MonoBehaviour
{

    public GameObject _MuzzleFlash;
    public GameObject bulletPrefab;
    public Transform player;
    public Transform firePoint;
    private Rigidbody2D rb;
    NavMeshAgent agent;
    EnemyHealthBar healthBar;
    public GameObject weaponPrefab;
    public GameObject bloodPrefab;
    public GameObject HealthBarDestroy;
    public float maxHealth = 4f;
    private float currentHealth;

    public float bulletDistance = 50f;
    public float detectionRange = 25f;
    public float shootingRange = 24f;
    public float distancetoStop = 20f;
    public float fireForce = 30f;
    private float lastShotTime;
    public float rotateSpeed = 0.25f;
    public float fireRate = 0.9f;
    public float moveSpeed = 17f;
    private float rayRange = 45f;
    public bool seenPlayer = false;
    // public AudioClip[] shootingSounds;
    EnemyWeapon enemyWeapon;
    private SpriteRenderer mySpriteRenderer;
    private Transform aimTransform;
    private Animator myAnimator;

    public float wanderRadius = 30f; // Radius in which the enemy can wander
    public float wanderInterval = 5f; // Time between each wander
    private Vector2 wanderTarget; // The target position to wander to
    public LayerMask layerMask;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        aimTransform = transform.Find("EnemyAim");
        myAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(Wander());

    }
    void Update()
    {
        AdjustPlayerFacingDirection();


        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector3 directionToPlayer = (player.position - firePoint.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, directionToPlayer, rayRange, layerMask);
            Vector3 endPos = firePoint.position + (firePoint.up * rayRange);
            Debug.DrawRay(firePoint.position, directionToPlayer * rayRange, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Player found");
            }

            if (hit.collider != null && !hit.collider.CompareTag("Enemy") && hit.collider.CompareTag("Player"))
            {
                seenPlayer = true;
                  enemyWeapon.AimAtPlayer();
                agent.SetDestination(player.position);
                enemyWeapon.Shoot();
            }
        }
        if (seenPlayer)
        {          
            agent.SetDestination(player.position);
            enemyWeapon.AimAtPlayer();
           // enemyWeapon.Shoot();
        }
            // Handle collision detection with bullets 
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bulletDistance); //checks if bullets hit or passed near then follow player
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Bullet"))
                {

                seenPlayer = true;
                agent.SetDestination(player.position);
                   // break;
                }
            }
        myAnimator.SetFloat("moveX", agent.velocity.x);
        myAnimator.SetFloat("moveY", agent.velocity.y);
    }
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if( player.position.x < transform.position.x )
        {
            if (seenPlayer)
            {
                mySpriteRenderer.flipX = true;  //flips the sprite to the players movement
                if (aimTransform != null)
                {
                    aimTransform.localPosition = new Vector3(-Mathf.Abs(aimTransform.localPosition.x), aimTransform.localPosition.y, aimTransform.localPosition.z); //flips the gun holder
                }
            }
        } else
        {
            if (seenPlayer)
            {
                mySpriteRenderer.flipX = false;
                if (aimTransform != null)
                {
                    aimTransform.localPosition = new Vector3(Mathf.Abs(aimTransform.localPosition.x), aimTransform.localPosition.y, aimTransform.localPosition.z);
                }
            }
        }
    }
    
    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
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
        Instantiate(bloodPrefab, transform.position, transform.rotation);
        Destroy(HealthBarDestroy);
        myAnimator.SetBool("isDead", true); 
        DisableEnemyActions();
        
        StartCoroutine(DestroyAfterAnimation());
    }
    void DisableEnemyActions()
    {
        agent.isStopped = true;
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false; // Disable enemy collider
            enemyWeapon.enabled = false;
        this.enabled = false;
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the death animation to finish
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    IEnumerator Wander()
    {
        while (true)
        {
            if (!seenPlayer) // Only wander if the player hasn't been seen
            {
                wanderTarget = GetRandomWanderTarget();
                agent.SetDestination(wanderTarget);
            }
            yield return new WaitForSeconds(wanderInterval);
        }
    }

    Vector2 GetRandomWanderTarget()
    {
        Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
        randomDirection += (Vector2)transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        return hit.position;
    }
}
