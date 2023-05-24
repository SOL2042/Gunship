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
    [SerializeField] private float shootRange = 100f;
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
    private int credit = 500;
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
        EventManager.instance.AddListener("addTankCredit", (p) =>
        {
            Credit();
        });
        bulletPosition = transform.GetChild(16).GetChild(0).GetChild(0).transform;
        bullet = Resources.Load<GameObject>("Prefabs/T90Bullet");
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� Transform ������Ʈ ��������
        tankRigidbody = GetComponent<Rigidbody>(); // ��ũ�� Rigidbody ������Ʈ ��������
        turretTransform = transform.GetChild(16).transform;
    }

    private void Update()
    {
        //Debug.Log(enemy);
        Move();
        //USbaseTransform = GameObject.FindWithTag("USBase").transform;
        //Debug.Log(USbaseTransform.name);
    }
    private void Move()
    {
        if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 40, gameObject.transform.position.z), shootRange, Vector3.down, out RaycastHit hit, 1000, enemyLayer))
        {
            enemy = hit.collider.gameObject; //GameObject.FindWithTag("Enemy").transform;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            turretTransform.LookAt(enemy.transform);
            
            //Debug.Log(turretTransform.rotation);
            if (distanceToEnemy <= shootRange) // ��� ���� ���� ���� ��
            {
                moveSpeed = 0;

                if (Time.time - lastShootTime >= shootInterval) // ��� ������ ���� ���
                {
                    Shoot(); // ���
                    lastShootTime = Time.time; // ������ ��� �ð� ����
                }
            }
            else
            {
                moveSpeed = 20;
                Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
                tankRigidbody.MovePosition(tankRigidbody.position + movement);
            }

        }
        else // ��� ���� �ۿ� ���� ��
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
    private void Credit()
    {
        UI_Manager.instance.credit += credit;
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
        EventManager.instance.PostEvent("addTankCredit", null);
        GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);

        float RandomX = Random.Range(-100, 100);
        float RandomZ = Random.Range(800, 900);

        gameObject.SetActive(false);
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.GetChild(16).rotation = Quaternion.identity;
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        gameObject.transform.position = new Vector3(RandomX, 2, RandomZ);
        Destroy(go, 3);
    }
    public override void SetHit(UnitData data)
    {
        
    }
}
