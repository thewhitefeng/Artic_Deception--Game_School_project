using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
   [SerializeField] private DoorSetActive Door ;
    public float pickupRange = 1.5f;
    private bool isClosed = true ;

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange);

       
            if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    ToggleDoor();
                }
            }
              
        }
      

    }

    private void ToggleDoor()
    {
        if (isClosed == true)
        {
            Door.OpenDoor();
            isClosed = false;
        }
        else if (isClosed == false)
        {
            Door.CloseDoor();
            isClosed = true;
        }
    }

}
