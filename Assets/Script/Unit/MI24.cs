using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MI24 : UnitData
{
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
    List<Transform> initialWaypoints;           // ��������Ʈ ����Ʈ
    Queue<Transform> waypointQueue;             // ��������Ʈ ť

    Transform currentWaypoint;                  // ���� ��������Ʈ

    float prevWaypointDistance;                 // ������ ��������Ʈ �Ÿ�
    float waypointDistance;                     // ��������Ʈ �Ÿ�
    bool isComingClose;                         // ����������� Ȯ��

    float prevRotY;                             // ������ Y ȸ��
    float currRotY;                             // ������ Y ȸ��
    float rotateAmount;                         // ȸ����
    float zRotateValue;                         // zȸ����

    [SerializeField] private float shootInterval = 0.3f; // ��� ����
    [SerializeField] private float lastShootTime = 0f; // ��� ����
    private int score = 500;
    private int credit = 20000;
    private GameObject enemy;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject deadEffect;

    [SerializeField] Transform bulletPosition;

    void ChangeWaypoint()
    {
        if (waypointQueue.Count == 0)
        {
            currentWaypoint = null;
            return;
        }

        currentWaypoint = waypointQueue.Dequeue();
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint.position);
        prevWaypointDistance = waypointDistance;

        isComingClose = false;
    }

    void CheckWaypoint()
    {
        if (currentWaypoint == null) return;
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint.position);

        if (waypointDistance >= prevWaypointDistance) // Aircraft is going farther from the waypoint
        {
            if (isComingClose == true)
            {
                //ChangeWaypoint();
            }
        }
        else
        {
            isComingClose = true;
        }

        prevWaypointDistance = waypointDistance;
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
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lerpAmount);
        }
    }

    void ZAxisRotate()
    {
        currRotY = transform.eulerAngles.y;
        float diff = prevRotY - currRotY;
        if (diff < -180) diff += 360;
        else if (diff > 180) diff -= 360;

        zRotateValue = Mathf.Lerp(transform.eulerAngles.z, Mathf.Clamp(diff / Time.deltaTime, -90, 90), turningForce);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, zRotateValue);
        prevRotY = transform.eulerAngles.y;
    }

    void Move()
    {
        transform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
    }


    void Start()
    {
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
        bullet = Resources.Load<GameObject>("Prefabs/MI24_Bullet");
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
    }




    void Update()
    {
        currentWaypoint = target;

        CheckWaypoint();
        Rotate();
        //ZAxisRotate();
        if (waypointDistance >= 200f)
        {
            Move();
        }
        else
        {
            if (Time.time - lastShootTime >= shootInterval) // ��� ������ ���� ���
            {
                if (target.parent.gameObject.activeInHierarchy)
                {
                    Fire(); // ���
                    lastShootTime = Time.time; // ������ ��� �ð� ����
                }
            }
        }
        //Find();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Dead();
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

        Destroy(bullet, 3);

    }
    public override void PostHit(UnitData data, RaycastHit hit)
    {

    }
    public override void SetHit(UnitData data)
    {

    }



}
