using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassDoor : MonoBehaviour
{
    public GameObject Spawners;
    public Transform spawnPoint1;
    public Transform spawnPoint2;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            if (spawnPoint1 != null)
            {
                Instantiate(Spawners, spawnPoint1.position, spawnPoint1.rotation);
            }
           if (spawnPoint2 != null) {
                Instantiate(Spawners, spawnPoint2.position, spawnPoint2.rotation);
            }
      
        }
    }
   
}
