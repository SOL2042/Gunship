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
            SoundManager.instance.PlayBgm("16");
        });
        EventManager.instance.AddListener("OpenInGameScene", (e) =>
        {
            SceneManager.LoadScene("InGameScene");
            SoundManager.instance.PlayBgm($"{Random.Range(1,35)}");
        });

    }

    private void OpenScene(string sceneName)
    {
        
    }

    
}
