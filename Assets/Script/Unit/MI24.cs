using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MI24 : UnitData
{
    MI24 mi_24;
    Transform target;

    [SerializeField]
    float maxSpeed;                             // �ִ� ���ǵ�
    [SerializeField]
    float minSpeed;                             // �ּ� ���ǵ�
    [SerializeField]
    float defaultSpeed;                         // �⺻ �ӵ�

    float speed;                                // ��ü �ӵ�

    [SerializeField]
    float speedLerpAmount;                      // ���ӷ�
    [SerializeField]
    float turningForce;                         // ��ȸ��
    [SerializeField]
    float turningTime;                          // ��ȸ�ð�

    [SerializeField]
    List<Transform> initialWaypoints;           // �ʱ� ��������Ʈ ����Ʈ
    Queue<Transform> waypointQueue;             // ��������Ʈ �ִ� ť

    Transform currentWaypoint;                  // ���� ��������Ʈ

    float prevWaypointDistance;                 // ������ ��������Ʈ �Ÿ�
    float waypointDistance;                     // ���� ���� ��������Ʈ ������ �Ÿ�
    bool isComingClose;                         // ����������� Ȯ��

    float prevRotY;                             // ������ Y ȸ��
    float currRotY;                             // ������ Y ȸ��
    float rotateAmount;                         // ȸ����
    float zRotateValue;                         // zȸ����
    [SerializeField]
    float zRotateMaxThreshold = 0.5f;
    [SerializeField]
    float zRotateAmount = 90;

    private float shootInterval = 0.1f;    // ��� ����
    [SerializeField] private float lastShootTime = 0f;      // ��� ����
    private int score = 500;                                // ���ھ�  
    private int credit = 20000;                             // ũ����

    [SerializeField] GameObject deadEffect;                 // �״� ����Ʈ
    [SerializeField] Transform bulletPosition;              // �Ѿ� ������
    List<GameObject> bullets;

    float bulletDamage = 10f;
    void ChangeWaypoint()                                    // ��������Ʈ ���� �Լ�
    {
        if (waypointQueue.Count == 0)                           // waypointQueue ���� ������ 0�̶��
        {
            currentWaypoint = null;                             // ���� ��������Ʈ�� null
            return;                                             // �������´�
        }

        currentWaypoint = waypointQueue.Dequeue();                                                  // ���� ��������Ʈ�� Queue�� ����Ǿ��ִ� ������ ��������Ʈ ��ȯ �� ���� 
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint.position);          // ���� ���� ��������Ʈ ������ �Ÿ�
        prevWaypointDistance = waypointDistance;                                                    // ������ ��������Ʈ �Ÿ��� ����

        isComingClose = false;                                                                      // ����������� Ȯ���ϴ� ���� false
    }

    void CheckWaypoint()                                                                                    // ��������Ʈ üũ�ϴ� �Լ�
    {
        if (currentWaypoint == null) return;                                                                // ���� ��������Ʈ�� ���� ��� Ż��
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint.position);                  // ���� ��������Ʈ�� ������ �Ÿ�

        if (waypointDistance >= prevWaypointDistance) // Aircraft is going farther from the waypoint        // ���� ��������Ʈ�� �Ÿ��� ���� ��������Ʈ�� �Ÿ����� ũ�ų� ������
        {
            if (isComingClose == true)                                                                      // ��������Ʈ�� ��������ٸ�
            {
                //ChangeWaypoint();                                                                         // ��������Ʈ ����
            }
        }
        else                                                                                                // ���� ��������Ʈ�� �Ÿ��� �� ũ�ٸ�
        {
            isComingClose = true;                                                                           // isComingClose�� true�� ����
        }

        prevWaypointDistance = waypointDistance;                                                            // ���� ��������Ʈ�� ����
    }

    void Rotate()
    {
        if (currentWaypoint == null)
            return;

        Vector3 targetDir = currentWaypoint.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(targetDir);

        float delta = Quaternion.Angle(transform.rotation, lookRotation);
        if (delta > 0f)
        {
            float lerpAmount = Mathf.SmoothDampAngle(delta, 0.0f, ref rotateAmount, turningTime);
            lerpAmount = 1.0f - (lerpAmount / delta);

            Vector3 eulerAngle = lookRotation.eulerAngles;
            eulerAngle.z += zRotateValue * zRotateAmount;
            lookRotation = Quaternion.Euler(eulerAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lerpAmount);
        }
    }

    void ZAxisRotate()
    {
        currRotY = transform.eulerAngles.y;
        float diff = prevRotY - currRotY;
        if (diff < -180) diff += 360;
        if (diff > 180) diff -= 360;

        prevRotY = transform.eulerAngles.y;
        zRotateValue = Mathf.Lerp(zRotateValue, Mathf.Clamp(diff / zRotateMaxThreshold, -1, 1), turningForce * Time.deltaTime);
    }

    void Move()
    {
        transform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
    }

    void Start()
    {
        bullets = new List<GameObject>();
        mi_24 = new MI24();

        target = FindObjectOfType<Player>().transform;

        speed = defaultSpeed;

        turningTime = 1 / turningForce;

        waypointQueue = new Queue<Transform>();
        foreach (Transform t in initialWaypoints)
        {
            waypointQueue.Enqueue(t);
        }
        ChangeWaypoint();

        EventManager.instance.AddListener("addMI24Score", (p) =>
        {
            Score();
        });
        EventManager.instance.AddListener("addMI24Credit", (p) =>
        {
            Credit();
        });
        bulletPosition = transform.GetChild(7).transform;
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
    }

    void Update()
    {
        currentWaypoint = target;

        CheckWaypoint();
        Rotate();
        ZAxisRotate();
        if (waypointDistance >= 200f)
        {
            Move();
        }
        else
        {
            transform.Translate(new Vector3(10, 0, 0) * Time.deltaTime);
            if (Time.time - lastShootTime >= shootInterval)
            {
                if (target.gameObject.activeInHierarchy)
                {
                    if (bullets.Count <= 10)
                    {
                        Fire(); // ���
                        lastShootTime = Time.time; // ��������� �ð� ����
                    }
                    else
                    {
                        if(Time.time - lastShootTime >= 2)
                        {
                            lastShootTime = Time.time;
                            bullets.Clear();
                        }
                    }
                }
            }
        }

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

    private void Dead()
    {
        EventManager.instance.PostEvent("addMI24Score", null);
        EventManager.instance.PostEvent("addMI24Credit", null);
        GameObject go = Instantiate(deadEffect, transform.position, Quaternion.identity);

        float RandomX = Random.Range(-100, 100);
        float RandomZ = Random.Range(800, 900);

        gameObject.SetActive(false);
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = new Vector3(RandomX, 2, RandomZ);
        Destroy(go, 3);
    }

    public MI24()
    {
        totalData += new Mi24_Status();
        myData += new Mi24_Status();
    }

    private void Score()
    {
        UI_Manager.instance.score += score;
    }
    private void Credit()
    {
        UI_Manager.instance.credit += credit;
    }
    private void Fire()
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/MI24_Bullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        bullet.transform.rotation = bulletPosition.transform.rotation;
        MI24_Bullet bulletScript = bullet.GetComponent<MI24_Bullet>();
        bulletScript.Damage(ref bulletDamage);
        bullets.Add(bullet);
        Destroy(bullet, 3);
    }
    public override void PostHit(WeaponData data)
    {
        Debug.Log($"data : {data}");
        Debug.Log($"data.damage : {data.damage}");
        Debug.Log($"totalData.currentHp : {mi_24.totalData.currentHp}");
        mi_24.totalData.currentHp -= data.damage;
        if (mi_24.totalData.currentHp <= 0)
        {
            Dead();
        }
        
    }
}
