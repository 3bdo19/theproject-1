using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
 private player Player;

 private void Awake()
 {
   Player = GetComponentInParent<player>();     
 }

 public void DisableMovementAndJump() => Player.EnableMovementAndJump(false);

 public void EnableMovementAndJump() => Player.EnableMovementAndJump(true);  
}
