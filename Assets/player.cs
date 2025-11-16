using System;
using UnityEditor.MPE;
using UnityEngine;

public class player : MonoBehaviour
{

 private Animator anim;
 private Rigidbody2D rb;

 [SerializeField]private float moveSpeed = 3.5f;
 [SerializeField]private float jumpforce = 8;
 private float xinput;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponentInChildren<Animator>();    
  }


  void Update()
  {
  
    handleinput();
    movement();
    handleanimations();
    
  }

    private void handleanimations()
    {
      bool ismoving = rb.linearVelocity.x != 0;
      anim.SetBool("ismoving", ismoving);
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
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);      
  }
}
  