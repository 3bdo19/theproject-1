using System;
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
      jump();
    }
  }

  private void movement()
  {
    rb.linearVelocity = new Vector2(xinput * moveSpeed, rb.linearVelocity.y);
  }

  private void jump()
  {
    if (isGrounded)
    {
      rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);    
    }
          
  }

  private void HandleCollision()
  {
    isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundcheckdistance, WhatisGround);     
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
        Gizmos.DrawLine(transform.position , transform.position + new Vector3(0, -groundcheckdistance));
    }
}
 