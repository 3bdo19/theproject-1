using System.Threading;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

 public static UI instance;   

 [SerializeField]private GameObject gameOverUI;



 private int KillCount;


    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }

    private void Update()
    {
    
    }

    public void AddKillCount()
    {

    }

    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void EnableGameOverUI()
    {    
      gameOverUI.SetActive(true);
    }
}
