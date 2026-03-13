using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class levelscript : MonoBehaviour
{
public void openlevel(int levelid)
{
    SceneManager.LoadScene(levelid); 
}

public void LoadMainMenu()
{
    UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
}
}
