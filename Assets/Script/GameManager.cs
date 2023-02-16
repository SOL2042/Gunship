using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> bulletPool = new List<GameObject>();

    public int bulletCnt = 50;

    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        bullet = Resources.Load("Prefabs/Bullet") as GameObject;

        CreatBulletPool();

        Transform bulletSpawnPoint = GameObject.Find("Gunship/Gunship/BulletPosition")?.transform;

        InvokeRepeating("CreatBullet",0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void CreatBulletPool()
    {
        

        
    }

    private void CreatBullet()
    {
        Instantiate(bullet);
    }
}
