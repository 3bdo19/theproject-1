using UnityEngine;

public class CursorFixer : MonoBehaviour
{
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Cursor Forced Visible!");
        }
    }
}