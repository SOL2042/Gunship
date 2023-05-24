using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform RespwanPoint;

    Player player;
    private float respwanInterval = 4f;
    private float lastRespwanTime = 0f;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
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

        if(player.gameObject.activeInHierarchy == false)
        {
            lastRespwanTime += Time.deltaTime;
            if (lastRespwanTime >= respwanInterval)
            { 
                player.GetComponent<WeaponController>().totalData.currentHp = player.GetComponent<WeaponController>().totalData.maxHp;
                player.GetComponent<WeaponController>().missileCnt = 8;
                player.GetComponent<WeaponController>().rocketCnt = 38;
                player.GetComponent<WeaponController>().bulletCnt = 150;
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
