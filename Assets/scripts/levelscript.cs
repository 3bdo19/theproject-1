using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class levelscript : MonoBehaviour
{
public void openlevel(int levelid)
{
    // This loads the scene using the Index number from Build Settings
    SceneManager.LoadScene(levelid); 
}

public void LoadMainMenu()
{
    // Index 0 is almost always the Main Menu in Build Settings
    UnityEngine.SceneManagement.SceneManager.LoadScene(1); 
}
}
