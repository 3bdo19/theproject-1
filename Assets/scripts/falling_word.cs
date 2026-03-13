using UnityEngine;
using TMPro;
using Unity.Cinemachine;

public class FallingWord : MonoBehaviour
{
    private bool isCorrect;
    private WordDestroyer manager;
    private float speed;
    public int gameHelper;

    private TextMeshPro textComponent;

    public void Setup(string text, bool correct, WordDestroyer m, float s)
    {
        textComponent = GetComponentInChildren<TextMeshPro>();
        
        if (textComponent != null)
        {
            textComponent.text = text;
        }
        else
        {
            var uiText = GetComponentInChildren<TextMeshProUGUI>();
            if (uiText != null) uiText.text = text;
        }

        isCorrect = correct;
        manager = m;
        speed = s; 
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

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
            GameObject managerObj = GameObject.Find("ShakeManager");
            if (managerObj != null)
            {
                var source = managerObj.GetComponent<Unity.Cinemachine.CinemachineImpulseSource>();
                if (source != null) source.GenerateImpulse();
            }

            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null) playerScript.die();

            if (manager != null)
            {
                manager.WordClicked(false);
            }
        }
        
        Destroy(gameObject);
    }
}
}