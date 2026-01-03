using UnityEngine;
public class enemy : Entity
{
  [Header("movement details")]
 [SerializeField]protected float moveSpeed = 3.5f;

  private bool playerDetected;

    protected override void Update()
    {
      base.Update();
      HandleAttack();
    }

    protected override void HandleAttack()
    {
        if (playerDetected == true)
        {
          anim.SetTrigger("attack");
        }
    }
   
  
    protected override void movement()
    {
        if (canMove == true)
        {
          rb.linearVelocity = new UnityEngine.Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        }
    
        else
        {
          rb.linearVelocity = new UnityEngine.Vector2(0, rb.linearVelocity.y);
        }
    }


    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(AttackPoint.position, AttackRadius, WhatisTarget);
    }

    protected override void Die()
    {
      base.Die();
      UI.instance.AddKillCount();
    }
}
