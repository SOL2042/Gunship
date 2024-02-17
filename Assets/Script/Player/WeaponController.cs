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
    Transform rocketSpawnPoint; // ���� �߻� ��ġ
    [SerializeField]
    LayerMask rocketTargetLayer; // �������� ������ ���̾�
    [SerializeField]
    RectTransform aimPointUI; // UI�� ǥ�õ� ������

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
            // �������� ���̾ �¾��� �� UI�� ������ ǥ��
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

        if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.forward, out temp, 1000.0f, (-1) - (1 << 6) & (-1) - (1 << 13) & (-1) - (1 << 14))) // ī�޶��� ��ġ���� ī�޶� �ٶ󺸴� �������� ���̸� ���� �浹Ȯ��
        {
            // �浹�� ����Ǹ� �Ѿ��� ����������Ʈ(firePos)�� �浹�� �߻�����ġ�� �ٶ󺸰� �����. 
            // �� ���¿��� �߻��Է��� ������ �Ѿ��� �浹������ ���ư��� �ȴ�.
            
            bulletPosition.LookAt(temp.point);
            //bullet = new Bullet(temp.point, false);
            Debug.DrawRay(bulletPosition.position, bulletPosition.forward * 1000.0f, Color.red); // �� ���̴� �ռ� ������ ����׿� ���̿� �浹������ �����Ѵ�
        }
        else
        {
            Quaternion dir = cameraTransform.rotation;
            bulletPosition.rotation = dir;
        }

        if (Input.GetMouseButton(0))    //���� �߻� 
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

        if(Input.GetKeyDown(KeyCode.Space)) // ���� �߻� 
        {
            LaunchRocket();
        }

        if (Input.GetKeyDown(KeyCode.G))    // �̻��� �߻�
        {
            if (enemy != null)  // ���� ������
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); // �÷��̾�� ������ �Ÿ��� 

                enemyPosition = enemy.transform;    // ���� ��ġ

                if (distanceToEnemy <= shootRange) // ��ݹ��� ���� ���� ��
                {
                    LaunchMissile();                // �̻��� �߻� �Լ�

                    if (missileCnt % 2 == 1)        // ��⿡ �޷��ִ� �翷�� �̻��� ������Ʈ ������ ����
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
                else                                    // �Ÿ����� ���� ���� ��� null�� �Ѱ���
                {
                    enemy = null;
                }
            }
        }
        if (enemy != null)                              //���� null���� �ƴѵ� ���̾��Ű�� Ȱ��ȭ ���°� �ƴҰ�� ���� null�� ����
            if (enemy.activeInHierarchy == false)
            {
                enemy = null;
            }

        if (Input.GetKeyDown(KeyCode.F))                    // ����̻��� ���̴� Ű�� (���� ���)
        {
            UI_Manager.instance.AAMissileRadar();
        }
        if (Input.GetKey(KeyCode.J))                        // �ڻ�� �ڵ�
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

    private void Die()                          // �÷��̾� ���� ��� �Լ�
    {
        if (myData.currentHp <= 0)
        {
            gameObject.SetActive(false);
            GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);
            Destroy(go, 3);
            gameObject.transform.rotation = Quaternion.identity;
        }
    }

    private void Suicide()                      // �ڻ� �Լ�
    {
        myData.currentHp -= totalData.maxHp;
        Refresh();
    }
    public override void PostHit(WeaponData data)
    {
        myData.currentHp -= data.damage;
        Refresh();
    }

    private void OnTriggerEnter(Collider other)         // Ʈ���� �浹 �Լ�            
    {
        if(other.GetComponent<MI24_Bullet>())                //other�� ���̾ EnemyBullet�� ���
        {
            PostHit(other.GetComponent<MI24_Bullet>());
            Debug.Log(myData.currentHp);
            Debug.Log(totalData.currentHp);
        }
        if (other.GetComponent<TankBullet>())                //other�� ���̾ EnemyBullet�� ���
        {
            PostHit(other.GetComponent<TankBullet>());
            Debug.Log(myData.currentHp);
            Debug.Log(totalData.currentHp);
        }
        if (other.gameObject.layer == 9)
        {
            if (Player.instance.rgb.velocity.x >= 20f || Player.instance.rgb.velocity.x <= -20f)            //�÷��̾��� �ӵ��� ���� ������ �������� ����
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