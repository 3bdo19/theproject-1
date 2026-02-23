using UnityEngine;
using TMPro;

[ExecuteInEditMode] // This makes it work even when not in Play mode
public class ArabicFixer : MonoBehaviour
{
    private TMP_Text textComponent;

    void Update()
    {
        if (textComponent == null) textComponent = GetComponent<TMP_Text>();
        
        // Force Unity's internal RTL logic
        if (textComponent != null)
        {
            textComponent.isRightToLeftText = true;
            
            // This forces the letters to check their neighbors and "connect"
            textComponent.ForceMeshUpdate();
        }
    }
}