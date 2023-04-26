using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : UnitData
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

    private Transform playerTransform; // �÷��̾��� Transform ������Ʈ
    private Transform USbaseTransform; // �÷��̾� ������ Transform ������Ʈ
    private Rigidbody tankRigidbody; // ��ũ�� Rigidbody ������Ʈ
    private float lastShootTime; // ������ ��� �ð�
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
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� Transform ������Ʈ ��������
        USbaseTransform = GameObject.FindGameObjectWithTag("USBase").transform;
        tankRigidbody = GetComponent<Rigidbody>(); // ��ũ�� Rigidbody ������Ʈ ��������
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
        // �������� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, USbaseTransform.position);

        if (distanceToPlayer <= totalData.unitshootRange) // ��� ���� ���� ���� ��
        {
            //moveSpeed = 0;
            Vector3 directionToPlayer = USbaseTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            
            if (Time.time - lastShootTime >= shootInterval) // ��� ������ ���� ���
            {
                Shoot(); // ���
                lastShootTime = Time.time; // ������ ��� �ð� ����
            }
        }
        else // ��� ���� �ۿ� ���� ��
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

                    // ��ũ�� �÷��̾� ������ �̵�
                    Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                    tankRigidbody.MovePosition(tankRigidbody.position + movement);
                }
            }
            
        }
    }

    private void Shoot()
    {
        // ��� ���� ����
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
