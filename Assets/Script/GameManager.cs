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

        CreatBulletPool();

        Transform bulletSpawnPoint = GameObject.Find("Gunship/Gunship/BulletPosition")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        bulletPosition = player.transform.Find("BulletPosition");
        //if (Input.GetMouseButton(0))
        //{
        //    fireTimer += Time.deltaTime;
        //    if (fireTimer >= 0.1f)
        //    {
        //        CreatBullet();
        //        fireTimer = 0f;
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < bulletCnt; i++)
        //    {
        //        if (bulletPool[i].activeInHierarchy == true)
        //        {
        //            bulletPool[i].SetActive(false);
        //            bulletPool[i].transform.position = bulletPosition.position;
        //        }

        //    }
        //}
    }


    private void CreatBulletPool() // ÃÑ¾Ë »ý¼º
    {
        for(int i = 0; i < bulletCnt; i++) //
        {
            bulletPool.Add(Instantiate(bullet, bulletPosition.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0), player.transform.rotation));
            
            bulletPool[i].layer = 6;
            bulletPool[i].SetActive(false);
        }
    }


    private int poolIndex = 0;
    private void CreatBullet()
    {
        if (!bulletPool[poolIndex].activeInHierarchy)
        {
            bulletPool[poolIndex].SetActive(true);
            poolIndex++;
        }
        else
        {
            poolIndex++;
            CreatBullet();
        }

        if (bulletPool.Count - 1 >= poolIndex)
            poolIndex = 0;

        return;

        for (int i = 0; i < bulletCnt; i++)
        {
            if (fireTimer >= 0.1f)
            {
                bulletPool[i].SetActive(true);
                
            }
            else
            {
                fireTimer = 0.09f;
            }

        }
    }
}
