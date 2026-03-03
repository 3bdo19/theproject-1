using UnityEngine;

public class CursorFixer : MonoBehaviour
{
    void Update()
    {
        // Press 'M' during play mode to force the mouse back if it disappears
        if (Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Cursor Forced Visible!");
        }
    }
}