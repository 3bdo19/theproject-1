using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.MPE;
using UnityEngine;

public class Entity : MonoBehaviour
{

 protected Animator anim;
 protected Rigidbody2D rb;

 public Collider2D[] enemyColliders;
 [Header("attack details")]
 [SerializeField] protected float AttackRadius;
 [SerializeField] protected Transform AttackPoint;
 [SerializeField] protected LayerMask WhatisTarget;

[Header("movement details")]
 [SerializeField]protected float moveSpeed = 3.5f;
 [SerializeField]private float jumpforce = 8;
 protected int facingDir = 1;
 private float xinput;
 private bool facingright = true;
 protected bool canMove = true;
 private bool canJump = true;

 [Header("collision details")]
 [SerializeField] private float groundcheckdistance;
 [SerializeField] private LayerMask WhatisGround;
 private bool isGrounded;
 

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponentInChildren<Animator>();    
  }


  protected virtual void Update()
  {
    HandleCollision();
    handleinput();
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
    throw new NotImplementedException();
  }

  public void EnableMovementAndJump(bool enable)
  {
    canMove = enable;
    canJump = enable;

  }
  protected void handleanimations()
  {
    anim.SetFloat("xvelocity", rb.linearVelocity.x);
    anim.SetFloat("yvelocity", rb.linearVelocity.y);
    anim.SetBool("isGrounded", isGrounded);
  }

    private void handleinput()
  {
    xinput = Input.GetAxisRaw("Horizontal");

    if (Input.GetKeyDown(KeyCode.Space))
    {
      trytojump();
    }
  
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      TryToAttack();     
    }
      
  }
  
  protected virtual void TryToAttack()
  {
    if (isGrounded)
    {
        // 1. Trigger the animation
     anim.SetTrigger("attack"); 
      
    }
  }

  private void trytojump()
  {
    if (isGrounded == true && canJump == true)
    {
      rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, jumpforce);    
    }
          
  }

  protected virtual void movement()
  {
    if (canMove == true)
    {
      rb.linearVelocity = new UnityEngine.Vector2(xinput * moveSpeed, rb.linearVelocity.y);
    }
    
    else
    {
      rb.linearVelocity = new UnityEngine.Vector2(0, rb.linearVelocity.y);
    }
  }


  protected virtual void HandleCollision()
  {
    isGrounded = Physics2D.Raycast(transform.position, UnityEngine.Vector2.down, groundcheckdistance, WhatisGround);     
  }
  protected void handleflip()
  {
    if (rb.linearVelocity.x > 0 && facingright == false)
      flip();      
    else if (rb.linearVelocity.x < 0 && facingright == true)
      flip();      
        
  }
  private void flip()
  {
    transform.Rotate(0, 180 ,0);
    facingright = !facingright;
    facingDir = facingDir * -1;
  }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position , transform.position + new UnityEngine.Vector3(0, -groundcheckdistance));
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRadius);
    }
}
  