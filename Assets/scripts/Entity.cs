using System;
using System.Collections;
using System.Numerics;
using UnityEngine;

public class Entity : MonoBehaviour
{

 protected Animator anim;
 protected Rigidbody2D rb;
 protected Collider2D col;
 protected SpriteRenderer sr;

 [Header("Health")]
 [SerializeField]private int maxhealth = 1;
 [SerializeField]private int currentHealth;
 [SerializeField]private Material damagematerial;
 [SerializeField]private float damagefeedbackduration = .1f;
 private Coroutine damageFeedbackCoroutine;


 [Header("attack details")]
 [SerializeField] protected float AttackRadius;
 [SerializeField] protected Transform AttackPoint;
 [SerializeField] protected LayerMask WhatisTarget;

 [Header("collision details")]
 [SerializeField] private float groundcheckdistance;
 [SerializeField] private LayerMask WhatisGround;
 protected bool isGrounded;

 // facing direction details
  protected int facingDir = 1;
 protected bool facingright = true;
 protected bool canMove = true;
 

  protected virtual void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponentInChildren<Animator>();   
    col = GetComponent<Collider2D>(); 
    sr = GetComponentInChildren<SpriteRenderer>();

    currentHealth = maxhealth;
  }


  protected virtual void Update()
  {
    HandleCollision();
    movement();
    handleanimations();
    handleflip();
    
  }

  public void DamageTarget()
  {
    Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRadius, WhatisTarget);
    foreach (Collider2D enemy in enemyColliders)
    {
      Entity entityTarget = enemy.GetComponent<Entity>();
      entityTarget.Takedamage();
    }
  }

  public void Takedamage()
  {
    currentHealth = currentHealth - 1;
     
    playDamageFeedback();

    if (currentHealth <= 0)
    {
      Die();
    }
  }

  private void playDamageFeedback()
  {
    if(damageFeedbackCoroutine != null)
    {
      StopCoroutine(damageFeedbackCoroutine);
    }

    StartCoroutine(DamageFeedbackCo());
  }

  private IEnumerator DamageFeedbackCo()
  {
   Material originalMat = sr.material;
   sr.material = damagematerial;

   yield return new WaitForSeconds(damagefeedbackduration);

   sr.material = originalMat;

  }

  protected virtual void Die()
  {
    anim.enabled = false;
    col.enabled = false;
    rb.gravityScale = 12;
    rb.linearVelocity= new UnityEngine.Vector2(rb.linearVelocity.x, 15);

    Destroy(gameObject, 3);
  }

  public virtual void EnableMovement(bool enable)
  {
    canMove = enable;

  }
  protected void handleanimations()
  {
    anim.SetFloat("xvelocity", rb.linearVelocity.x);
    anim.SetFloat("yvelocity", rb.linearVelocity.y);
    anim.SetBool("isGrounded", isGrounded);
  }


  
  protected virtual void HandleAttack()
  {
    if (isGrounded)
    {
        // 1. Trigger the animation
     anim.SetTrigger("attack"); 
      
    }
  }



  protected virtual void movement()
  {

  }


  protected virtual void HandleCollision()
  {
    isGrounded = Physics2D.Raycast(transform.position, UnityEngine.Vector2.down, groundcheckdistance, WhatisGround);     
  }


  protected virtual void handleflip()
  {
    if (rb.linearVelocity.x > 0 && facingright == false)
      flip();      
    else if (rb.linearVelocity.x < 0 && facingright == true)
      flip();      
        
  }
  public void flip()
  {
    transform.Rotate(0, 180 ,0);
    facingright = !facingright;
    facingDir = facingDir * -1;
  }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position , transform.position + new UnityEngine.Vector3(0, -groundcheckdistance));
        
        if(AttackPoint != null)
          Gizmos.DrawWireSphere(AttackPoint.position, AttackRadius);
    }
}
  