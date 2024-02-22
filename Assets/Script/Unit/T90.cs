using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : UnitData
{
    T90 t90;
    TankBullet tankBullet;
    GameObject deadEffect;
    GameObject bullet;

    [SerializeField] private float moveSpeed = 20f; // 이동 속도
    //[SerializeField] private float turnSpeed = 100f; // 회전 속도
    //[SerializeField] private float turretTurnSpeed = 70f; // 회전 속도
    [SerializeField] private float shootRange = 100f;
    [SerializeField] private float shootInterval = 4f; // 사격 간격
    [SerializeField] private Transform turretTransform; // 포탑 Transform 컴포넌트

    private Rigidbody tankRigidbody; // 탱크의 Rigidbody 컴포넌트
    private float lastShootTime; // 마지막 사격 시간
    [SerializeField] private Transform bulletPosition;
    T90_InitStatus t90_InitStatus;
    
    private int score = 100;
    private int credit = 200;
    public int killCount = 0;
    private GameObject enemy;

    public LayerMask enemyLayer;

    private float damage = 200;

    [SerializeField] GameObject unitHpUI;
    public T90()
    {
        totalData = new T90_InitStatus();
        myData = new T90_InitStatus();
    }
    private void Start()
    {
        t90 = new T90();
        
        EventManager.instance.AddListener("addTankScore", (p)=>
        {
            Score();
        });
        EventManager.instance.AddListener("addTankCredit", (p) =>
        {
            Credit();
        });
        EventManager.instance.AddListener("addKillCount", (p) =>
        {
            KillCount();
        });
        bulletPosition = transform.GetChild(16).GetChild(0).GetChild(0).transform;
        bullet = Resources.Load<GameObject>("Prefabs/T90Bullet");
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
        tankRigidbody = GetComponent<Rigidbody>(); // 탱크의 Rigidbody 컴포넌트 가져오기
        turretTransform = transform.GetChild(16).transform;
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 40, gameObject.transform.position.z), shootRange, Vector3.down, out RaycastHit hit, 1000, enemyLayer))
        {
            enemy = hit.collider.gameObject; 
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            turretTransform.LookAt(enemy.transform);

            if (distanceToEnemy <= shootRange) // 사격 범위 내에 있을 때
            {
                moveSpeed = 0;

                if (Time.time - lastShootTime >= shootInterval) // 사격 간격이 지난 경우
                {
                    Shoot(); // 사격
                    lastShootTime = Time.time; // 마지막 사격 시간 갱신
                }
            }
            else
            {
                moveSpeed = 20;
                Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                tankRigidbody.MovePosition(tankRigidbody.position + movement);
            }
        }
        else // 사격 범위 밖에 있을 때
        {
            enemy = null;
            if (enemy == null)
            {
                moveSpeed = 20f;
                Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                tankRigidbody.MovePosition(tankRigidbody.position + movement);
            }
        }
        if(enemy != null)
            if (enemy.activeInHierarchy == false)
            {
                enemy = null;
            }
    }
    private void Shoot()
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/T90Bullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        bullet.transform.rotation = bulletPosition.transform.rotation;
        tankBullet = bullet.GetComponent<TankBullet>();
        tankBullet.Damage(ref damage);
        Destroy(bullet, 3);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TankBullet>())
        {
            PostHit(other.GetComponent<TankBullet>());
        }
        if (other.gameObject.GetComponent<Bullet>())
        {
            PostHit(other.GetComponent<Bullet>());
        }
        if (other.gameObject.GetComponent<Rocket>())
        {
            PostHit(other.GetComponent<Rocket>());
        }
        if (other.gameObject.GetComponent<HellFire_Missile>())
        {
            PostHit(other.GetComponent<HellFire_Missile>());
        }
    }
    private void Score()
    {
        UI_Manager.instance.score += score;
    }
    private void Credit()
    {
        UI_Manager.instance.credit += credit;
    }
    private void KillCount()
    {
        EnemyController.instance.killCount += 1;
    }
    public override void PostHit(WeaponData data)
    {
        Debug.Log($"data : {data}");
        Debug.Log($"data.damage : {data.damage}");
        Debug.Log($"totalData.currentHp : {t90.totalData.currentHp}");
        t90.totalData.currentHp -= data.damage;
        if (t90.totalData.currentHp <= 0)
        {
            Dead();
        }
    }
    private void Dead()
    {
        EventManager.instance.PostEvent("addTankScore", null);
        EventManager.instance.PostEvent("addTankCredit", null);
        EventManager.instance.PostEvent("addKillCount", null);
        GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);
        Refresh();
        Destroy(go, 3);
    }

    private void Refresh()
    {
        t90.totalData.currentHp = t90.totalData.maxHp;
        float RandomX = Random.Range(-100, 100);
        float RandomZ = Random.Range(800, 900);

        gameObject.SetActive(false);
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.GetChild(16).rotation = Quaternion.identity;
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        gameObject.transform.position = new Vector3(RandomX, 2, RandomZ);
    }
}
