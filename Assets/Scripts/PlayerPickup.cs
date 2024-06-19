using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 2f; // Set the pickup range
    private GameObject currentWeapon;
    private MonoBehaviour currentWeaponScript;
    private TextMeshProUGUI bulletsText;
    public weapon bullText;
  
    void Awake()
    {
        bulletsText = GameObject.Find("bulletsText").GetComponent<TextMeshProUGUI>();

    }
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupWeapon();
            if (currentWeapon != null)
            {
                EnableCurrentWeaponScript();            
                bulletsText.gameObject.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        { 
            DropWeapon();
            bulletsText.text = string.Empty;
         }
        
    }
    void TryPickupWeapon()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange);

        foreach (Collider2D collider in colliders)
        {
                if (collider.CompareTag("Weapon"))
                {
                    if (currentWeapon != null)
                    {
                        // Drop the current weapon before picking up the new one
                        DropWeapon();
                    }
                    // Pick up the weapon
                    PickUp(collider.gameObject);
                    // Disable the weapon's collider and enable it for the player
                    collider.enabled = false;
                    // Set the picked up weapon as the current weapon
                    currentWeapon = collider.gameObject;
                    // Set the weapon's parent to the player so it moves with the player
                    currentWeapon.transform.parent = transform;
                    // Optionally, set the weapon's position relative to the player
                    currentWeapon.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
                    // Rotate the weapon to match the player's forward direction
                    currentWeapon.transform.rotation = transform.rotation; 
                    break;
                }
        }
    }

    void PickUp(GameObject weapon)
    {        
        // Store the current weapon reference (you might want to keep track of the player's current weapon)
        currentWeapon = weapon;      
        currentWeaponScript = currentWeapon.GetComponent<MonoBehaviour>();//scripts
    }
   
        void DropWeapon()
    {
        if (currentWeapon != null)
        {
            // Set the weapon's parent back to null to detach it from the player
            currentWeapon.transform.parent = null;
            // Enable the weapon's collider
            currentWeapon.GetComponent<Collider2D>().enabled = true;          
            DisableCurrentWeaponScript();
            // Reset the current weapon reference
            currentWeapon = null;
        }
    }
    void EnableCurrentWeaponScript()
    {
        if (currentWeaponScript != null)
        {
            currentWeaponScript.enabled = true;
        }
    }

    void DisableCurrentWeaponScript()
    {
        if (currentWeaponScript != null )
        {
            currentWeaponScript.enabled = false;           
        }
    }
}
