using UnityEngine;

[CreateAssetMenu(fileName = "NewSituation", menuName = "GermanGame/Situation")]
public class SituationData : ScriptableObject
{
    [Header("The Scene")]
    [TextArea(3, 10)]
    public string questionText; // e.g., "Ich möchte ____ Brot kaufen."
    
    [Header("Choices")]
    public string[] answers;      // e.g., ["ein", "eine", "einen"]
    public int correctIndex;      // The index of the right answer (0, 1, or 2)

    [Header("Feedback")]
    public string successMessage; // What Herr Schmidt says if you're right
    public string failMessage;    // What Herr Schmidt says if you're wrong
}