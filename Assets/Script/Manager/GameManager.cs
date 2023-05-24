using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform RespwanPoint;

    Player player;
    private float respwanInterval = 4f;
    private float lastRespwanTime = 0f;

    private bool isPause;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause == false)
            {
                Time.timeScale = 0;
                UI_Manager.instance.Pause();
                isPause = true;
            }
            else
            {
                Time.timeScale = 1;
                UI_Manager.instance.pauseUI.SetActive(false);
                isPause = false;
            }
        }
        if (isPause == false)
        {
            if (USBase.instance.gameObject.activeInHierarchy != true)
            {
                Time.timeScale = 0;
                UI_Manager.instance.Defeat();
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        else
        {

        }

        if (player.gameObject.activeInHierarchy == false)
        {
            lastRespwanTime += Time.deltaTime;
            UI_Manager.instance.sucideUI.SetActive(false);
            if (lastRespwanTime >= respwanInterval)
            { 
                player.GetComponent<WeaponController>().totalData.currentHp = player.GetComponent<WeaponController>().totalData.maxHp;
                player.GetComponent<WeaponController>().missileCnt = 8;
                player.GetComponent<WeaponController>().rocketCnt = 38;
                player.GetComponent<WeaponController>().bulletCnt = 150;
                player.throttle = 80f;
                for (int i = 7; i < 15; i++)
                {
                    player.gameObject.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                }
                player.transform.position = RespwanPoint.position;
                player.GetComponent<Player>().rgb.velocity = Vector3.zero;
                player.GetComponent<Player>().rgb.freezeRotation = true;
                player.gameObject.SetActive(true);
                if(player.gameObject.activeInHierarchy)
                {
                    player.GetComponent<Player>().rgb.freezeRotation = false;
                }
            }
        }
        else
        {
            lastRespwanTime = 0;
        }
        //if(player.gameObject.activeInHierarchy == false)
        //{
        //    StartCoroutine(player.GetComponent<WeaponController>().Respawn());
        //}
        //else
        //{
        //    StopCoroutine(player.GetComponent<WeaponController>().Respawn());
        //}
    }

    
   
}
