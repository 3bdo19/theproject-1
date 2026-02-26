using UnityEngine;
using TMPro;
using Unity.Cinemachine;

public class FallingWord : MonoBehaviour
{
    private bool isCorrect;
    private WordDestroyer manager;
    private float speed;

    // Use TextMeshPro (World Space) instead of TextMeshProUGUI (Canvas)
    private TextMeshPro textComponent;

    public void Setup(string text, bool correct, WordDestroyer m, float s)
    {
        // 🛠️ FIX 1: Try both possibilities to prevent the NullReference blizzard
        textComponent = GetComponentInChildren<TextMeshPro>();
        
        if (textComponent != null)
        {
            textComponent.text = text;
        }
        else
        {
            // If it's still using the UI version, this catches it
            var uiText = GetComponentInChildren<TextMeshProUGUI>();
            if (uiText != null) uiText.text = text;
        }

        isCorrect = correct;
        manager = m;
        speed = s; 
    }

    void Update()
    {
        // 🛠️ FIX 2: Gravity handles the fall if you have a Rigidbody2D.
        // But if you prefer script-movement, this works:
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    // This handles the click in the game world
    private void OnMouseDown()
    {
        if (manager != null)
        {
            manager.WordClicked(isCorrect);
            Destroy(gameObject);
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        if (!isCorrect) 
        {
            // 1. SHAKE (Now it will actually run because we fixed the error below)
            GameObject managerObj = GameObject.Find("ShakeManager");
            if (managerObj != null)
            {
                var source = managerObj.GetComponent<Unity.Cinemachine.CinemachineImpulseSource>();
                if (source != null) source.GenerateImpulse();
            }

            // 2. KILL PLAYER
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null) playerScript.die();

            // 🛠️ FIX FOR LINE 60:
            // We check if manager exists. If it's null, the game won't crash anymore.
            if (manager != null)
            {
                manager.WordClicked(false);
            }
        }
        
        Destroy(gameObject);
    }
}
}