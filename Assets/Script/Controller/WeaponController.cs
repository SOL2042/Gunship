using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    #region Singleton
    private static WeaponController _instance;
    public static WeaponController instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<WeaponController>();
            return _instance;
        }
    }
    #endregion

    Bullet bullet;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform enemyPosition;

    [SerializeField]
    Transform rightMissilePosition;
    [SerializeField]
    Transform leftMissilePosition;

    public float missileCnt;
    public float missileCooldownTime;

    float rightMslCooldown;
    float leftMslCooldown;

    public float bulletCnt;
   
    public float gunRPM;

    public float fireRange;

    Player player;
    // Weapon Inputs
    
    public GameObject missilePrefab;
    // Weapon Callbacks


    [SerializeField] Transform bulletPosition;

    private float fireTimer = 0;


    void Start()
    {
        enemyPosition = null;
        cameraTransform = Camera.main.transform;
        player = GetComponent<Player>();
        missileCnt = 8f;
        bulletCnt = 150f;
        fireRange = 300f;
    }
    private void Update()
    {
       
        if (EnemyController.instance.t90s.Count == 0)
        {
            enemyPosition = null;
        }
        else
        {
            enemyPosition = GameObject.FindWithTag("Enemy").transform;
        }
        

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000.0f, Color.green);

        RaycastHit temp;

        if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.forward, out temp, 1000.0f, (-1) - (1 << 6))) // 카메라의 위치에서 카메라가 바라보는 정면으로 레이를 쏴서 충돌확인
        {
            // 충돌이 검출되면 총알의 리스폰포인트(firePos)가 충돌이 발생한위치를 바라보게 만든다. 
            // 이 상태에서 발사입력이 들어오면 총알은 충돌점으로 날아가게 된다.
            
            //Debug.Log(temp.point);
            bulletPosition.LookAt(temp.point);
            //bullet = new Bullet(temp.point, false);
            Debug.DrawRay(bulletPosition.position, bulletPosition.forward * 1000.0f, Color.red); // 이 레이는 앞서 선언한 디버그용 레이와 충돌점에서 교차한다
        }
        else
        {
            Quaternion dir = cameraTransform.rotation * Quaternion.Euler(-7, 0, 0);
            bulletPosition.rotation = dir;
        }

        if (Input.GetMouseButton(0))
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= 0.1f)
            {
                MainGunFire();
                fireTimer = 0f;
            }
        }
        else
        {
            CeaseFire();
            fireTimer = 0.09f;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            LaunchMissile();
            if (missileCnt % 2 == 1)
            {
                if(missileCnt == 7)
                gameObject.transform.GetChild(0).GetChild(7).gameObject.SetActive(false);
                if(missileCnt == 5)
                gameObject.transform.GetChild(0).GetChild(8).gameObject.SetActive(false);
                if(missileCnt == 3)
                gameObject.transform.GetChild(0).GetChild(9).gameObject.SetActive(false);
                if(missileCnt == 1)
                gameObject.transform.GetChild(0).GetChild(10).gameObject.SetActive(false);

            }
            else
            {
                if (missileCnt == 6)
                    gameObject.transform.GetChild(0).GetChild(11).gameObject.SetActive(false);
                if (missileCnt == 4)
                    gameObject.transform.GetChild(0).GetChild(12).gameObject.SetActive(false);
                if (missileCnt == 2)
                    gameObject.transform.GetChild(0).GetChild(13).gameObject.SetActive(false);
                if (missileCnt == 0)
                    gameObject.transform.GetChild(0).GetChild(14).gameObject.SetActive(false);
            }
           
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        MissileCooldown(ref rightMslCooldown);
        MissileCooldown(ref leftMslCooldown);
    }


    private void MainGunFire()
    {
        if (bulletCnt > 0)
        {
            player.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            GameObject go = Resources.Load<GameObject>("Prefabs/Bullet");
            GameObject bullet = Instantiate(go, bulletPosition.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0), Quaternion.identity);
            bullet.layer = 6;
            bulletCnt--;
            Destroy(bullet, 3);
        }
        else
        {
            CeaseFire();
        }
    }

    private void CeaseFire()
    {
        gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    }

    private void LaunchMissile()
    {
        Vector3 missilePosition;
        if (missileCnt <= 0)
            return;

        if (leftMslCooldown > 0 && rightMslCooldown > 0)
        {
            // Beep sound
            return;
        }

        if (missileCnt % 2 == 1)
        {
            missilePosition = rightMissilePosition.position;
            rightMslCooldown = missileCooldownTime;
        }
        else
        {
            missilePosition = leftMissilePosition.position;
            leftMslCooldown = missileCooldownTime;
        }
        
        GameObject missile = Instantiate(missilePrefab, missilePosition, transform.rotation);
        HellFire_Missile missileScript = missile.GetComponent<HellFire_Missile>();
        missileScript.Launch(enemyPosition, player.speed + 15, gameObject.layer);

        missileCnt--;
    }

    void MissileCooldown(ref float cooldown)
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0) cooldown = 0;
        }
        else return;
    }

    private void Reload()
    {
        if(missileCnt <= 0)
        {
           missileCnt = 8;

           for (int i = 7; i < 15; i++)
           {
               gameObject.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
           }
        }
        if(bulletCnt <= 0)
        {
            bulletCnt = 150;
        }
    }
}