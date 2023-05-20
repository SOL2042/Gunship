using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform bulletPosition;

    Player player;

    public List<GameObject> bulletPool = new List<GameObject>();

    public int bulletCnt = 50;

    private float fireTimer = 0;
    private GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        bullet = Resources.Load("Prefabs/Bullet") as GameObject;

    }

    // Update is called once per frame
    void Update()
    {
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
