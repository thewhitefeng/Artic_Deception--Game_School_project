using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private UIManager uiManager;
    public TextMeshProUGUI playerHealthText;
    PlayerHealthBar healthBar;
    public PauseMenu pauseMenu;
    private bool isDead;
    PlayerMovement PlayerMovement;
    private DamageOverlayController damageOverlayController;
    [SerializeField] private GameObject healthBarDestroy;
    [SerializeField] private GameObject bulletsTextDestroy;

    private void Awake()
    {
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBar>();
        PlayerMovement = gameObject.GetComponent<PlayerMovement>();
        damageOverlayController = FindObjectOfType<DamageOverlayController>();
    }
    void Start()
    {
        currentHealth = maxHealth;       
    }
    private void Update()
    {
  
    }
    public void TakeDamage(int damage)
    {
        if (PlayerMovement.canTakeDamage)
        {
            currentHealth -= damage;
            damageOverlayController.ShowDamageEffect();
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                Die();
                pauseMenu.GameOver();
               
            }
        }
    }
    public void TakeHeal(int heal)
    {
        if(currentHealth < maxHealth)
        {           
            currentHealth += heal;
            damageOverlayController.ShowHealEffect();
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }      
    }

    void Die()
    {
        Destroy(gameObject);
        healthBarDestroy.SetActive(false);
        bulletsTextDestroy.SetActive(false);
    }
   
   
}
