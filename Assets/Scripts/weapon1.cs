using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class weapon1 : MonoBehaviour
{
    private Transform aimTransform;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;

    public float fireRate = 0.1f; // Rate of fire in bullets per second
    private float nextFireTime; // Time of the next allowed fire

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
        GetComponent<weapon1>().enabled = false;
        // collider enabled when triggered
        Collider2D collider = GetComponent<Collider2D>();
       // collider.isTrigger = true;

        bulletsText = GameObject.Find("bulletsText").GetComponent<TextMeshProUGUI>();
        reloadText = GameObject.Find("reloadText").GetComponent<TextMeshProUGUI>();

        bulletsInMagazine = magazineCapacity;
    }
    private void Update()
    {
        aimTransform = transform.parent;
        HandleAiming();
        UpdateUI();
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && !isReloading)
        {
            fire();
            CinemachineShake.Instance.ShakeCamera(4f, 0.3f);
            nextFireTime = Time.time + 1f / fireRate;
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
    void fire()
    {
        if (bulletsInMagazine > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
         Rigidbody2D rb  = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
            SoundManager.PlaySound(SoundManager.SoundType.AutoShoot);// sound
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
 