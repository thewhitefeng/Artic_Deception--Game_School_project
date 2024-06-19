using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class weapon : MonoBehaviour
{
    //Vector3 MousePosition;
    [SerializeField] private Transform aimTransform;
    public SpriteRenderer characterRenderer, weaponRenderer;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;

    public int magazineCapacity = 10; // Maximum number of bullets in the magazine
    public float reloadTime = 1f; // Time it takes to reload in seconds
    private bool isReloading = false;
    private int bulletsInMagazine;
    

     private TextMeshProUGUI bulletsText; // Reference to UI Text for displaying bullets
     private TextMeshProUGUI reloadText; // Reference to UI Text for displaying reload time
                                         // public float fireRate = 0.1f; // Rate of fire in bullets per second
    public GameObject _MuzzleFlash;                                 // private float nextFireTime; // Time of the next allowed fire
    private AudioSource _as;
     public AudioClip _ac;
    private void Awake()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
        GetComponent<weapon>().enabled = false;
        // collider enabled when triggered
        bulletsText = GameObject.Find("bulletsText").GetComponent<TextMeshProUGUI>();
        reloadText = GameObject.Find("reloadText").GetComponent<TextMeshProUGUI>();
        bulletsInMagazine = magazineCapacity;      
       
    } 
 
    private void Update()
    {
        _as = GetComponent<AudioSource>();
        aimTransform = transform.parent; // rotate around the player gameobject
        HandleAiming();
        UpdateUI(); // text show bullets
        if (Input.GetButtonDown("Fire1") && !isReloading )   
        {
            fire();
            CinemachineShake.Instance.ShakeCamera(5f, 0.3f);          
        }
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && bulletsInMagazine < magazineCapacity)
        {
            StartCoroutine(Reload());
        }
       
    }
    private void HandleAiming()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePosition.z = 0f;

        Vector2 aimDirection = (MousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
       // aimTransform.eulerAngles = new Vector3(0,0,angle);
        /* Vector2 scale = aimTransform.localScale;  
        if(aimDirection.x < 0)
        {
            scale.y = scale.y * (-1);
        }else if (aimDirection.x > 0)
        {
            scale.x = scale.x * 1;
        }
        transform.localScale = scale; */
        // Flip the weapon by rotating it 180 degrees if aiming to the left
        if (aimDirection.x < 0)                                                     //rotating the gun
        {                           
            aimTransform.localRotation = Quaternion.Euler(180, 0, -angle);
        }
        else
        {
            aimTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }

    }

    void fire()
    {
        if (bulletsInMagazine > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
            SoundManager.PlaySound(SoundManager.SoundType.PistolShoot);
         
            Instantiate(_MuzzleFlash, firePoint.position, firePoint.rotation);
            bulletsInMagazine--;
        }
        else
        {
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        SoundManager.PlaySound(SoundManager.SoundType.ReloadSound);
        reloadText.text = "Reloading..."; // Display reload text
        yield return new WaitForSeconds(reloadTime);
        bulletsInMagazine = magazineCapacity;
     //  SoundManager.PlaySound(SoundManager.SoundType.ReloadSound);
        isReloading = false;
        reloadText.text = string.Empty;    
    }
    void UpdateUI()
    {
        // Update bullets text
       if (bulletsText != null)
        {
            bulletsText.text = "Bullets: " + bulletsInMagazine + "/" + magazineCapacity;
        }
        
    }
  
}
 