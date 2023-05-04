using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : UnitData
{
    TankBullet tankBullet;
    GameObject deadEffect;
    GameObject bullet;

    [SerializeField] private float moveSpeed = 20f; // �̵� �ӵ�
    [SerializeField] private float turnSpeed = 100f; // ȸ�� �ӵ�
    [SerializeField] private float turretTurnSpeed = 70f; // ȸ�� �ӵ�
    [SerializeField] private float shootRange = 200f;
    //[SerializeField] private float shootRange = 100f; // ��� ����
    [SerializeField] private float shootInterval = 4f; // ��� ����
    [SerializeField] private Transform turretTransform; // ��ž Transform ������Ʈ

    private Transform playerTransform; // �÷��̾��� Transform ������Ʈ
    private Transform USbaseTransform; // �÷��̾� ������ Transform ������Ʈ
    private Rigidbody tankRigidbody; // ��ũ�� Rigidbody ������Ʈ
    private float lastShootTime; // ������ ��� �ð�
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
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� Transform ������Ʈ ��������
        USbaseTransform = GameObject.FindGameObjectWithTag("USBase").transform;
        tankRigidbody = GetComponent<Rigidbody>(); // ��ũ�� Rigidbody ������Ʈ ��������
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
            if (distanceToEnemy <= shootRange + 50) // ��� ���� ���� ���� ��
            {
                Debug.Log($"enemy : {hit.collider.name}");
                moveSpeed = 0;
                //Vector3 directionToEnemy = enemy.transform.position - transform.position;
                //Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                //Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turretTurnSpeed * Time.deltaTime);

                if (Time.time - lastShootTime >= shootInterval) // ��� ������ ���� ���
                {
                    Shoot(); // ���
                    lastShootTime = Time.time; // ������ ��� �ð� ����
                    
                }
            }
            

        }
        else // ��� ���� �ۿ� ���� ��
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

            //��ũ�� �� ������ �̵�


        }
    }
    //private void Move()
    //{
    //    // �������� �Ÿ� ���
    //    float distanceToPlayer = Vector3.Distance(transform.position, USbaseTransform.position);

    //    if (distanceToPlayer <= t90_InitStatus.unitshootRange) // ��� ���� ���� ���� ��
    //    {
    //        //moveSpeed = 0;
    //        Vector3 directionToPlayer = USbaseTransform.position - transform.position;
    //        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
    //        turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            
    //        if (Time.time - lastShootTime >= shootInterval) // ��� ������ ���� ���
    //        {
    //            Shoot(); // ���
    //            lastShootTime = Time.time; // ������ ��� �ð� ����
    //        }
    //    }
    //    else // ��� ���� �ۿ� ���� ��
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
               
    //            // ��ũ�� �÷��̾� ������ �̵�
    //            Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
    //            tankRigidbody.MovePosition(tankRigidbody.position + movement);
    //        }
            
    //    }
    //}

    private void Shoot()
    {
        // ��� ���� ����
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
