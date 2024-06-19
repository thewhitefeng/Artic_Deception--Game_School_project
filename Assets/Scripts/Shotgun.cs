using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    private Transform aimTransform;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 30f;

    public int pelletsPerShot = 5; // Number of pellets per shot
    public float spreadAngle = 20f; // Spread angle of the pellets/ Start is called before the first frame update

    public int magazineCapacity = 25; // Maximum number of bullets in the magazine
    public float reloadTime = 2f; // Time it takes to reload in seconds
    private bool isReloading = false;
    public int bulletsInMagazine;
    public GameObject _MuzzleFlash;

    private TextMeshProUGUI bulletsText; // Reference to UI Text for displaying bullets
    private TextMeshProUGUI reloadText;  // Reference to UI Text for displaying reload time
    public SpriteRenderer characterRenderer, weaponRenderer; 

    private void Awake()
    {
        GetComponent<Shotgun>().enabled = false;
        // collider enabled when triggered
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
       
        bulletsText = GameObject.Find("bulletsText").GetComponent<TextMeshProUGUI>();
        reloadText = GameObject.Find("reloadText").GetComponent<TextMeshProUGUI>();

        bulletsInMagazine = magazineCapacity;
    }
   
    // Update is called once per frame
    void Update()
    {
        aimTransform = transform.parent;
        HandleAiming();
        UpdateUI();
        if (Input.GetButtonDown("Fire1") && !isReloading)   //&& Time.time >= nextFireTime)
        {
            Fire();
            CinemachineShake.Instance.ShakeCamera(5f, 1f);
            // nextFireTime = Time.time + 1f / fireRate;
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

        Vector3 aimDirection = (MousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        if (aimDirection.x < 0)
        {
            aimTransform.localRotation = Quaternion.Euler(180, 0, -angle);
        }
        else
        {
            aimTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }

    }
    public void Fire()
    {
        if (bulletsInMagazine > 0)
        {
        // Iterate to spawn pellets
        for (int i = 0; i < pelletsPerShot; i++)
        {
            // Calculate rotation based on spread angle
            Quaternion spreadRotation = Quaternion.Euler(0f, 0f, Random.Range(-spreadAngle / 2f, spreadAngle / 2f));
            // Spawn pellet with spread rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * spreadRotation);
            // Add force to the pellet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.right * fireForce, ForceMode2D.Impulse);
        }
            Instantiate(_MuzzleFlash, firePoint.position, firePoint.rotation);
            SoundManager.PlaySound(SoundManager.SoundType.ShotgunShoot);
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