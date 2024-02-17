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

    [SerializeField]
    Transform rocketSpawnPoint; // 로켓 발사 위치
    [SerializeField]
    LayerMask rocketTargetLayer; // 레이저가 맞춰질 레이어
    [SerializeField]
    RectTransform aimPointUI; // UI로 표시될 조준점

    [SerializeField]
    LayerMask missileTargetLayer;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform enemyPosition;

    [SerializeField]
    Transform rightMissilePosition;
    [SerializeField]
    Transform leftMissilePosition;

    [SerializeField]
    Transform rightRocketPosition;
    [SerializeField]
    Transform leftRocketPosition;

    GameObject enemy;

    public float missileCnt;
    public float missileCooldownTime;

    float rightMslCooldown;
    float leftMslCooldown;

    public float bulletCnt;
    public float rocketCnt;
    public float rocketCooldownTime;
    float rightRckCooldown;
    float leftRckCooldown;

    public float reloadMissileTimer;
    public float reloadRocketTimer;
    public float reloadBulletTimer;
    public float reloadInterval = 5f;

    public float suicideTimer = 3f;
    public float suicideInterval = 0f;
    Player player;
    // Weapon Inputs
    
    public GameObject missilePrefab;
    // Weapon Callbacks

    public GameObject rocketPrefab;


    [SerializeField] Transform bulletPosition;

    private float fireTimer = 0;

    public float shootRange = 500f;

    public LayerMask enemyLayer;

    GameObject deadEffect;

    float boresightAngle = 60f;

    float bulletDamage = 50f;
    float rocketDamage = 200f;
    float missileDamage = 1000f;

    Vector3 enemyTransform;
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
        rocketCnt = 38f;
        bulletCnt = 150f;
    }
    private void Update()
    {
        Ray rocketRay = new Ray(rocketSpawnPoint.position, rocketSpawnPoint.forward);
        RaycastHit rocketRayhit;

        if (Physics.Raycast(rocketRay, out rocketRayhit, Mathf.Infinity, rocketTargetLayer))
        {
            // 레이저가 레이어에 맞았을 때 UI로 조준점 표시
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(rocketRayhit.point);
            aimPointUI.position = screenPoint;
        }


        if (Physics.BoxCast(cameraTransform.transform.position, transform.lossyScale / 2, cameraTransform.transform.forward, out RaycastHit hit, cameraTransform.rotation, 400f, missileTargetLayer))
        {
            enemy = hit.collider.gameObject;
        }

        if (enemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > shootRange)
            {
                enemy = null;
            }
            else
            {
                if (enemy.activeInHierarchy == true)
                {
                    enemyTransform = enemy.transform.position;
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(enemyTransform);
                    UI_Manager.instance.MissileTarget();
                    UI_Manager.instance.hellFire_MissileTargetUI.transform.position = screenPosition;
                }
                else
                {
                    UI_Manager.instance.hellFire_MissileTargetUI.SetActive(false);
                }
            }
        }
        else
        {
            UI_Manager.instance.hellFire_MissileTargetUI.SetActive(false);
        }

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

        if (Input.GetMouseButton(0))    //기총 발사 
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

        if(Input.GetKeyDown(KeyCode.Space)) // 로켓 발사 
        {
            LaunchRocket();
        }

        if (Input.GetKeyDown(KeyCode.G))    // 미사일 발사
        {
            if (enemy != null)  // 적이 있으면
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); // 플레이어와 적과의 거리값 

                enemyPosition = enemy.transform;    // 적의 위치

                if (distanceToEnemy <= shootRange) // 사격범위 내에 있을 때
                {
                    LaunchMissile();                // 미사일 발사 함수

                    if (missileCnt % 2 == 1)        // 헬기에 달려있는 양옆의 미사일 오브젝트 번갈아 끄기
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
                else                                    // 거리내에 적이 없을 경우 null을 넘겨줌
                {
                    enemy = null;
                }
            }
        }
        if (enemy != null)                              //적이 null값이 아닌데 하이어라키에 활성화 상태가 아닐경우 적을 null로 만듬
            if (enemy.activeInHierarchy == false)
            {
                enemy = null;
            }

        if (Input.GetKeyDown(KeyCode.F))                    // 대공미사일 레이더 키기 (구현 요망)
        {
            UI_Manager.instance.AAMissileRadar();
        }
        if (Input.GetKey(KeyCode.J))                        // 자살용 코드
        {
            UI_Manager.instance.Sucide();
            Debug.Log($"Suicide : {suicideTimer}");
            suicideTimer -= Time.deltaTime;
            if (suicideTimer <= suicideInterval)
            {
                UI_Manager.instance.sucideUI.SetActive(false);
                Suicide();
                suicideTimer = 3f;
            }
        }
        else
        {
            UI_Manager.instance.sucideUI.SetActive(false);
            suicideTimer = 3f;
        }
       
        Die();
        Reload();
        
        MissileCooldown(ref rightMslCooldown);
        MissileCooldown(ref leftMslCooldown);
        RocketCooldown(ref rightRckCooldown);
        RocketCooldown(ref leftRckCooldown);
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
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Speed(player.speed + 15);
            bulletScript.Damage(ref bulletDamage); 

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
        missileScript.Damage(ref missileDamage);
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

    private void LaunchRocket()
    {
        Vector3 rocketPosition;
        if (rocketCnt <= 0)
            return;

        if (leftRckCooldown > 0 && rightRckCooldown > 0)
        {
            // Beep sound
            return;
        }
        if (rocketCnt % 2 == 1)
        {
            rocketPosition = rightRocketPosition.position;
            rightRckCooldown = rocketCooldownTime;
        }
        else
        {
            rocketPosition = leftRocketPosition.position;
            leftRckCooldown = rocketCooldownTime;
        }

        GameObject rocket = Instantiate(rocketPrefab, rocketPosition, transform.rotation);
        Rocket rocketScript = rocket.GetComponent<Rocket>();
        rocketScript.Launch(player.speed + 15);
        rocketScript.Damage(ref rocketDamage);
        rocketCnt--;
    }

    void RocketCooldown(ref float cooldown)
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
        if (missileCnt <= 0)
        {
            reloadMissileTimer += Time.deltaTime;
            if (reloadMissileTimer >= reloadInterval)
            {
                missileCnt = 8;

                for (int i = 7; i < 15; i++)
                {
                    gameObject.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            reloadMissileTimer = 0;
        }
        if (rocketCnt <= 0)
        {
            reloadRocketTimer += Time.deltaTime;
            if (reloadRocketTimer >= reloadInterval)
            {
                rocketCnt = 38;
            }
        }
        else
        {
            reloadRocketTimer = 0;
        }
        if (bulletCnt <= 0)
        {
            reloadBulletTimer += Time.deltaTime;
            if (reloadBulletTimer >= reloadInterval)
            {
                bulletCnt = 150;
            }
        }
        else
        {
            reloadBulletTimer = 0;
        }
    }

    private void Die()                          // 플레이어 죽을 경우 함수
    {
        if (myData.currentHp <= 0)
        {
            gameObject.SetActive(false);
            GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);
            Destroy(go, 3);
            gameObject.transform.rotation = Quaternion.identity;
        }
    }

    private void Suicide()                      // 자살 함수
    {
        myData.currentHp -= totalData.maxHp;
        Refresh();
    }
    public override void PostHit(WeaponData data)
    {
        myData.currentHp -= data.damage;
        Refresh();
    }

    private void OnTriggerEnter(Collider other)         // 트리거 충돌 함수            
    {
        if(other.GetComponent<MI24_Bullet>())                //other의 레이어가 EnemyBullet일 경우
        {
            PostHit(other.GetComponent<MI24_Bullet>());
            Debug.Log(myData.currentHp);
            Debug.Log(totalData.currentHp);
        }
        if (other.GetComponent<TankBullet>())                //other의 레이어가 EnemyBullet일 경우
        {
            PostHit(other.GetComponent<TankBullet>());
            Debug.Log(myData.currentHp);
            Debug.Log(totalData.currentHp);
        }
        if (other.gameObject.layer == 9)
        {
            if (Player.instance.rgb.velocity.x >= 20f || Player.instance.rgb.velocity.x <= -20f)            //플레이어의 속도에 따라 터질지 안터질지 구분
            {
                myData.currentHp -= 1000f;
            }
            else if (Player.instance.rgb.velocity.y > 20f || Player.instance.rgb.velocity.y <= -20f)
            {
                myData.currentHp -= 1000f;
            }
            else if (Player.instance.rgb.velocity.z > 20f || Player.instance.rgb.velocity.z <= -20f)
            {
                myData.currentHp -= 1000f;
            }
        }
        Refresh();
    }
    
  
}