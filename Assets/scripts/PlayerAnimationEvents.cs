using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
 private Entity Player;

 private void Awake()
 {
   Player = GetComponentInParent<Entity>();     
 }

 public void DamageEnemies() => Player.DamageTarget();

 public void DisableMovementAndJump() => Player.EnableMovementAndJump(false);

 public void EnableMovementAndJump() => Player.EnableMovementAndJump(true);  
}
