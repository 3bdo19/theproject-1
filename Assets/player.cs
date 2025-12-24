using System;
using System.Numerics;
using UnityEditor.MPE;
using UnityEngine;

public class player : MonoBehaviour
{

 private Animator anim;
 private Rigidbody2D rb;
[Header("movement details")]
 [SerializeField]private float moveSpeed = 3.5f;
 [SerializeField]private float jumpforce = 8;
 private float xinput;
 private bool facingright = true;
 private bool canMove = true;
 private bool canJump = true;

 [Header("collision details")]
 [SerializeField] private float groundcheckdistance;
 [SerializeField] private LayerMask WhatisGround;
 private bool isGrounded;
 

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponentInChildren<Animator>();    
  }


  void Update()
  {
    HandleCollision();
    handleinput();
    movement();
    handleanimations();
    handleflip();
    
  }

  public void EnableMovementAndJump(bool enable)
  {
    canMove = enable;
    canJump = enable;

  }
  private void handleanimations()
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
  
  private void TryToAttack()
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

  private void movement()
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


  private void HandleCollision()
  {
    isGrounded = Physics2D.Raycast(transform.position, UnityEngine.Vector2.down, groundcheckdistance, WhatisGround);     
  }
  private void handleflip()
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
  }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position , transform.position + new UnityEngine.Vector3(0, -groundcheckdistance));
    }
}
  