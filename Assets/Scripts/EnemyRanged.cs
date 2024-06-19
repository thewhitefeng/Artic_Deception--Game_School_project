using UnityEngine;
using UnityEngine.AI;
public class EnemyRanged : MonoBehaviour
{
    public Transform player;
    NavMeshAgent agent;
    EnemyHealthBar healthBar;
    public float maxHealth = 4f;
    private float currentHealth;
    public bool seenPlayer = false;
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;

    public float damageCooldown = 1.0f;  // Time between damage inflictions
    private float lastDamageTime;
    public int damage = 10;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        }
    private void Update()
    {
        AdjustPlayerFacingDirection();
        agent.SetDestination(player.position);
       // myAnimator.SetFloat("moveX", agent.velocity.x);
      //  myAnimator.SetFloat("moveY", agent.velocity.y);
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
        //SoundManager.PlaySound(SoundManager.SoundType.PistolShoot);
        // myAnimator.SetTrigger("isDead");
       // myAnimator.SetBool("isDead", true);
        Destroy(gameObject);
    }
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (player.position.x < transform.position.x)
        {         
                mySpriteRenderer.flipX = true;  //flips the sprite to the players movement                  
        }
        else
        {      
                mySpriteRenderer.flipX = false;           
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}
