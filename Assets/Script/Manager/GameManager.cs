using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform RespwanPoint;
    [SerializeField] Transform allyRespwan;
    Player player;
    private float respwanInterval = 4f;
    private float lastRespwanTime = 0f;

    private bool isPause;
    private bool isDefeat;
    private bool viewInventory = false;

    GameObject t54;
    private List<GameObject> t54s;

    private float level = 1;
    [SerializeField]
    private float randomX;
    private float maxTankCnt = 10f;

    void Start()
    {
        player = FindObjectOfType<Player>();
        t54s = new List<GameObject>();
        t54 = Resources.Load("Prefabs/T54") as GameObject;
        Instantiate();
    }
    void Update()
    {
        UIControll();
        PlayerRespwan();
        CreateUnit();
        randomX = Random.Range(-20, 80);
    }
    private void ViewInventoryUI(bool value)
    {
        viewInventory = value;
        UI_Manager.instance.inventory.SetActive(value);
        if(viewInventory)
        {
            
        }

    }


    private void PlayerRespwan()
    {
        if (player.gameObject.activeInHierarchy == false)
        {
            lastRespwanTime += Time.deltaTime;
            UI_Manager.instance.sucideUI.SetActive(false);
            if (lastRespwanTime >= respwanInterval)
            {
                player.GetComponent<WeaponController>().totalData.currentHp = player.GetComponent<WeaponController>().totalData.maxHp;
                player.GetComponent<WeaponController>().totalData.flyMode = FlyMode.Default;
                player.GetComponent<WeaponController>().missileCnt = 8;
                player.GetComponent<WeaponController>().rocketCnt = 38;
                player.GetComponent<WeaponController>().bulletCnt = 150;

                UI_Manager.instance.AAMissileRadarUI.SetActive(false);
                player.throttle = 80f;
                for (int i = 7; i < 15; i++)
                {
                    player.gameObject.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                }
                player.transform.position = RespwanPoint.position;
                player.GetComponent<Player>().rgb.velocity = Vector3.zero;
                player.GetComponent<Player>().rgb.freezeRotation = true;
                player.gameObject.SetActive(true);
                if (player.gameObject.activeInHierarchy)
                {
                    player.GetComponent<Player>().rgb.freezeRotation = false;
                }
            }
        }
        else
        {
            lastRespwanTime = 0;
        }
    }
    private void UIControll()
    {
        if (isDefeat != true)
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
                    UI_Manager.instance.Pause();
                    isPause = false;
                }
            }
        }
        if (isPause != true)
        {
            if (USBase.instance.gameObject.activeInHierarchy != true)
            {
                Time.timeScale = 0;
                UI_Manager.instance.Defeat();
                isDefeat = true;
            }
            else
            {
                isDefeat = false;
                Time.timeScale = 1;
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    ViewInventoryUI(!viewInventory);
                }
            }

        }
        else
        {
            
        }
    }

    private void CreateUnit()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            int unActivedUnit = -1;                                         // ����ִ� ĭ

            for (int i = 0; i < t54s.Count; i++)                   // ������� ����ĭ�� ����ִ��� ���ִ��� ���� Ȯ���ϴ� �ݺ��� 
            {
                if (t54s[i].activeInHierarchy != true)                 // unitCell[i]�� �ڵ尡 ������
                {
                    if (unActivedUnit == -1) unActivedUnit = i;                 // blankCell�� -1 �̸� i�� ����
                }
                else                                                    // unitCell[i]�� �ڵ尡 ������
                {
                    
                }
            }

            if (unActivedUnit != -1)                                        // blankCell�� �ռ� Ȯ���� i��
            {
                t54s[unActivedUnit].SetActive(true);
            }
            else                                                        // blankCell�� -1�̸�
            {
                
            }
        }

    }

    private void Instantiate()
    {
        for (int i = 0; i < maxTankCnt; i++)
        {
            t54s.Add(Instantiate(t54, allyRespwan));
            t54s[i].SetActive(false);
            t54s[i].transform.position = new Vector3(t54s[i].transform.position.x + randomX, t54s[i].transform.position.y, t54s[i].transform.position.z);
        }


        
    }

}
