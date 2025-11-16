using UnityEngine;

public class player : MonoBehaviour
{

 private Rigidbody2D rb;
 private float moveSpeed = 3.5f;
 private float xinput;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();    
  }


  void Update()
  {
      xinput = Input.GetAxisRaw("Horizontal");

      rb.linearVelocity = new Vector2(xinput * moveSpeed, rb.linearVelocity.y);
  }
}
  