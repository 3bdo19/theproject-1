using UnityEngine;
public class enemy : Entity
{
    protected override void Update()
    {
     HandleCollision();
     movement();
     handleanimations();
     handleflip();
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
}
