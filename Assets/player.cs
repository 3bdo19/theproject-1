using UnityEngine;

public class player : MonoBehaviour
{

 private Rigidbody2D rb;

 [SerializeField]private float moveSpeed = 3.5f;
 [SerializeField]private float jumpforce = 8;
 private float xinput;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();    
  }


  void Update()
  {
      xinput = Input.GetAxisRaw("Horizontal");

      rb.linearVelocity = new Vector2(xinput * moveSpeed, rb.linearVelocity.y);

      if (Input.GetKeyDown(KeyCode.Space))
      {
          rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
      }
  }
}
  