using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : UnitData
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

    private Transform playerTransform; // 플레이어의 Transform 컴포넌트
    private Transform USbaseTransform; // 플레이어 기지의 Transform 컴포넌트
    private Rigidbody tankRigidbody; // 탱크의 Rigidbody 컴포넌트
    private float lastShootTime; // 마지막 사격 시간
    private float deadEffectTime = 3;
    private float deadEffectTimer = 3;
    [SerializeField] private Transform bulletPosition;
    T90_InitStatus t90_InitStatus;
    private int score = 100;

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
        bullet = Resources.Load<GameObject>("Prefabs/TankBullet");
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 컴포넌트 가져오기
        USbaseTransform = GameObject.FindGameObjectWithTag("USBase").transform;
        tankRigidbody = GetComponent<Rigidbody>(); // 탱크의 Rigidbody 컴포넌트 가져오기
        turretTransform = transform.GetChild(16).transform;
    }

    private void Update()
    {
        Move();
        USbaseTransform = GameObject.FindWithTag("USBase").transform;
        //Debug.Log(USbaseTransform.name);
    }

    private void Move()
    {
        // 기지와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, USbaseTransform.position);

        if (distanceToPlayer <= totalData.unitshootRange) // 사격 범위 내에 있을 때
        {
            //moveSpeed = 0;
            Vector3 directionToPlayer = USbaseTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            
            if (Time.time - lastShootTime >= shootInterval) // 사격 간격이 지난 경우
            {
                Shoot(); // 사격
                lastShootTime = Time.time; // 마지막 사격 시간 갱신
            }
        }
        else // 사격 범위 밖에 있을 때
        {
            if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z), 3, Vector3.down, out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer == 11 && hit.collider.gameObject != gameObject)
                {
                    moveSpeed = 0;
                    Debug.Log($"{gameObject.name} : {hit}");
                }
                else
                {
                    moveSpeed = 20f;

                    Vector3 directionToPlayer = USbaseTransform.position - transform.position;
                    directionToPlayer.y = 0f;

                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));

                    // 탱크를 플레이어 쪽으로 이동
                    Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                    tankRigidbody.MovePosition(tankRigidbody.position + movement);
                }
            }
            
        }
    }

    private void Shoot()
    {
        // 사격 로직 구현
        //transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/TankBullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        bullet.transform.rotation = bulletPosition.transform.rotation;
        
        Destroy(bullet, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
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
