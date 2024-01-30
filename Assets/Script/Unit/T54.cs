using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T54 : UnitData
{
    TankBullet tankBullet;
    GameObject deadEffect;
    GameObject bullet;

    Transform originPosition;
    [SerializeField]
    Transform respwanPosition;

    [SerializeField] private float moveSpeed = 20f; // �̵� �ӵ�
    [SerializeField] private float turnSpeed = 900f; // ȸ�� �ӵ�
    [SerializeField] private float turretTurnSpeed = 1000f; // ȸ�� �ӵ�
    [SerializeField] private float shootRange = 100f; // ��� ����
    [SerializeField] private float shootInterval = 4f; // ��� ����
    [SerializeField] private Transform turretTransform; // ��ž Transform ������Ʈ

    private GameObject enemy; // ���� Transform ������Ʈ
    private Rigidbody tankRigidbody; // ��ũ�� Rigidbody ������Ʈ
    private float lastShootTime; // ������ ��� �ð�
    [SerializeField] private Transform bulletPosition;
    T54_InitStatus t54_InitStatus;
    private int score = 100;

    public LayerMask enemyLayer;

    private float damage = 100;

    T54 t54;

    public T54()
    {
        totalData = new T54_InitStatus();
        myData = new T54_InitStatus();
    }
    private void Start()
    {
        t54 = new T54();
        respwanPosition = GameObject.Find("Transform :: AllyRespwanPoint").transform;
        originPosition = transform;
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
        if (Physics.SphereCast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 200, gameObject.transform.position.z), shootRange, Vector3.down, out RaycastHit hit, 1000, enemyLayer))
        {
            enemy = hit.collider.gameObject; 

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            turretTransform.LookAt(enemy.transform);
            if (distanceToEnemy <= shootRange + 10) // ��� ���� ���� ���� ��
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
                enemy = null;
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
            else
            {
                Vector3 directionToBase = originPosition.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToBase);
                tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));
            }
        }
        if (enemy != null)
            if (enemy.activeInHierarchy == false)
            {
                enemy = null;
            }
    }
    private void Shoot()
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/T54Bullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        bullet.transform.rotation = bulletPosition.transform.rotation;
        tankBullet = bullet.GetComponent<TankBullet>();
        tankBullet.Damage(ref damage);
        Debug.Log($"T54 damage: {damage}");
        Destroy(bullet, 3);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            PostHit(other.gameObject.GetComponent<TankBullet>());
        }
    }
    private void Score()
    {
        UI_Manager.instance.score += score;
    }
    public override void PostHit(WeaponData data)
    {
        t54.totalData.currentHp -= data.GetComponent<TankBullet>().damage;
        if (t54.totalData.currentHp <= 0)
        {
            Dead();
        }
    }
    private void Dead()
    {
        GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        Refresh();
        Destroy(go, 3);
    }
    private void Refresh()
    {
        t54.totalData.currentHp = t54.totalData.maxHp;
        gameObject.transform.position = respwanPosition.position;
        gameObject.transform.rotation = Quaternion.identity;
    }
}
