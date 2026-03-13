using UnityEngine;

[CreateAssetMenu(fileName = "NewSituation", menuName = "GermanGame/Situation")]
public class SituationData : ScriptableObject
{
    [Header("The Scene")]
    [TextArea(3, 10)]
    public string questionText; 
    
    [Header("Choices")]
    public string[] answers;      
    public int correctIndex;      

    [Header("Feedback")]
    public string successMessage; 
    public string failMessage;    
}