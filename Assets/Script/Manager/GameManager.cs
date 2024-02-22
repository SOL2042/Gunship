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

    [SerializeField]
    private float randomX;
    private float maxTankCnt = 20f;
    private void FixedUpdate()
    {
        randomX = Random.Range(-20, 80);
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        player = FindObjectOfType<Player>();
        t54s = new List<GameObject>();
        t54 = Resources.Load("Prefabs/T54") as GameObject;
        Instantiate();
        InventoryManager.instance.AddUnit(new T54_InitStatus(), 1000);
        InventoryManager.instance.AddUnit(new T90_InitStatus(), 2000);
    }
    void Update()
    {
        UIControll();
        PlayerRespwan();
        if(viewInventory)
        CreateUnit();
        for (int i = 0; i < t54s.Count; i++)
        {
            if (!t54s[i].activeInHierarchy)
                t54s[i].transform.position = new Vector3(randomX, t54s[i].transform.position.y, t54s[i].transform.position.z);
        }
        if (!SoundManager.instance.audioSourceBgm.isPlaying)
        {
            SoundManager.instance.PlayBgm($"{Random.Range(1, 35)}");
        }
    }
    private void ViewInventoryUI(bool value)
    {
        viewInventory = value;
        UI_Manager.instance.inventory.SetActive(value);
    }

    private void PlayerRespwan()
    {
        if (player.gameObject.activeInHierarchy == false)
        {
            lastRespwanTime += Time.deltaTime;
            UI_Manager.instance.sucideUI.SetActive(false);
            if (lastRespwanTime >= respwanInterval)
            {
                player.GetComponent<WeaponController>().myData.currentHp = player.GetComponent<WeaponController>().totalData.maxHp;
                player.GetComponent<WeaponController>().myData.flyMode = FlyMode.Default;
                player.GetComponent<WeaponController>().missileCnt = 8;
                player.GetComponent<WeaponController>().rocketCnt = 38;
                player.GetComponent<WeaponController>().bulletCnt = 150;
                player.GetComponent<WeaponController>().enemy = null;
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
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                    UI_Manager.instance.Pause();
                    isPause = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
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
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
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
    }

    private void CreateUnit()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            int unActivedUnit = -1;                               

            for (int i = 0; i < t54s.Count; i++)                  
            {
                if (t54s[i].activeInHierarchy != true)            
                {
                    if (unActivedUnit == -1) unActivedUnit = i;   
                }
            }

            if (unActivedUnit != -1)                              
            {
                if (UI_Manager.instance.credit >= 1000)
                {
                    t54s[unActivedUnit].SetActive(true);
                    InventoryManager.instance.UseUnit(0);
                }
            }
        }

    }
    private void Instantiate()
    {
        for (int i = 0; i < maxTankCnt; i++)
        {
            t54s.Add(Instantiate(t54, allyRespwan));
            t54s[i].SetActive(false);
        }
    }
}
