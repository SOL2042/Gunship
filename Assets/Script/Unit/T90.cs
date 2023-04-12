using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : MonoBehaviour 
{
    TankBullet tankBullet;

    GameObject explosionPrefab;
    GameObject bullet;

    [SerializeField] private float moveSpeed = 20f; // �̵� �ӵ�
    [SerializeField] private float turnSpeed = 900f; // ȸ�� �ӵ�
    [SerializeField] private float turretTurnSpeed = 70f; // ȸ�� �ӵ�
    [SerializeField] private float shootRange = 100f; // ��� ����
    [SerializeField] private float shootInterval = 4f; // ��� ����
    [SerializeField] private Transform turretTransform; // ��ž Transform ������Ʈ

    private Transform playerTransform; // �÷��̾��� Transform ������Ʈ
    private Transform USbaseTransform; // �÷��̾� ������ Transform ������Ʈ
    private Rigidbody tankRigidbody; // ��ũ�� Rigidbody ������Ʈ
    private float lastShootTime; // ������ ��� �ð�

    [SerializeField] private Transform bulletPosition;


    private void Start()
    {
        
        bulletPosition = transform.GetChild(16).GetChild(0).GetChild(0).transform;
        bullet = Resources.Load<GameObject>("Prefabs/TankBullet");
        explosionPrefab = Resources.Load<GameObject>("Prefabs/BigExplosion");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� Transform ������Ʈ ��������
        USbaseTransform = GameObject.FindGameObjectWithTag("USBase").transform;
        tankRigidbody = GetComponent<Rigidbody>(); // ��ũ�� Rigidbody ������Ʈ ��������
        turretTransform = GameObject.FindWithTag("Enemy").transform.GetChild(16).transform;
    }

    private void Update()
    {
        Debug.Log(turretTransform);
        Move();
    }

    private void Move()
    {
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= shootRange) // ��� ���� ���� ���� ��
        {
            moveSpeed = 0;
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            turretTransform.LookAt(directionToPlayer);
            
            tankBullet = new TankBullet(playerTransform.position, true);
            
            if (Time.time - lastShootTime >= shootInterval) // ��� ������ ���� ���
            {
                Shoot(); // ���
                lastShootTime = Time.time; // ������ ��� �ð� ����
            }
        }
        else // ��� ���� �ۿ� ���� ��
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            directionToPlayer.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));

            // ��ũ�� �÷��̾� ������ �̵�
            Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
            tankRigidbody.MovePosition(tankRigidbody.position + movement);
        }
    }



    private void Shoot()
    {
        // ��� ���� ����
        //transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/TankBullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        
        Destroy(bullet, 3);
        Debug.Log("Shoot!");
    }

    private void Dead()
    {
        Debug.Log("�׾���!");
    }
    
}
