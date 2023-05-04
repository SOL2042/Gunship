using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : UnitData
{
    TankBullet tankBullet;
    GameObject deadEffect;
    GameObject bullet;

    [SerializeField] private float moveSpeed = 20f; // 이동 속도
    [SerializeField] private float turnSpeed = 100f; // 회전 속도
    [SerializeField] private float turretTurnSpeed = 70f; // 회전 속도
    [SerializeField] private float shootRange = 200f;
    //[SerializeField] private float shootRange = 100f; // 사격 범위
    [SerializeField] private float shootInterval = 4f; // 사격 간격
    [SerializeField] private Transform turretTransform; // 포탑 Transform 컴포넌트

    private Transform playerTransform; // 플레이어의 Transform 컴포넌트
    private Transform USbaseTransform; // 플레이어 기지의 Transform 컴포넌트
    private Rigidbody tankRigidbody; // 탱크의 Rigidbody 컴포넌트
    private float lastShootTime; // 마지막 사격 시간
    private float deadEffectTime = 3;
    private float deadEffectTimer = 3;
    [SerializeField] private Transform bulletPosition;
    T90_InitStatus t90_InitStatus;
    private int score = 100;
    private GameObject enemy;

    public LayerMask enemyLayer;

    public T90()
    {
        t90_InitStatus = new T90_InitStatus();
    }


    private void Start()
    {
        EventManager.instance.AddListener("addTankScore", (p)=>
        {
            Score();
        });
        bulletPosition = transform.GetChild(16).GetChild(0).GetChild(0).transform;
        bullet = Resources.Load<GameObject>("Prefabs/T90Bullet");
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 컴포넌트 가져오기
        USbaseTransform = GameObject.FindGameObjectWithTag("USBase").transform;
        tankRigidbody = GetComponent<Rigidbody>(); // 탱크의 Rigidbody 컴포넌트 가져오기
        turretTransform = transform.GetChild(16).transform;
    }

    private void Update()
    {
        Debug.Log(enemy);
        Move();
        //USbaseTransform = GameObject.FindWithTag("USBase").transform;
        //Debug.Log(USbaseTransform.name);
    }
    private void Move()
    {
        if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 200, gameObject.transform.position.z), shootRange, Vector3.down, out RaycastHit hit, 1000, enemyLayer))
        {
            Debug.Log(hit.collider.gameObject.layer);
            Debug.DrawRay(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 50, gameObject.transform.position.z), Vector3.down * 100, Color.red, 1000);

            enemy = hit.collider.gameObject; //GameObject.FindWithTag("Enemy").transform;

            if (enemy.activeInHierarchy != true)
            {
                enemy = null;
            }
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            Debug.Log(distanceToEnemy);
            turretTransform.LookAt(enemy.transform);
            if (distanceToEnemy <= shootRange + 50) // 사격 범위 내에 있을 때
            {
                Debug.Log($"enemy : {hit.collider.name}");
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
            if (enemy == null)
            {
                moveSpeed = 20f;
                Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                tankRigidbody.MovePosition(tankRigidbody.position + movement);
            }
            //else
            //{
                //Vector3 directionToBase = originPosition.position - transform.position;
                //Quaternion targetRotation = Quaternion.LookRotation(directionToBase);
                //tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));
            //}


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
    //private void Move()
    //{
    //    // 기지와의 거리 계산
    //    float distanceToPlayer = Vector3.Distance(transform.position, USbaseTransform.position);

    //    if (distanceToPlayer <= t90_InitStatus.unitshootRange) // 사격 범위 내에 있을 때
    //    {
    //        //moveSpeed = 0;
    //        Vector3 directionToPlayer = USbaseTransform.position - transform.position;
    //        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
    //        turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            
    //        if (Time.time - lastShootTime >= shootInterval) // 사격 간격이 지난 경우
    //        {
    //            Shoot(); // 사격
    //            lastShootTime = Time.time; // 마지막 사격 시간 갱신
    //        }
    //    }
    //    else // 사격 범위 밖에 있을 때
    //    {
    //        if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z), 3, Vector3.down, out RaycastHit hit))
    //        {
    //            //Debug.Log($"{gameObject.name} : {hit}");
               
    //            moveSpeed = 20f;
               
    //            Vector3 directionToPlayer = USbaseTransform.position - transform.position;
    //            directionToPlayer.y = 0f;
               
    //            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
    //            //if (tankRigidbody.rotation != targetRotation)
    //            //{
    //                //tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));
    //                ////transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    //            //}
    //            //else
    //            //{
               
    //            //}
               
    //            // 탱크를 플레이어 쪽으로 이동
    //            Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
    //            tankRigidbody.MovePosition(tankRigidbody.position + movement);
    //        }
            
    //    }
    //}

    private void Shoot()
    {
        // 사격 로직 구현
        //transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/T90Bullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        bullet.transform.rotation = bulletPosition.transform.rotation;
        
        Destroy(bullet, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
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
        EventManager.instance.PostEvent("addTankScore", null);
        GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(EnemyController.instance.RandomX, 2, EnemyController.instance.RandomZ);
        Destroy(go, 3);
    }

    

    public override void SetHit(UnitData data)
    {
        
    }
}
