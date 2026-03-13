using UnityEngine;
using TMPro;

[ExecuteInEditMode] 
public class ArabicFixer : MonoBehaviour
{
    private TMP_Text textComponent;

    void Update()
    {
        if (textComponent == null) textComponent = GetComponent<TMP_Text>();
        
        if (textComponent != null)
        {
            textComponent.isRightToLeftText = true;
            
            textComponent.ForceMeshUpdate();
        }
    }
}