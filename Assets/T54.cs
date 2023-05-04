using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T54 : UnitData
{
    TankBullet tankBullet;
    GameObject deadEffect;
    GameObject bullet;

    Transform originPosition;

    [SerializeField] private float moveSpeed = 20f; // 이동 속도
    [SerializeField] private float turnSpeed = 900f; // 회전 속도
    [SerializeField] private float turretTurnSpeed = 1000f; // 회전 속도
    [SerializeField] private float shootRange = 200f; // 사격 범위
    [SerializeField] private float shootInterval = 4f; // 사격 간격
    [SerializeField] private Transform turretTransform; // 포탑 Transform 컴포넌트

    private GameObject enemy; // 적의 Transform 컴포넌트
    private Rigidbody tankRigidbody; // 탱크의 Rigidbody 컴포넌트
    private float lastShootTime; // 마지막 사격 시간
    [SerializeField] private Transform bulletPosition;
    T54_InitStatus t54_InitStatus;
    private int score = 100;

    public LayerMask enemyLayer;

    private void Start()
    {
        originPosition = transform;
        t54_InitStatus = new T54_InitStatus();
        EventManager.instance.AddListener("addTankScore", (p) =>
        {
            Score();
        });
        bulletPosition = transform.GetChild(3).GetChild(0).GetChild(0).transform;
        bullet = Resources.Load<GameObject>("Prefabs/T54Bullet");
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
        tankRigidbody = GetComponent<Rigidbody>(); // 탱크의 Rigidbody 컴포넌트 가져오기
        turretTransform = transform.GetChild(3).transform;
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 200, gameObject.transform.position.z), shootRange, Vector3.down, out RaycastHit hit, 1000, enemyLayer))
        {
            Debug.Log(hit.collider.gameObject.layer);
            Debug.DrawRay(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 50, gameObject.transform.position.z), Vector3.down * 100, Color.red, 1000);
            
            enemy = hit.collider.gameObject; //GameObject.FindWithTag("Enemy").transform;
            
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            Debug.Log(distanceToEnemy);
            turretTransform.LookAt(enemy.transform);
            if (distanceToEnemy <= shootRange + 50) // 사격 범위 내에 있을 때
            {
                Debug.Log($"enemy : {enemy}");
                moveSpeed = 0;
                //Vector3 directionToEnemy = enemy.transform.position - transform.position;
                //Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                //Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turretTurnSpeed * Time.deltaTime);

                if (Time.time - lastShootTime >= shootInterval) // 사격 간격이 지난 경우
                {
                    Shoot(); // 사격
                    lastShootTime = Time.time; // 마지막 사격 시간 갱신
                }
            }
            
        }
        else // 사격 범위 밖에 있을 때
        {
            if (enemy != null)
            {
                moveSpeed = 20f;
                Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                tankRigidbody.MovePosition(tankRigidbody.position + movement);
            }
            else
            {
                Vector3 directionToBase = originPosition.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToBase);
                tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));
            }

            
            //Vector3 directionToEnemy = enemy.transform.position - transform.position;
            //directionToEnemy.y = 0f;

            //Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            //if (tankRigidbody.rotation != targetRotation)
            //{
            //    tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));
            //    //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            //}
            //else
            //{
            //
            //}

            //탱크를 적 쪽으로 이동
            
            
        }
    }

    private void Shoot()
    {
        // 사격 로직 구현
        GameObject go = Resources.Load<GameObject>("Prefabs/T54Bullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        bullet.transform.rotation = bulletPosition.transform.rotation;

        Destroy(bullet, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Dead();
        }
    }

    private void Score()
    {
        UI_Manager.instance.score += score;
    }

    public override void PostHit(UnitData data, RaycastHit hit)
    {
        totalData.currentHp -= data.totalData.bulletAtk;

        if (totalData.currentHp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(EnemyController.instance.RandomX, 0, EnemyController.instance.RandomZ);
        Destroy(go, 3);
    }

    public override void SetHit(UnitData data)
    {

    }
}
