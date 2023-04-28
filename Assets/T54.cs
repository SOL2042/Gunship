using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T54 : UnitData
{
    TankBullet tankBullet;
    GameObject deadEffect;
    GameObject bullet;

    [SerializeField] private float moveSpeed = 20f; // �̵� �ӵ�
    [SerializeField] private float turnSpeed = 900f; // ȸ�� �ӵ�
    [SerializeField] private float turretTurnSpeed = 70f; // ȸ�� �ӵ�
    //[SerializeField] private float shootRange = 100f; // ��� ����
    [SerializeField] private float shootInterval = 4f; // ��� ����
    [SerializeField] private Transform turretTransform; // ��ž Transform ������Ʈ

    private Transform enemyTransform; // �÷��̾��� Transform ������Ʈ
    private Rigidbody tankRigidbody; // ��ũ�� Rigidbody ������Ʈ
    private float lastShootTime; // ������ ��� �ð�
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
        tankRigidbody = GetComponent<Rigidbody>(); // ��ũ�� Rigidbody ������Ʈ ��������
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
                    if (distanceToEnemy <= t54_InitStatus.unitshootRange) // ��� ���� ���� ���� ��
                    {
                        moveSpeed = 0;
                        Vector3 directionToEnemy = enemyTransform.position - transform.position;
                        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                        turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                    
                        if (Time.time - lastShootTime >= shootInterval) // ��� ������ ���� ���
                        {
                            Shoot(); // ���
                            lastShootTime = Time.time; // ������ ��� �ð� ����
                        }
                    }
                    else // ��� ���� �ۿ� ���� ��
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
                    
                        // ��ũ�� �÷��̾� ������ �̵�
                        Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                        tankRigidbody.MovePosition(tankRigidbody.position + movement);
                    }
                    
                }
            }
        }
        
        
    }

    private void Shoot()
    {
        // ��� ���� ����
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
