using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    
    private void Awake()
    {
        
        EventManager.instance.AddListener("OpenSettingRoomScene", (e) => 
        {
            SceneManager.LoadScene("SettingRoomScene");
        });
        EventManager.instance.AddListener("OpenMainMenuScene", (e) =>
        {
            SceneManager.LoadScene("MainMenuScene");
        });
        EventManager.instance.AddListener("OpenInGameScene", (e) =>
        {
            SceneManager.LoadScene("InGameScene");
        });

    }

    private void OpenScene(string sceneName)
    {
        
    }

    
}
