using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : UnitData
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

    GameObject enemy;

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

    private Transform ResPosition;
    

    [SerializeField] Transform bulletPosition;

    private float fireTimer = 0;

    public float shootRange = 500f;

    public LayerMask enemyLayer;

    GameObject deadEffect;

    public WeaponController()
    {
        totalData = new Player_InitStatus();
        myData = new Player_InitStatus();
    }

    private void Awake()
    {
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
    }
    void Start()
    {
        enemyPosition = null;
        cameraTransform = Camera.main.transform;
        player = GetComponent<Player>();
        missileCnt = 8f;
        bulletCnt = 150f;
        fireRange = 400f;
    }
    private void Update()
    {
        if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 500, gameObject.transform.position.z), shootRange, Vector3.down, out RaycastHit hit, 1000, enemyLayer))
        {
            enemy = hit.collider.gameObject; //GameObject.FindWithTag("Enemy").transform;
            Debug.Log(enemy);
        }
        Die();
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000.0f, Color.green);

        RaycastHit temp;

        if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.forward, out temp, 1000.0f, (-1) - (1 << 6) & (-1) - (1 << 13) & (-1) - (1 << 14))) // 카메라의 위치에서 카메라가 바라보는 정면으로 레이를 쏴서 충돌확인
        {
            // 충돌이 검출되면 총알의 리스폰포인트(firePos)가 충돌이 발생한위치를 바라보게 만든다. 
            // 이 상태에서 발사입력이 들어오면 총알은 충돌점으로 날아가게 된다.
            
            bulletPosition.LookAt(temp.point);
            //bullet = new Bullet(temp.point, false);
            Debug.DrawRay(bulletPosition.position, bulletPosition.forward * 1000.0f, Color.red); // 이 레이는 앞서 선언한 디버그용 레이와 충돌점에서 교차한다
        }
        else
        {
            Quaternion dir = cameraTransform.rotation;
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
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                enemyPosition = enemy.transform;

                if (distanceToEnemy <= shootRange) // 사격 범위 내에 있을 때
                {
                    Debug.Log($"enemy : {hit.collider.name}");
                    LaunchMissile();
                    if (missileCnt % 2 == 1)
                    {
                        if (missileCnt == 7)
                            gameObject.transform.GetChild(0).GetChild(7).gameObject.SetActive(false);
                        if (missileCnt == 5)
                            gameObject.transform.GetChild(0).GetChild(8).gameObject.SetActive(false);
                        if (missileCnt == 3)
                            gameObject.transform.GetChild(0).GetChild(9).gameObject.SetActive(false);
                        if (missileCnt == 1)
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
            }
        }
        else // 사격 범위 밖에 있을 때
        {
            enemy = null;
        }
        if (enemy != null)
            if (enemy.activeInHierarchy == false)
            {
                enemy = null;
            }
           
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        MissileCooldown(ref rightMslCooldown);
        MissileCooldown(ref leftMslCooldown);
    }

    private void Refresh()
    {
        totalData = new Unit_Status();
        totalData += myData;
        
        EventManager.instance.PostEvent("UI:HPValue", totalData.currentHp / totalData.maxHp);
    }
    private void MainGunFire()
    {
        if (bulletCnt > 0)
        {
            player.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            GameObject go = Resources.Load<GameObject>("Prefabs/Bullet");
            GameObject bullet = Instantiate(go, bulletPosition.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0), Quaternion.identity);
            
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
        missileScript.Launch(enemyPosition, player.speed + 15);

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

    private void Die()
    {
        if (totalData.currentHp <= 0)
        {
            gameObject.SetActive(false);
            GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);
            
            Destroy(go, 3);
            gameObject.transform.rotation = Quaternion.identity;
        }
    }

    public override void PostHit(UnitData data, RaycastHit hit)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 15)
        {
            myData.currentHp -= 1000f;
            Debug.Log(myData.currentHp);
            
            Refresh();
        }
    }

    public IEnumerator Respawn()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
    }

    public override void SetHit(UnitData data)
    {
        
    }
}