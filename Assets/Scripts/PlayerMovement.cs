using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private float MoveX;
    private float MoveY; 
    private Rigidbody2D rb;
    public float PlayerSpeed;

    Vector2 DirectionMovement;
    Vector2 MousePosition;
    private GameObject currentWeapon;
    public float pickUpRadius = 2f;
    
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCooldown = 1f;
     public bool isDashing;
    bool canDash=true;
   public bool canTakeDamage=true;

    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Transform aimTransform;
    private Collider2D myCollider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        aimTransform = transform.Find("Aim");
        myCollider = GetComponent<Collider2D>();
       // GameObject.DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (isDashing) { return; }
        Inputs();
   
        if(Input.GetKeyDown(KeyCode.Space)&&canDash)
        {          
            StartCoroutine(Dash());
        }
    }
    private void FixedUpdate()
    {
       // if (isDashing) { return; }

        Move();
        AdjustPlayerFacingDirection();
    }
    private void Inputs()
    {
        MoveX = Input.GetAxisRaw("Horizontal");
        MoveY = Input.GetAxisRaw("Vertical");

        myAnimator.SetFloat("moveX",DirectionMovement.x);
        myAnimator.SetFloat("moveY", DirectionMovement.y);
    }
    private void Move()
    {      
        DirectionMovement = new Vector2(MoveX, MoveY).normalized;
        rb.velocity = new Vector2(DirectionMovement.x * PlayerSpeed, DirectionMovement.y * PlayerSpeed);
    }
    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePos.x < playerScreenPoint.x )
        {
            mySpriteRenderer.flipX = true;  
            if (aimTransform != null)       
            {
                aimTransform.localPosition = new Vector3(-Mathf.Abs(aimTransform.localPosition.x), aimTransform.localPosition.y, aimTransform.localPosition.z);
            }
        } else
        {
            mySpriteRenderer.flipX= false;
            if (aimTransform != null)
            {
                aimTransform.localPosition = new Vector3(Mathf.Abs(aimTransform.localPosition.x), aimTransform.localPosition.y, aimTransform.localPosition.z);
            }
        }
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true; 
        canTakeDamage = false;
       // myCollider.enabled = false;
        rb.velocity = new Vector2 (DirectionMovement.x * dashSpeed, DirectionMovement.y * dashSpeed);
        myAnimator.SetBool("Roll",true);
        yield return new WaitForSeconds(dashDuration);
       // myCollider.enabled = true;
       canTakeDamage=true;
        isDashing = false;
        myAnimator.SetBool("Roll", false);
        yield return new WaitForSeconds(dashCooldown);  
        canDash = true;
    }

}
