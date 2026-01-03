using UnityEngine;

public class ObjectToProtect : Entity
{
    [Header("extra details")]
    [SerializeField] private Transform player;

    protected override void Update()
    {
        handleflip();
    }

    protected override void handleflip()
    {
        if (player.transform.position.x > transform.position.x && facingright == false)
         flip();      
      else if (player.transform.position.x < transform.position.x && facingright == true)
         flip();      
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }
}
