using UnityEngine;

public class Entity_AnimationEvents : MonoBehaviour
{
 private Entity entity;

 private void Awake()
 {
   entity = GetComponentInParent<Entity>();     
 }

 public void DamageTargets() => entity.DamageTarget();

 public void DisableMovementAndJump() => entity.EnableMovementAndJump(false);

 public void EnableMovementAndJump() => entity.EnableMovementAndJump(true);  
}
