using UnityEngine;
using UnityEngine.AI;
public class testranged : MonoBehaviour
{
    public int pelletsPerShot = 5; // Number of pellets per shot
    public float spreadAngle = 20f; // Spread angle of the pellets/ Start is called before the first frame update


    public GameObject _MuzzleFlash;
    public GameObject bulletPrefab;
    public Transform player;
    public Transform firePoint;
    private Rigidbody2D rb;
    NavMeshAgent agent;
    EnemyHealthBar healthBar;
    public GameObject weaponPrefab;
    public float maxHealth = 4f;
    private float currentHealth;

    public float detectionRange = 25f;
    public float shootingRange = 24f;
    public float distancetoStop = 20f;
    public float fireForce = 30f;
    private float lastShotTime;
    public float rotateSpeed = 0.25f;
    public float fireRate = 0.9f;
    public float moveSpeed = 17f;
    private float rayRange = 60f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();

    }
    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Update()
    {
        if (player != null)
        {

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, player.position - firePoint.position, rayRange);
            Vector3 endPos = firePoint.position + (firePoint.up * rayRange);
            Debug.DrawRay(firePoint.position, (player.position - firePoint.position).normalized * rayRange, Color.red);

            if (distanceToPlayer <= detectionRange)     // 
            {
                RotateTowardsTarget();

                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    if (distanceToPlayer <= shootingRange)  // range to shoot
                    {
                        Shoot();

                    }
                    if (distanceToPlayer >= distancetoStop) // range to stopfollowing the player and shoot...
                    {
                        MoveTowardsPlayer();
                        // agent.SetDestination(player.position);
                    }
                }
                else if (hit.collider.CompareTag("Wall"))
                {
                    MoveTowardsPlayer();
                    // agent.SetDestination(player.position);
                }
                else
                {
                    agent.SetDestination(player.position);
                }
            }
        }
    }

    void Shoot()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            for (int i = 0; i < pelletsPerShot; i++)
           {
                // Calculate rotation based on spread angle
                Quaternion spreadRotation = Quaternion.Euler(0f, 0f, Random.Range(-spreadAngle / 2f, spreadAngle / 2f));

                // Spawn pellet with spread rotation
                GameObject EnemyBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * spreadRotation);
                Rigidbody2D rb = EnemyBullet.GetComponent<Rigidbody2D>();
                rb.AddForce(EnemyBullet.transform.up * fireForce, ForceMode2D.Impulse);
            }
            Instantiate(_MuzzleFlash, firePoint.position, firePoint.rotation);
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
        //Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        // Implement death behavior here (e.g., play death animation, disable GameObject)
        Destroy(gameObject);
    }
}
