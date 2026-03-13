using UnityEngine;

public class Player : Entity
{
    [Header("movement details")]
    [SerializeField]protected float moveSpeed = 3.5f;
    [SerializeField]private float jumpforce = 8;
    private float xinput;
    private bool canJump = true;
    protected Transform player;
    private scripts_manager myManager;


    protected override void Awake()
    {
        base.Awake();
        myManager = FindObjectOfType<scripts_manager>();

        player = FindFirstObjectByType<Player>().transform; 
    }


    protected override void Update()
    {
        base.Update();
        handleinput();
    }


    protected override void movement()
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

    private void trytojump()
    {
    if (isGrounded == true && canJump == true)
    {
      rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, jumpforce);    
    }
          
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
      HandleAttack();     
     } 
    }

    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }

    public void die()
    {
       anim.enabled = false;
       col.enabled = false;
       rb.gravityScale = 12;
       rb.linearVelocity= new UnityEngine.Vector2(rb.linearVelocity.x, 15);

     myManager.TriggerGameOver(); 
    

       Destroy(gameObject, 3);
    }
}
