using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T54 : UnitData
{
    TankBullet tankBullet;
    GameObject deadEffect;
    GameObject bullet;

    [SerializeField] private float moveSpeed = 20f; // 이동 속도
    [SerializeField] private float turnSpeed = 900f; // 회전 속도
    [SerializeField] private float turretTurnSpeed = 70f; // 회전 속도
    //[SerializeField] private float shootRange = 100f; // 사격 범위
    [SerializeField] private float shootInterval = 4f; // 사격 간격
    [SerializeField] private Transform turretTransform; // 포탑 Transform 컴포넌트

    private Transform enemyTransform; // 플레이어의 Transform 컴포넌트
    private Rigidbody tankRigidbody; // 탱크의 Rigidbody 컴포넌트
    private float lastShootTime; // 마지막 사격 시간
    [SerializeField] private Transform bulletPosition;
    T54_InitStatus t54_InitStatus;
    private int score = 100;

    


    private void Start()
    {
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
        if (EnemyController.instance.t90s.Count == 0)
        {
            enemyTransform = null;
        }
        else
        {
            if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z), t54_InitStatus.unitshootRange, Vector3.down, out RaycastHit hit, 11))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.layer == 11)
                {
                    enemyTransform = hit.transform; //GameObject.FindWithTag("Enemy").transform;
                    float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);
                    if (distanceToEnemy <= t54_InitStatus.unitshootRange) // 사격 범위 내에 있을 때
                    {
                        moveSpeed = 0;
                        Vector3 directionToEnemy = enemyTransform.position - transform.position;
                        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                        turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                    
                        if (Time.time - lastShootTime >= shootInterval) // 사격 간격이 지난 경우
                        {
                            Shoot(); // 사격
                            lastShootTime = Time.time; // 마지막 사격 시간 갱신
                        }
                    }
                    else // 사격 범위 밖에 있을 때
                    {
                        moveSpeed = 20f;
                    
                        Vector3 directionToPlayer = enemyTransform.position - transform.position;
                        directionToPlayer.y = 0f;
                    
                        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                        //if (tankRigidbody.rotation != targetRotation)
                        //{
                        //    tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));
                        //    //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                        //}
                        //else
                        //{
                        //
                        //}
                    
                        // 탱크를 플레이어 쪽으로 이동
                        Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                        tankRigidbody.MovePosition(tankRigidbody.position + movement);
                    }
                    
                }
            }
        }
        
        
    }

    private void Shoot()
    {
        // 사격 로직 구현
        //transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/T54Bullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        bullet.transform.rotation = bulletPosition.transform.rotation;

        Destroy(bullet, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
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
        gameObject.transform.position = new Vector3(EnemyController.instance.RandomX, 0, EnemyController.instance.RandomZ);
        Destroy(go, 3);
    }



    public override void SetHit(UnitData data)
    {

    }
}
